using HtmlAgilityPack;
using Modelos.SAT;
using Newtonsoft.Json;
using System.Collections.Immutable;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text;
using static Modelos.SAT.Result;

namespace SAT
{
    public class Main
    {
        private readonly static WebServices _web = new WebServices();
        private readonly static Tools _tool = new Tools(); 
        public async Task RFC()
        {
            await INIT();
        }







        public async Task<string> SearchByCurp(string COOKIES, string VIEW_STATE, string CURP)
        {
            try
            {
                var _result = await _web.BuscarCURP(COOKIES, VIEW_STATE, CURP);

                var _headers = _result.Headers;
                var _content = await _result.Content.ReadAsStringAsync();
                var _html = _tool.StringHTML(_content);

                var _msg = _html.GetElementbyId("messageList").InnerHtml;
                var _tableResult = _html.GetElementbyId("visorForm:tablaResultados").InnerHtml;
                var _viewState = _html.GetElementbyId("javax.faces.ViewState").InnerHtml;


                var _i = _viewState.IndexOf("CDATA");
                var _l = _viewState.Length;
                var _view = _viewState.Substring(_i + 6, _l - _i - 9);
                var _iR = _tableResult.IndexOf("CDATA");
                var _lR = _tableResult.Length;
                var _data = _tableResult.Substring(_iR + 6, _lR - _iR - 9);

                var _tResult = new HtmlDocument();
                _tResult.LoadHtml(_data);

                var _tablaResultadosHTML = new HtmlDocument();
                _tablaResultadosHTML.LoadHtml(_tResult.GetElementbyId("visorForm:tablaResultados_data").InnerHtml);
                var _trTable = _tablaResultadosHTML.DocumentNode.SelectNodes("//tr");
                var _labels = _trTable[0].SelectNodes("//label");

                //Get Main Data
                var name = "";
                var pApellido = "";
                var sApellido = "";
                var rfc = "";
                var curp = "";
                var ciudad = "";
                var source = "";
                for (var i = 0; i < _labels.Count - 2; i++)
                {
                    var _current = _labels[i];
                    var _id = _current.Id;
                    var _label = _current.InnerHtml;
                    switch (i)
                    {
                        case 0:
                            name = _label;
                            break;
                        case 1:
                            pApellido = _label;
                            break;
                        case 2:
                            sApellido = _label;
                            break;
                        case 3:
                            source = _id;
                            rfc = _label;
                            break;
                        case 4:
                            curp = _label;
                            break;
                        case 5:
                            ciudad = _label;
                            break;
                        default:
                            break;
                    }

                }


                //var _onClick = await _web.ClickRFC(COOKIES, VIEW_STATE, curp, source);
                //var _onClickStr = await _onClick.Content.ReadAsStringAsync();

                //var _onInit = await _web.InitVisualizador(COOKIES);
                //var _onInitTxt = await _onInit.Content.ReadAsStringAsync();

                //var _resumenTrib = await _web.HTMLResumen(COOKIES);
                //var _resumenTxt = await _resumenTrib.Content.ReadAsStringAsync();

                //var _resumentHTML = new HtmlDocument();
                //_resumentHTML.LoadHtml(_resumenTxt);



                ////18
                //var _tables = _resumentHTML.DocumentNode.SelectNodes("//table");
                //var _btnHandle = _tables[17].SelectNodes("//button");







                //var _onDownload = await _web.DownloadRFC(COOKIES, rfc);


                //var _warning = new HtmlDocument();
                //_warning.LoadHtml(_msg);
                return "";
            }
            catch(Exception ex)
            {
                var _err = ex.Message;

                Console.Write(_err.ToString());
                return "Caducó";
            }

        }



        private async Task INIT()
        {
            //Enviroment
            var _sessions = new Modelos.SAT.Cookies();


            // 0
            var _r = await _web.Init();

            var _header = _r.Headers;
            var _content = await _r.Content.ReadAsStringAsync();
            var _html = _tool.StringHTML(_content);
            var _guid = _html.GetElementbyId("guid").GetAttributes("value").FirstOrDefault()!.Value;
            var _lguid = _html.GetElementbyId("lGuid").GetAttributes("value").FirstOrDefault()!.Value;

            var _mH1 = _tool.ManipulateHeader(_header);
            
            // 1
            var _log = await _web.LogIn(_mH1.SetCookie, _guid, _lguid);
            var _headers = _log.Headers;
            var _status = _log.StatusCode;
            if (_status == System.Net.HttpStatusCode.OK)
            {
                var _res = _tool.ManipulateHeader(_headers);

                // No body contents
                // 2
                var _ssosid = await _web.SSOSID(_res.SetCookie);
                var _header2 = _tool.ManipulateHeader(_ssosid.Headers);

                // 3
                var _spassertion = await _web.SPASSERTION(_header2.SetCookie);
                var _header3 = _tool.ManipulateHeader(_spassertion.Headers);
                _sessions.IPC = _header3.IPC;

                // 4
                var _accessEF = await _web.AccessEF(_header3.SetCookie);
                var _header4 = _tool.ManipulateHeader(_accessEF.Headers);
                _sessions.RFC = _header4.RFC;

                // 5
                var _sso = await _web.SSO("PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiPz48c2FtbHA6QXV0aG5SZXF1ZXN0IHhtbG5zOnNhbWxwPSJ1cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6cHJvdG9jb2wiIERlc3RpbmF0aW9uPSJodHRwczovL2F1dGguc2lhdC5zYXQuZ29iLm14L25pZHAvc2FtbDIvc3NvIiBGb3JjZUF1dGhuPSJmYWxzZSIgSUQ9Il8weDBhNTc0OWIzM2IxY2M1NzMwZDkxZjkyMmE4ZWNmNDljIiBJc1Bhc3NpdmU9ImZhbHNlIiBJc3N1ZUluc3RhbnQ9IjIwMjItMTEtMjhUMTk6NDM6NTQuODc4WiIgVmVyc2lvbj0iMi4wIj48c2FtbDpJc3N1ZXIgeG1sbnM6c2FtbD0idXJuOm9hc2lzOm5hbWVzOnRjOlNBTUw6Mi4wOmFzc2VydGlvbiI+d2xyZmNhbXBlLnNpYXQuc2F0LmdvYi5teDwvc2FtbDpJc3N1ZXI+PC9zYW1scDpBdXRoblJlcXVlc3Q+", _header4.SetCookie);
                var _header5 = _tool.ManipulateHeader(_sso.Headers);

                // 6
                var _acs = await _web.ACS("PHNhbWxwOlJlc3BvbnNlIHhtbG5zOnNhbWxwPSJ1cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6cHJvdG9jb2wiIHhtbG5zOnNhbWw9InVybjpvYXNpczpuYW1lczp0YzpTQU1MOjIuMDphc3NlcnRpb24iIERlc3RpbmF0aW9uPSJodHRwczovL3JmY2FtcGUuc2lhdC5zYXQuZ29iLm14L3NhbWwyL3NwL2Fjcy9wb3N0IiBJRD0iaWRZOVp3cTVtbmdFaWRFR1BMUjlNdjN3X3pDV0kiIEluUmVzcG9uc2VUbz0iXzB4MGE1NzQ5YjMzYjFjYzU3MzBkOTFmOTIyYThlY2Y0OWMiIElzc3VlSW5zdGFudD0iMjAyMi0xMS0yOFQxOTo0Mzo1NVoiIFZlcnNpb249IjIuMCI+PHNhbWw6SXNzdWVyPmh0dHBzOi8vYXV0aC5zaWF0LnNhdC5nb2IubXgvbmlkcC9zYW1sMi9tZXRhZGF0YTwvc2FtbDpJc3N1ZXI+PHNhbWxwOlN0YXR1cz48c2FtbHA6U3RhdHVzQ29kZSBWYWx1ZT0idXJuOm9hc2lzOm5hbWVzOnRjOlNBTUw6Mi4wOnN0YXR1czpTdWNjZXNzIi8+PC9zYW1scDpTdGF0dXM+PHNhbWw6QXNzZXJ0aW9uIElEPSJpZHJKNVd0M1hoamJTZEltcWg2bHVsM2xsVTJWSSIgSXNzdWVJbnN0YW50PSIyMDIyLTExLTI4VDE5OjQzOjU1WiIgVmVyc2lvbj0iMi4wIj48c2FtbDpJc3N1ZXI+aHR0cHM6Ly9hdXRoLnNpYXQuc2F0LmdvYi5teC9uaWRwL3NhbWwyL21ldGFkYXRhPC9zYW1sOklzc3Vlcj48ZHM6U2lnbmF0dXJlIHhtbG5zOmRzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwLzA5L3htbGRzaWcjIj4KPGRzOlNpZ25lZEluZm8+CjxkczpDYW5vbmljYWxpemF0aW9uTWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS8xMC94bWwtZXhjLWMxNG4jIi8+CjxkczpTaWduYXR1cmVNZXRob2QgQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNyc2Etc2hhMjU2Ii8+CjxkczpSZWZlcmVuY2UgVVJJPSIjaWRySjVXdDNYaGpiU2RJbXFoNmx1bDNsbFUyVkkiPgo8ZHM6VHJhbnNmb3Jtcz4KPGRzOlRyYW5zZm9ybSBBbGdvcml0aG09Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyNlbnZlbG9wZWQtc2lnbmF0dXJlIi8+CjxkczpUcmFuc2Zvcm0gQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzEwL3htbC1leGMtYzE0biMiLz4KPC9kczpUcmFuc2Zvcm1zPgo8ZHM6RGlnZXN0TWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS8wNC94bWxlbmMjc2hhMjU2Ii8+CjxkczpEaWdlc3RWYWx1ZT45emVZc3B3cmhwRW9raUgyVkU1Q2Q5dVNycXZRU1NHWWZrYktzNmVmLzRjPTwvZHM6RGlnZXN0VmFsdWU+CjwvZHM6UmVmZXJlbmNlPgo8L2RzOlNpZ25lZEluZm8+CjxkczpTaWduYXR1cmVWYWx1ZT4KUjlhTnFYQ0h6VHdicTNNdjV4QkJVTWZpcWtJWDI0ZTNpQ0JzTVJZVVhxMWIwWGhkc2swNmNraVQ2WUJkNHZvd2E0cWdyU1ppOUlaMAowOWdnU0NFbk9kVjA3b21nMEx3WFNsZWtETm1kZTJoL3FKOHBZK1RGS09KTlFYQzllanB0TlNTWjI3ZlJSQ1ZNVkVqT3J4eWlMbkxuCjd4c3l1WmJVRUZteWdpR3laNFNnMXlkSnRHbHlLWnBKc2t1bzVNMVJTZThJeEZtNHB1MTJUSk1NcUprMHQvME90MXR0aEthSUZjdkgKRU1EaGl3UUJGQ1JSOVlnMUlteUt3cWJyNlVYMkFxcE1BbkM5REE4RU1xSGNWck5RMFlCUEVVdHh4SkgrbmFhMXd4dDBBMzZnc2dvagp1V0hwT0ZIYVZNUzhuMUpXNzZJTmhUdVNQa2QvejVaM2FQY3BiZz09CjwvZHM6U2lnbmF0dXJlVmFsdWU+CjxkczpLZXlJbmZvPgo8ZHM6WDUwOURhdGE+CjxkczpYNTA5Q2VydGlmaWNhdGU+Ck1JSUZMakNDQkJhZ0F3SUJBZ0lrQWh3Ui82VXBHblc5QTZsWGs1M043R0NTUmFGUDhPUGF1bEs4b0pvakFnSVdDOWU5TUEwR0NTcUcKU0liM0RRRUJCUVVBTURveEdqQVlCZ05WQkFzVEVVOXlaMkZ1YVhwaGRHbHZibUZzSUVOQk1Sd3dHZ1lEVlFRS0ZCTjBjV2xrYm5CeQpiMkZqWldVd01WOTBjbVZsTUI0WERURTBNRFl3T1RBeE1ESXpNVm9YRFRJME1EWXdPVEF4TURJek1Wb3dRREVWTUJNR0ExVUVBeE1NCmRHVnpkQzF6YVdkdWFXNW5NUll3RkFZRFZRUUxFdzFoWTJObGMzTk5ZVzVoWjJWeU1ROHdEUVlEVlFRS0V3WnViM1psYkd3d2dnRWkKTUEwR0NTcUdTSWIzRFFFQkFRVUFBNElCRHdBd2dnRUtBb0lCQVFDYU16Uis2bjBXRURrU2hiUjIyWHQxLzVxOWlPbEJJUzZSekZKMwpMZUdVSWRvcW9CWHNkYjZCUm5lc3dJcFNDTWxJLzFJODkvYXVIQ2ZXdGNTcXUzZHdZMi9XaEdyL3BRNllCTm5zK1JCVVM5M3BlaFllCk95OTNMU1VSc2tTM1QvWXE0MjU4NFRvRnlaYVpXc1ZnbGljUlorOU94bVhxQStWTEd3UkxmbS9yMnpiSDJ1aENrQStiV2tEVm04VzYKVzZNMWgrNHNkZW1pV0MyYW1YZVdmTkoxMmtKWFlpMElNYUYwbVJtN25ZOTZwQllTYldSRzk5R1JBUWpTaVBQYjRKUFBOaS9WRi8rTApGcld3OVM2MmtXUE1lZjJTdk5iVjdxMWJKMlBTUi9WTE8xajI3YWwxZ0tQTGFXdVhpNHRURVc5cW56QmtoVG5RemRWR2RzSGExaVpkCkFnTUJBQUdqZ2dJVU1JSUNFREFkQmdOVkhRNEVGZ1FVTjZzSU9YTVhIb2FuWmJ4ZlZObzdwbHBzK2hFd0h3WURWUjBqQkJnd0ZvQVUKZlpLblM2VEVid1hRRnFNMVVQeDRGNElWRXJvd2dnSE1CZ3RnaGtnQmh2ZzNBUWtFQVFTQ0Fic3dnZ0czQkFJQkFBRUIveE1kVG05MgpaV3hzSUZObFkzVnlhWFI1SUVGMGRISnBZblYwWlNoMGJTa1dRMmgwZEhBNkx5OWtaWFpsYkc5d1pYSXVibTkyWld4c0xtTnZiUzl5ClpYQnZjMmwwYjNKNUwyRjBkSEpwWW5WMFpYTXZZMlZ5ZEdGMGRISnpYM1l4TUM1b2RHMHdnZ0ZJb0JvQkFRQXdDREFHQWdFQkFnRkcKTUFnd0JnSUJBUUlCQ2dJQmFhRWFBUUVBTUFnd0JnSUJBUUlCQURBSU1BWUNBUUVDQVFBQ0FRQ2lCZ0lCRndFQi82T0NBUVNnV0FJQgpBZ0lDQVA4Q0FRQUREUUNBQUFBQUFBQUFBQUFBQUFBRENRQ0FBQUFBQUFBQUFEQVlNQkFDQVFBQ0NILy8vLy8vLy8vL0FRRUFBZ1FHCjhOOUlNQmd3RUFJQkFBSUlmLy8vLy8vLy8vOEJBUUFDQkFidzMwaWhXQUlCQWdJQ0FQOENBUUFERFFCQUFBQUFBQUFBQUFBQUFBQUQKQ1FCQUFBQUFBQUFBQURBWU1CQUNBUUFDQ0gvLy8vLy8vLy8vQVFFQUFnUVIvNlVwTUJnd0VBSUJBQUlJZi8vLy8vLy8vLzhCQVFBQwpCQkgvcFNtaVRqQk1BZ0VDQWdFQUFnSUEvd01OQUlBQUFBQUFBQUFBQUFBQUFBTUpBSUFBQUFBQUFBQUFNQkl3RUFJQkFBSUlmLy8vCi8vLy8vLzhCQVFBd0VqQVFBZ0VBQWdoLy8vLy8vLy8vL3dFQkFEQU5CZ2txaGtpRzl3MEJBUVVGQUFPQ0FRRUF0VVVFK0pCa1NYRWoKbkY1b2ZtVXlqWWUweG1ZNk44V3hjZVlxejVzb2tFcFpDWmVvNnd1YlRqNzE5Q0g4NE5pTjZVSE00YjBiVnhITEJJemtlcWVDb3ZCbAo4OXh3YWhvVVB3UVFjUzgrQVp3Q0FoVDVrcnkySlJma1ZnTlFYZ3BuMWt1di9hWUwzakxNb0p1YTJkS0NoVUdmdlN5TUxHVktJZlNZCllObmI4dTNyZ1dKdG9PNHFKM0NocCtxQ1Ura0RzSkx2c0FYNDRmWXloRnlDZ3dERDNvTk90M0NHMmU4NjlHUUpWUVAreGlPa1AxMHEKYmozRWx6cjMycnNiWDRyc0N4blArY3I2dG51eGFaZGpKNytjVEJ0clJzVDJqbHhJdDVLTUJqbWxNWDJyZEUzUXJZQk9iTVdTRnplMQpOM1JnRVB6QUwwRXFjOXR5RFNqcm0zQmZhQT09CjwvZHM6WDUwOUNlcnRpZmljYXRlPgo8L2RzOlg1MDlEYXRhPgo8L2RzOktleUluZm8+CjwvZHM6U2lnbmF0dXJlPjxzYW1sOlN1YmplY3Q+PHNhbWw6TmFtZUlEIEZvcm1hdD0idXJuOm9hc2lzOm5hbWVzOnRjOlNBTUw6MS4xOm5hbWVpZC1mb3JtYXQ6dW5zcGVjaWZpZWQiIE5hbWVRdWFsaWZpZXI9Imh0dHBzOi8vYXV0aC5zaWF0LnNhdC5nb2IubXgvbmlkcC9zYW1sMi9tZXRhZGF0YSIgU1BOYW1lUXVhbGlmaWVyPSJ3bHJmY2FtcGUuc2lhdC5zYXQuZ29iLm14Ij5QT0FNODk1MTwvc2FtbDpOYW1lSUQ+PHNhbWw6U3ViamVjdENvbmZpcm1hdGlvbiBNZXRob2Q9InVybjpvYXNpczpuYW1lczp0YzpTQU1MOjIuMDpjbTpiZWFyZXIiPjxzYW1sOlN1YmplY3RDb25maXJtYXRpb25EYXRhIEluUmVzcG9uc2VUbz0iXzB4MGE1NzQ5YjMzYjFjYzU3MzBkOTFmOTIyYThlY2Y0OWMiIE5vdE9uT3JBZnRlcj0iMjAyMi0xMS0yOFQxOTo0ODo1NVoiIFJlY2lwaWVudD0iaHR0cHM6Ly9yZmNhbXBlLnNpYXQuc2F0LmdvYi5teC9zYW1sMi9zcC9hY3MvcG9zdCIvPjwvc2FtbDpTdWJqZWN0Q29uZmlybWF0aW9uPjwvc2FtbDpTdWJqZWN0PjxzYW1sOkNvbmRpdGlvbnMgTm90QmVmb3JlPSIyMDIyLTExLTI4VDE5OjM4OjU1WiIgTm90T25PckFmdGVyPSIyMDIyLTExLTI4VDE5OjQ4OjU1WiI+PHNhbWw6QXVkaWVuY2VSZXN0cmljdGlvbj48c2FtbDpBdWRpZW5jZT53bHJmY2FtcGUuc2lhdC5zYXQuZ29iLm14PC9zYW1sOkF1ZGllbmNlPjwvc2FtbDpBdWRpZW5jZVJlc3RyaWN0aW9uPjwvc2FtbDpDb25kaXRpb25zPjxzYW1sOkF1dGhuU3RhdGVtZW50IEF1dGhuSW5zdGFudD0iMjAyMi0xMS0yOFQxOTo0Mzo1NFoiIFNlc3Npb25JbmRleD0iaWRySjVXdDNYaGpiU2RJbXFoNmx1bDNsbFUyVkkiPjxzYW1sOkF1dGhuQ29udGV4dD48c2FtbDpBdXRobkNvbnRleHRDbGFzc1JlZj51cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6YWM6Y2xhc3NlczpQYXNzd29yZDwvc2FtbDpBdXRobkNvbnRleHRDbGFzc1JlZj48c2FtbDpBdXRobkNvbnRleHREZWNsUmVmPnNhdC9maWVsL2VtcGxlYWRvcy91cmk8L3NhbWw6QXV0aG5Db250ZXh0RGVjbFJlZj48L3NhbWw6QXV0aG5Db250ZXh0Pjwvc2FtbDpBdXRoblN0YXRlbWVudD48c2FtbDpBdHRyaWJ1dGVTdGF0ZW1lbnQ+PHNhbWw6QXR0cmlidXRlIHhtbG5zOnhzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYSIgeG1sbnM6eHNpPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYS1pbnN0YW5jZSIgTmFtZT0iYWRtaW5HcmFsIiBOYW1lRm9ybWF0PSJ1cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6YXR0cm5hbWUtZm9ybWF0OmJhc2ljIj48c2FtbDpBdHRyaWJ1dGVWYWx1ZSB4c2k6dHlwZT0ieHM6c3RyaW5nIj5BR0FGRjwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48L3NhbWw6QXR0cmlidXRlPjxzYW1sOkF0dHJpYnV0ZSB4bWxuczp4cz0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEiIHhtbG5zOnhzaT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEtaW5zdGFuY2UiIE5hbWU9InJmY19sYXJnbyIgTmFtZUZvcm1hdD0idXJuOm9hc2lzOm5hbWVzOnRjOlNBTUw6Mi4wOmF0dHJuYW1lLWZvcm1hdDpiYXNpYyI+PHNhbWw6QXR0cmlidXRlVmFsdWUgeHNpOnR5cGU9InhzOnN0cmluZyI+UE9BTTg5MDUwMUtVODwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48L3NhbWw6QXR0cmlidXRlPjxzYW1sOkF0dHJpYnV0ZSB4bWxuczp4cz0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEiIHhtbG5zOnhzaT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEtaW5zdGFuY2UiIE5hbWU9InRpcG9QZXJzb25hIiBOYW1lRm9ybWF0PSJ1cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6YXR0cm5hbWUtZm9ybWF0OmJhc2ljIj48c2FtbDpBdHRyaWJ1dGVWYWx1ZSB4c2k6dHlwZT0ieHM6c3RyaW5nIj5FPC9zYW1sOkF0dHJpYnV0ZVZhbHVlPjwvc2FtbDpBdHRyaWJ1dGU+PHNhbWw6QXR0cmlidXRlIHhtbG5zOnhzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYSIgeG1sbnM6eHNpPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYS1pbnN0YW5jZSIgTmFtZT0idW5pZGFkTmVnb2NpbyIgTmFtZUZvcm1hdD0idXJuOm9hc2lzOm5hbWVzOnRjOlNBTUw6Mi4wOmF0dHJuYW1lLWZvcm1hdDpiYXNpYyI+PHNhbWw6QXR0cmlidXRlVmFsdWUgeHNpOnR5cGU9InhzOnN0cmluZyI+QUdBRkY8L3NhbWw6QXR0cmlidXRlVmFsdWU+PC9zYW1sOkF0dHJpYnV0ZT48c2FtbDpBdHRyaWJ1dGUgeG1sbnM6eHM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvWE1MU2NoZW1hIiB4bWxuczp4c2k9Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvWE1MU2NoZW1hLWluc3RhbmNlIiBOYW1lPSJlbnRGZWRlcmF0aXZhIiBOYW1lRm9ybWF0PSJ1cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6YXR0cm5hbWUtZm9ybWF0OmJhc2ljIj48c2FtbDpBdHRyaWJ1dGVWYWx1ZSB4c2k6dHlwZT0ieHM6c3RyaW5nIj4yODwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48L3NhbWw6QXR0cmlidXRlPjxzYW1sOkF0dHJpYnV0ZSB4bWxuczp4cz0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEiIHhtbG5zOnhzaT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEtaW5zdGFuY2UiIE5hbWU9InNlZ3VuZG9BcGVsbGlkbyIgTmFtZUZvcm1hdD0idXJuOm9hc2lzOm5hbWVzOnRjOlNBTUw6Mi4wOmF0dHJuYW1lLWZvcm1hdDpiYXNpYyI+PHNhbWw6QXR0cmlidXRlVmFsdWUgeHNpOnR5cGU9InhzOnN0cmluZyI+QXJyZWRvbmRvPC9zYW1sOkF0dHJpYnV0ZVZhbHVlPjwvc2FtbDpBdHRyaWJ1dGU+PHNhbWw6QXR0cmlidXRlIHhtbG5zOnhzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYSIgeG1sbnM6eHNpPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYS1pbnN0YW5jZSIgTmFtZT0ic2Vzc2lvbklEIiBOYW1lRm9ybWF0PSJ1cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6YXR0cm5hbWUtZm9ybWF0OmJhc2ljIj48c2FtbDpBdHRyaWJ1dGVWYWx1ZSB4c2k6dHlwZT0ieHM6c3RyaW5nIj5EODNFMDk4RUNBQjZGNkVDNTYyMTM4REY5NTBENjUwMjwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48L3NhbWw6QXR0cmlidXRlPjxzYW1sOkF0dHJpYnV0ZSB4bWxuczp4cz0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEiIHhtbG5zOnhzaT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEtaW5zdGFuY2UiIE5hbWU9Im5vbWJyZUNvbXBsZXRvIiBOYW1lRm9ybWF0PSJ1cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6YXR0cm5hbWUtZm9ybWF0OmJhc2ljIj48c2FtbDpBdHRyaWJ1dGVWYWx1ZSB4c2k6dHlwZT0ieHM6c3RyaW5nIj5NYW51ZWwgUG9uY2UgQXJyZWRvbmRvPC9zYW1sOkF0dHJpYnV0ZVZhbHVlPjwvc2FtbDpBdHRyaWJ1dGU+PHNhbWw6QXR0cmlidXRlIHhtbG5zOnhzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYSIgeG1sbnM6eHNpPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYS1pbnN0YW5jZSIgTmFtZT0id29ya2ZvcmNlSUQiIE5hbWVGb3JtYXQ9InVybjpvYXNpczpuYW1lczp0YzpTQU1MOjIuMDphdHRybmFtZS1mb3JtYXQ6YmFzaWMiPjxzYW1sOkF0dHJpYnV0ZVZhbHVlIHhzaTp0eXBlPSJ4czpzdHJpbmciPjAwMDAwMTg2NzIyPC9zYW1sOkF0dHJpYnV0ZVZhbHVlPjwvc2FtbDpBdHRyaWJ1dGU+PHNhbWw6QXR0cmlidXRlIHhtbG5zOnhzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYSIgeG1sbnM6eHNpPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYS1pbnN0YW5jZSIgTmFtZT0ibWFjIiBOYW1lRm9ybWF0PSJ1cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6YXR0cm5hbWUtZm9ybWF0OmJhc2ljIj48c2FtbDpBdHRyaWJ1dGVWYWx1ZSB4c2k6dHlwZT0ieHM6c3RyaW5nIj5bZXRoMCAoZXRoMCk9MDAtMDAtMDAtMDAtMDAtMDBdPC9zYW1sOkF0dHJpYnV0ZVZhbHVlPjwvc2FtbDpBdHRyaWJ1dGU+PHNhbWw6QXR0cmlidXRlIHhtbG5zOnhzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYSIgeG1sbnM6eHNpPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYS1pbnN0YW5jZSIgTmFtZT0ibm9tYnJlcyIgTmFtZUZvcm1hdD0idXJuOm9hc2lzOm5hbWVzOnRjOlNBTUw6Mi4wOmF0dHJuYW1lLWZvcm1hdDpiYXNpYyI+PHNhbWw6QXR0cmlidXRlVmFsdWUgeHNpOnR5cGU9InhzOnN0cmluZyI+TWFudWVsPC9zYW1sOkF0dHJpYnV0ZVZhbHVlPjwvc2FtbDpBdHRyaWJ1dGU+PHNhbWw6QXR0cmlidXRlIHhtbG5zOnhzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYSIgeG1sbnM6eHNpPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYS1pbnN0YW5jZSIgTmFtZT0iZGVzY0RlcGFydGFtZW50byIgTmFtZUZvcm1hdD0idXJuOm9hc2lzOm5hbWVzOnRjOlNBTUw6Mi4wOmF0dHJuYW1lLWZvcm1hdDpiYXNpYyI+PHNhbWw6QXR0cmlidXRlVmFsdWUgeHNpOnR5cGU9InhzOnN0cmluZyI+QXVkaXRvcsOtYSBGaXNjYWwgRmVkZXJhbDwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48L3NhbWw6QXR0cmlidXRlPjxzYW1sOkF0dHJpYnV0ZSB4bWxuczp4cz0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEiIHhtbG5zOnhzaT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEtaW5zdGFuY2UiIE5hbWU9InNlcmlhbEZJRUwiIE5hbWVGb3JtYXQ9InVybjpvYXNpczpuYW1lczp0YzpTQU1MOjIuMDphdHRybmFtZS1mb3JtYXQ6YmFzaWMiPjxzYW1sOkF0dHJpYnV0ZVZhbHVlIHhzaTp0eXBlPSJ4czpzdHJpbmciPjAwMDAxMDAwMDAwNTAzNDU1Mjc5PC9zYW1sOkF0dHJpYnV0ZVZhbHVlPjwvc2FtbDpBdHRyaWJ1dGU+PHNhbWw6QXR0cmlidXRlIHhtbG5zOnhzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYSIgeG1sbnM6eHNpPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYS1pbnN0YW5jZSIgTmFtZT0iZGVzY0FkbWluR3JhbCIgTmFtZUZvcm1hdD0idXJuOm9hc2lzOm5hbWVzOnRjOlNBTUw6Mi4wOmF0dHJuYW1lLWZvcm1hdDpiYXNpYyI+PHNhbWw6QXR0cmlidXRlVmFsdWUgeHNpOnR5cGU9InhzOnN0cmluZyI+QWRtaW5pc3RyYWNpw7NuIEdlbmVyYWwgZGUgQXVkaXRvcsOtYSBGaXNjYWwgRmVkZXJhbDwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48L3NhbWw6QXR0cmlidXRlPjxzYW1sOkF0dHJpYnV0ZSB4bWxuczp4cz0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEiIHhtbG5zOnhzaT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEtaW5zdGFuY2UiIE5hbWU9Ikdyb3VwcyIgTmFtZUZvcm1hdD0idXJuOm9hc2lzOm5hbWVzOnRjOlNBTUw6Mi4wOmF0dHJuYW1lLWZvcm1hdDpiYXNpYyI+PHNhbWw6QXR0cmlidXRlVmFsdWUgeHNpOnR5cGU9InhzOnN0cmluZyI+Y249U0FUX1NEQ19DT05TX1NBTCxjbj1TREMsY249TGV2ZWwxMCxjbj1Sb2xlRGVmcyxjbj1Sb2xlQ29uZmlnLGNuPUFwcENvbmZpZyxjbj1NSURTLVVzZXJBcHBsaWNhdGlvbixjbj1Ecml2ZXJTZXQyLG91PUlETSxvdT1TRVJWSUNJT1Msbz1TQVQ8L3NhbWw6QXR0cmlidXRlVmFsdWU+PHNhbWw6QXR0cmlidXRlVmFsdWUgeHNpOnR5cGU9InhzOnN0cmluZyI+Y249U0FUX1NEQ19BRE1JTl9TQUwsY249U0RDLGNuPUxldmVsMTAsY249Um9sZURlZnMsY249Um9sZUNvbmZpZyxjbj1BcHBDb25maWcsY249TUlEUy1Vc2VyQXBwbGljYXRpb24sY249RHJpdmVyU2V0MixvdT1JRE0sb3U9U0VSVklDSU9TLG89U0FUPC9zYW1sOkF0dHJpYnV0ZVZhbHVlPjxzYW1sOkF0dHJpYnV0ZVZhbHVlIHhzaTp0eXBlPSJ4czpzdHJpbmciPmNuPVNJQVRfTllWX1JFRyxjbj1OWVYsY249TGV2ZWwxMCxjbj1Sb2xlRGVmcyxjbj1Sb2xlQ29uZmlnLGNuPUFwcENvbmZpZyxjbj1NSURTLVVzZXJBcHBsaWNhdGlvbixjbj1Ecml2ZXJTZXQyLG91PUlETSxvdT1TRVJWSUNJT1Msbz1TQVQ8L3NhbWw6QXR0cmlidXRlVmFsdWU+PHNhbWw6QXR0cmlidXRlVmFsdWUgeHNpOnR5cGU9InhzOnN0cmluZyI+Y249U0lBVF9OWVZfR0VOLGNuPU5ZVixjbj1MZXZlbDEwLGNuPVJvbGVEZWZzLGNuPVJvbGVDb25maWcsY249QXBwQ29uZmlnLGNuPU1JRFMtVXNlckFwcGxpY2F0aW9uLGNuPURyaXZlclNldDIsb3U9SURNLG91PVNFUlZJQ0lPUyxvPVNBVDwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48c2FtbDpBdHRyaWJ1dGVWYWx1ZSB4c2k6dHlwZT0ieHM6c3RyaW5nIj5jbj1TSUFUX05ZVl9DT05TLGNuPU5ZVixjbj1MZXZlbDEwLGNuPVJvbGVEZWZzLGNuPVJvbGVDb25maWcsY249QXBwQ29uZmlnLGNuPU1JRFMtVXNlckFwcGxpY2F0aW9uLGNuPURyaXZlclNldDIsb3U9SURNLG91PVNFUlZJQ0lPUyxvPVNBVDwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48c2FtbDpBdHRyaWJ1dGVWYWx1ZSB4c2k6dHlwZT0ieHM6c3RyaW5nIj5jbj1TSUFUX05ZVl9FWFAsY249TllWLGNuPUxldmVsMTAsY249Um9sZURlZnMsY249Um9sZUNvbmZpZyxjbj1BcHBDb25maWcsY249TUlEUy1Vc2VyQXBwbGljYXRpb24sY249RHJpdmVyU2V0MixvdT1JRE0sb3U9U0VSVklDSU9TLG89U0FUPC9zYW1sOkF0dHJpYnV0ZVZhbHVlPjxzYW1sOkF0dHJpYnV0ZVZhbHVlIHhzaTp0eXBlPSJ4czpzdHJpbmciPmNuPVNJQVRfTllWX0ZJUixjbj1OWVYsY249TGV2ZWwxMCxjbj1Sb2xlRGVmcyxjbj1Sb2xlQ29uZmlnLGNuPUFwcENvbmZpZyxjbj1NSURTLVVzZXJBcHBsaWNhdGlvbixjbj1Ecml2ZXJTZXQyLG91PUlETSxvdT1TRVJWSUNJT1Msbz1TQVQ8L3NhbWw6QXR0cmlidXRlVmFsdWU+PHNhbWw6QXR0cmlidXRlVmFsdWUgeHNpOnR5cGU9InhzOnN0cmluZyI+Y249U0lBVF9OWVZfRU5WSU9fQV9ORSxjbj1OWVYsY249TGV2ZWwxMCxjbj1Sb2xlRGVmcyxjbj1Sb2xlQ29uZmlnLGNuPUFwcENvbmZpZyxjbj1NSURTLVVzZXJBcHBsaWNhdGlvbixjbj1Ecml2ZXJTZXQyLG91PUlETSxvdT1TRVJWSUNJT1Msbz1TQVQ8L3NhbWw6QXR0cmlidXRlVmFsdWU+PHNhbWw6QXR0cmlidXRlVmFsdWUgeHNpOnR5cGU9InhzOnN0cmluZyI+Y249U0FUX0VORVRfRU1QX1JFRF9JTlQsY249RVhUUkFORVQsY249TGV2ZWwxMCxjbj1Sb2xlRGVmcyxjbj1Sb2xlQ29uZmlnLGNuPUFwcENvbmZpZyxjbj1NSURTLVVzZXJBcHBsaWNhdGlvbixjbj1Ecml2ZXJTZXQyLG91PUlETSxvdT1TRVJWSUNJT1Msbz1TQVQ8L3NhbWw6QXR0cmlidXRlVmFsdWU+PHNhbWw6QXR0cmlidXRlVmFsdWUgeHNpOnR5cGU9InhzOnN0cmluZyI+Y249U0lBVF9TQ19BR0VOVEVfT1BFUkFDSU9OLGNuPVNBQyxjbj1MZXZlbDEwLGNuPVJvbGVEZWZzLGNuPVJvbGVDb25maWcsY249QXBwQ29uZmlnLGNuPU1JRFMtVXNlckFwcGxpY2F0aW9uLGNuPURyaXZlclNldDIsb3U9SURNLG91PVNFUlZJQ0lPUyxvPVNBVDwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48c2FtbDpBdHRyaWJ1dGVWYWx1ZSB4c2k6dHlwZT0ieHM6c3RyaW5nIj5jbj1TSUFUX1NDX0NPTlNfVklTVEFfMzYwX1JPLGNuPVNBQyxjbj1MZXZlbDEwLGNuPVJvbGVEZWZzLGNuPVJvbGVDb25maWcsY249QXBwQ29uZmlnLGNuPU1JRFMtVXNlckFwcGxpY2F0aW9uLGNuPURyaXZlclNldDIsb3U9SURNLG91PVNFUlZJQ0lPUyxvPVNBVDwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48c2FtbDpBdHRyaWJ1dGVWYWx1ZSB4c2k6dHlwZT0ieHM6c3RyaW5nIj5jbj1TSUFUX1NDX0FHRU5URV9DSUVSUkFfQ0FTTyxjbj1TQUMsY249TGV2ZWwxMCxjbj1Sb2xlRGVmcyxjbj1Sb2xlQ29uZmlnLGNuPUFwcENvbmZpZyxjbj1NSURTLVVzZXJBcHBsaWNhdGlvbixjbj1Ecml2ZXJTZXQyLG91PUlETSxvdT1TRVJWSUNJT1Msbz1TQVQ8L3NhbWw6QXR0cmlidXRlVmFsdWU+PHNhbWw6QXR0cmlidXRlVmFsdWUgeHNpOnR5cGU9InhzOnN0cmluZyI+Y249U0FUX0NPTlRBRV9DT05TVUwsY249Q09OVEFFLGNuPUxldmVsMTAsY249Um9sZURlZnMsY249Um9sZUNvbmZpZyxjbj1BcHBDb25maWcsY249TUlEUy1Vc2VyQXBwbGljYXRpb24sY249RHJpdmVyU2V0MixvdT1JRE0sb3U9U0VSVklDSU9TLG89U0FUPC9zYW1sOkF0dHJpYnV0ZVZhbHVlPjxzYW1sOkF0dHJpYnV0ZVZhbHVlIHhzaTp0eXBlPSJ4czpzdHJpbmciPmNuPVNBVF9DT05UQUVfQ09OU1VMVEFJTkZPX0pFRkUsY249Q09OVEFFLGNuPUxldmVsMTAsY249Um9sZURlZnMsY249Um9sZUNvbmZpZyxjbj1BcHBDb25maWcsY249TUlEUy1Vc2VyQXBwbGljYXRpb24sY249RHJpdmVyU2V0MixvdT1JRE0sb3U9U0VSVklDSU9TLG89U0FUPC9zYW1sOkF0dHJpYnV0ZVZhbHVlPjxzYW1sOkF0dHJpYnV0ZVZhbHVlIHhzaTp0eXBlPSJ4czpzdHJpbmciPmNuPVNBVF9TSVBSRURfTVZEMDAxLGNuPVNJUFJFRCxjbj1MZXZlbDEwLGNuPVJvbGVEZWZzLGNuPVJvbGVDb25maWcsY249QXBwQ29uZmlnLGNuPU1JRFMtVXNlckFwcGxpY2F0aW9uLGNuPURyaXZlclNldDIsb3U9SURNLG91PVNFUlZJQ0lPUyxvPVNBVDwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48c2FtbDpBdHRyaWJ1dGVWYWx1ZSB4c2k6dHlwZT0ieHM6c3RyaW5nIj5hdXRoZW50aWNhdGVkPC9zYW1sOkF0dHJpYnV0ZVZhbHVlPjwvc2FtbDpBdHRyaWJ1dGU+PHNhbWw6QXR0cmlidXRlIHhtbG5zOnhzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYSIgeG1sbnM6eHNpPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYS1pbnN0YW5jZSIgTmFtZT0iZGVzY0FkbWluQ2VudHJhbCIgTmFtZUZvcm1hdD0idXJuOm9hc2lzOm5hbWVzOnRjOlNBTUw6Mi4wOmF0dHJuYW1lLWZvcm1hdDpiYXNpYyI+PHNhbWw6QXR0cmlidXRlVmFsdWUgeHNpOnR5cGU9InhzOnN0cmluZyI+QWRtaW5pc3RyYWNpw7NuIEdlbmVyYWwgZGUgQXVkaXRvcsOtYSBGaXNjYWwgRmVkZXJhbDwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48L3NhbWw6QXR0cmlidXRlPjxzYW1sOkF0dHJpYnV0ZSB4bWxuczp4cz0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEiIHhtbG5zOnhzaT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEtaW5zdGFuY2UiIE5hbWU9InBlcnNvbklEIiBOYW1lRm9ybWF0PSJ1cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6YXR0cm5hbWUtZm9ybWF0OmJhc2ljIj48c2FtbDpBdHRyaWJ1dGVWYWx1ZSB4c2k6dHlwZT0ieHM6c3RyaW5nIj5QOTAyODMzNDY8L3NhbWw6QXR0cmlidXRlVmFsdWU+PC9zYW1sOkF0dHJpYnV0ZT48c2FtbDpBdHRyaWJ1dGUgeG1sbnM6eHM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvWE1MU2NoZW1hIiB4bWxuczp4c2k9Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvWE1MU2NoZW1hLWluc3RhbmNlIiBOYW1lPSJsb2NhbGlkYWQiIE5hbWVGb3JtYXQ9InVybjpvYXNpczpuYW1lczp0YzpTQU1MOjIuMDphdHRybmFtZS1mb3JtYXQ6YmFzaWMiPjxzYW1sOkF0dHJpYnV0ZVZhbHVlIHhzaTp0eXBlPSJ4czpzdHJpbmciPjI4MDMyMEIxUEI8L3NhbWw6QXR0cmlidXRlVmFsdWU+PC9zYW1sOkF0dHJpYnV0ZT48c2FtbDpBdHRyaWJ1dGUgeG1sbnM6eHM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvWE1MU2NoZW1hIiB4bWxuczp4c2k9Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvWE1MU2NoZW1hLWluc3RhbmNlIiBOYW1lPSJlbWFpbCIgTmFtZUZvcm1hdD0idXJuOm9hc2lzOm5hbWVzOnRjOlNBTUw6Mi4wOmF0dHJuYW1lLWZvcm1hdDpiYXNpYyI+PHNhbWw6QXR0cmlidXRlVmFsdWUgeHNpOnR5cGU9InhzOnN0cmluZyI+bWFudWVsLnBvbmNlYUBzYXQuZ29iLm14PC9zYW1sOkF0dHJpYnV0ZVZhbHVlPjwvc2FtbDpBdHRyaWJ1dGU+PHNhbWw6QXR0cmlidXRlIHhtbG5zOnhzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYSIgeG1sbnM6eHNpPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYS1pbnN0YW5jZSIgTmFtZT0iY3VycCIgTmFtZUZvcm1hdD0idXJuOm9hc2lzOm5hbWVzOnRjOlNBTUw6Mi4wOmF0dHJuYW1lLWZvcm1hdDp1cmkiPjxzYW1sOkF0dHJpYnV0ZVZhbHVlIHhzaTp0eXBlPSJ4czpzdHJpbmciPlBPQU04OTA1MDFIVFNOUk4wODwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48L3NhbWw6QXR0cmlidXRlPjxzYW1sOkF0dHJpYnV0ZSB4bWxuczp4cz0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEiIHhtbG5zOnhzaT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEtaW5zdGFuY2UiIE5hbWU9InByaW1lckFwZWxsaWRvIiBOYW1lRm9ybWF0PSJ1cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6YXR0cm5hbWUtZm9ybWF0OmJhc2ljIj48c2FtbDpBdHRyaWJ1dGVWYWx1ZSB4c2k6dHlwZT0ieHM6c3RyaW5nIj5Qb25jZTwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48L3NhbWw6QXR0cmlidXRlPjxzYW1sOkF0dHJpYnV0ZSB4bWxuczp4cz0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEiIHhtbG5zOnhzaT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEtaW5zdGFuY2UiIE5hbWU9ImlwIiBOYW1lRm9ybWF0PSJ1cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6YXR0cm5hbWUtZm9ybWF0OmJhc2ljIj48c2FtbDpBdHRyaWJ1dGVWYWx1ZSB4c2k6dHlwZT0ieHM6c3RyaW5nIj5bZXRoMCAoZXRoMCk9LzE4OS4yNTAuNTYuMjE3XTwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48L3NhbWw6QXR0cmlidXRlPjxzYW1sOkF0dHJpYnV0ZSB4bWxuczp4cz0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEiIHhtbG5zOnhzaT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEtaW5zdGFuY2UiIE5hbWU9ImNuIiBOYW1lRm9ybWF0PSJ1cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6YXR0cm5hbWUtZm9ybWF0OmJhc2ljIj48c2FtbDpBdHRyaWJ1dGVWYWx1ZSB4c2k6dHlwZT0ieHM6c3RyaW5nIj5QT0FNODk1MTwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48L3NhbWw6QXR0cmlidXRlPjxzYW1sOkF0dHJpYnV0ZSB4bWxuczp4cz0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEiIHhtbG5zOnhzaT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEtaW5zdGFuY2UiIE5hbWU9InJmYyIgTmFtZUZvcm1hdD0idXJuOm9hc2lzOm5hbWVzOnRjOlNBTUw6Mi4wOmF0dHJuYW1lLWZvcm1hdDpiYXNpYyI+PHNhbWw6QXR0cmlidXRlVmFsdWUgeHNpOnR5cGU9InhzOnN0cmluZyI+UE9BTTg5NTE8L3NhbWw6QXR0cmlidXRlVmFsdWU+PC9zYW1sOkF0dHJpYnV0ZT48c2FtbDpBdHRyaWJ1dGUgeG1sbnM6eHM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvWE1MU2NoZW1hIiB4bWxuczp4c2k9Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvWE1MU2NoZW1hLWluc3RhbmNlIiBOYW1lPSJkZXNjRW50RmVkZXJhdGl2YSIgTmFtZUZvcm1hdD0idXJuOm9hc2lzOm5hbWVzOnRjOlNBTUw6Mi4wOmF0dHJuYW1lLWZvcm1hdDpiYXNpYyI+PHNhbWw6QXR0cmlidXRlVmFsdWUgeHNpOnR5cGU9InhzOnN0cmluZyI+VGFtYXVsaXBhczwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48L3NhbWw6QXR0cmlidXRlPjxzYW1sOkF0dHJpYnV0ZSB4bWxuczp4cz0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEiIHhtbG5zOnhzaT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEtaW5zdGFuY2UiIE5hbWU9Im5yZk1lbWJlck9mIiBOYW1lRm9ybWF0PSJ1cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6YXR0cm5hbWUtZm9ybWF0OmJhc2ljIj48c2FtbDpBdHRyaWJ1dGVWYWx1ZSB4c2k6dHlwZT0ieHM6c3RyaW5nIj5jbj1TQVRfU0RDX0NPTlNfU0FMLGNuPVNEQyxjbj1MZXZlbDEwLGNuPVJvbGVEZWZzLGNuPVJvbGVDb25maWcsY249QXBwQ29uZmlnLGNuPU1JRFMtVXNlckFwcGxpY2F0aW9uLGNuPURyaXZlclNldDIsb3U9SURNLG91PVNFUlZJQ0lPUyxvPVNBVDwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48c2FtbDpBdHRyaWJ1dGVWYWx1ZSB4c2k6dHlwZT0ieHM6c3RyaW5nIj5jbj1TQVRfU0RDX0FETUlOX1NBTCxjbj1TREMsY249TGV2ZWwxMCxjbj1Sb2xlRGVmcyxjbj1Sb2xlQ29uZmlnLGNuPUFwcENvbmZpZyxjbj1NSURTLVVzZXJBcHBsaWNhdGlvbixjbj1Ecml2ZXJTZXQyLG91PUlETSxvdT1TRVJWSUNJT1Msbz1TQVQ8L3NhbWw6QXR0cmlidXRlVmFsdWU+PHNhbWw6QXR0cmlidXRlVmFsdWUgeHNpOnR5cGU9InhzOnN0cmluZyI+Y249U0lBVF9OWVZfUkVHLGNuPU5ZVixjbj1MZXZlbDEwLGNuPVJvbGVEZWZzLGNuPVJvbGVDb25maWcsY249QXBwQ29uZmlnLGNuPU1JRFMtVXNlckFwcGxpY2F0aW9uLGNuPURyaXZlclNldDIsb3U9SURNLG91PVNFUlZJQ0lPUyxvPVNBVDwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48c2FtbDpBdHRyaWJ1dGVWYWx1ZSB4c2k6dHlwZT0ieHM6c3RyaW5nIj5jbj1TSUFUX05ZVl9HRU4sY249TllWLGNuPUxldmVsMTAsY249Um9sZURlZnMsY249Um9sZUNvbmZpZyxjbj1BcHBDb25maWcsY249TUlEUy1Vc2VyQXBwbGljYXRpb24sY249RHJpdmVyU2V0MixvdT1JRE0sb3U9U0VSVklDSU9TLG89U0FUPC9zYW1sOkF0dHJpYnV0ZVZhbHVlPjxzYW1sOkF0dHJpYnV0ZVZhbHVlIHhzaTp0eXBlPSJ4czpzdHJpbmciPmNuPVNJQVRfTllWX0NPTlMsY249TllWLGNuPUxldmVsMTAsY249Um9sZURlZnMsY249Um9sZUNvbmZpZyxjbj1BcHBDb25maWcsY249TUlEUy1Vc2VyQXBwbGljYXRpb24sY249RHJpdmVyU2V0MixvdT1JRE0sb3U9U0VSVklDSU9TLG89U0FUPC9zYW1sOkF0dHJpYnV0ZVZhbHVlPjxzYW1sOkF0dHJpYnV0ZVZhbHVlIHhzaTp0eXBlPSJ4czpzdHJpbmciPmNuPVNJQVRfTllWX0VYUCxjbj1OWVYsY249TGV2ZWwxMCxjbj1Sb2xlRGVmcyxjbj1Sb2xlQ29uZmlnLGNuPUFwcENvbmZpZyxjbj1NSURTLVVzZXJBcHBsaWNhdGlvbixjbj1Ecml2ZXJTZXQyLG91PUlETSxvdT1TRVJWSUNJT1Msbz1TQVQ8L3NhbWw6QXR0cmlidXRlVmFsdWU+PHNhbWw6QXR0cmlidXRlVmFsdWUgeHNpOnR5cGU9InhzOnN0cmluZyI+Y249U0lBVF9OWVZfRklSLGNuPU5ZVixjbj1MZXZlbDEwLGNuPVJvbGVEZWZzLGNuPVJvbGVDb25maWcsY249QXBwQ29uZmlnLGNuPU1JRFMtVXNlckFwcGxpY2F0aW9uLGNuPURyaXZlclNldDIsb3U9SURNLG91PVNFUlZJQ0lPUyxvPVNBVDwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48c2FtbDpBdHRyaWJ1dGVWYWx1ZSB4c2k6dHlwZT0ieHM6c3RyaW5nIj5jbj1TSUFUX05ZVl9FTlZJT19BX05FLGNuPU5ZVixjbj1MZXZlbDEwLGNuPVJvbGVEZWZzLGNuPVJvbGVDb25maWcsY249QXBwQ29uZmlnLGNuPU1JRFMtVXNlckFwcGxpY2F0aW9uLGNuPURyaXZlclNldDIsb3U9SURNLG91PVNFUlZJQ0lPUyxvPVNBVDwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48c2FtbDpBdHRyaWJ1dGVWYWx1ZSB4c2k6dHlwZT0ieHM6c3RyaW5nIj5jbj1TQVRfRU5FVF9FTVBfUkVEX0lOVCxjbj1FWFRSQU5FVCxjbj1MZXZlbDEwLGNuPVJvbGVEZWZzLGNuPVJvbGVDb25maWcsY249QXBwQ29uZmlnLGNuPU1JRFMtVXNlckFwcGxpY2F0aW9uLGNuPURyaXZlclNldDIsb3U9SURNLG91PVNFUlZJQ0lPUyxvPVNBVDwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48c2FtbDpBdHRyaWJ1dGVWYWx1ZSB4c2k6dHlwZT0ieHM6c3RyaW5nIj5jbj1TSUFUX1NDX0FHRU5URV9PUEVSQUNJT04sY249U0FDLGNuPUxldmVsMTAsY249Um9sZURlZnMsY249Um9sZUNvbmZpZyxjbj1BcHBDb25maWcsY249TUlEUy1Vc2VyQXBwbGljYXRpb24sY249RHJpdmVyU2V0MixvdT1JRE0sb3U9U0VSVklDSU9TLG89U0FUPC9zYW1sOkF0dHJpYnV0ZVZhbHVlPjxzYW1sOkF0dHJpYnV0ZVZhbHVlIHhzaTp0eXBlPSJ4czpzdHJpbmciPmNuPVNJQVRfU0NfQ09OU19WSVNUQV8zNjBfUk8sY249U0FDLGNuPUxldmVsMTAsY249Um9sZURlZnMsY249Um9sZUNvbmZpZyxjbj1BcHBDb25maWcsY249TUlEUy1Vc2VyQXBwbGljYXRpb24sY249RHJpdmVyU2V0MixvdT1JRE0sb3U9U0VSVklDSU9TLG89U0FUPC9zYW1sOkF0dHJpYnV0ZVZhbHVlPjxzYW1sOkF0dHJpYnV0ZVZhbHVlIHhzaTp0eXBlPSJ4czpzdHJpbmciPmNuPVNJQVRfU0NfQUdFTlRFX0NJRVJSQV9DQVNPLGNuPVNBQyxjbj1MZXZlbDEwLGNuPVJvbGVEZWZzLGNuPVJvbGVDb25maWcsY249QXBwQ29uZmlnLGNuPU1JRFMtVXNlckFwcGxpY2F0aW9uLGNuPURyaXZlclNldDIsb3U9SURNLG91PVNFUlZJQ0lPUyxvPVNBVDwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48c2FtbDpBdHRyaWJ1dGVWYWx1ZSB4c2k6dHlwZT0ieHM6c3RyaW5nIj5jbj1TQVRfQ09OVEFFX0NPTlNVTCxjbj1DT05UQUUsY249TGV2ZWwxMCxjbj1Sb2xlRGVmcyxjbj1Sb2xlQ29uZmlnLGNuPUFwcENvbmZpZyxjbj1NSURTLVVzZXJBcHBsaWNhdGlvbixjbj1Ecml2ZXJTZXQyLG91PUlETSxvdT1TRVJWSUNJT1Msbz1TQVQ8L3NhbWw6QXR0cmlidXRlVmFsdWU+PHNhbWw6QXR0cmlidXRlVmFsdWUgeHNpOnR5cGU9InhzOnN0cmluZyI+Y249U0FUX0NPTlRBRV9DT05TVUxUQUlORk9fSkVGRSxjbj1DT05UQUUsY249TGV2ZWwxMCxjbj1Sb2xlRGVmcyxjbj1Sb2xlQ29uZmlnLGNuPUFwcENvbmZpZyxjbj1NSURTLVVzZXJBcHBsaWNhdGlvbixjbj1Ecml2ZXJTZXQyLG91PUlETSxvdT1TRVJWSUNJT1Msbz1TQVQ8L3NhbWw6QXR0cmlidXRlVmFsdWU+PHNhbWw6QXR0cmlidXRlVmFsdWUgeHNpOnR5cGU9InhzOnN0cmluZyI+Y249U0FUX1NJUFJFRF9NVkQwMDEsY249U0lQUkVELGNuPUxldmVsMTAsY249Um9sZURlZnMsY249Um9sZUNvbmZpZyxjbj1BcHBDb25maWcsY249TUlEUy1Vc2VyQXBwbGljYXRpb24sY249RHJpdmVyU2V0MixvdT1JRE0sb3U9U0VSVklDSU9TLG89U0FUPC9zYW1sOkF0dHJpYnV0ZVZhbHVlPjwvc2FtbDpBdHRyaWJ1dGU+PHNhbWw6QXR0cmlidXRlIHhtbG5zOnhzPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYSIgeG1sbnM6eHNpPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYS1pbnN0YW5jZSIgTmFtZT0ibG9jYWxpZGFkQ1JNIiBOYW1lRm9ybWF0PSJ1cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6YXR0cm5hbWUtZm9ybWF0OmJhc2ljIj48c2FtbDpBdHRyaWJ1dGVWYWx1ZSB4c2k6dHlwZT0ieHM6c3RyaW5nIj5GLTA0NDwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48L3NhbWw6QXR0cmlidXRlPjxzYW1sOkF0dHJpYnV0ZSB4bWxuczp4cz0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEiIHhtbG5zOnhzaT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEtaW5zdGFuY2UiIE5hbWU9ImFkbWluQ2VudHJhbCIgTmFtZUZvcm1hdD0idXJuOm9hc2lzOm5hbWVzOnRjOlNBTUw6Mi4wOmF0dHJuYW1lLWZvcm1hdDpiYXNpYyI+PHNhbWw6QXR0cmlidXRlVmFsdWUgeHNpOnR5cGU9InhzOnN0cmluZyI+NTAwMDAwMDAwMDwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48L3NhbWw6QXR0cmlidXRlPjxzYW1sOkF0dHJpYnV0ZSB4bWxuczp4cz0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEiIHhtbG5zOnhzaT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEtaW5zdGFuY2UiIE5hbWU9ImRlcGFydGFtZW50b0lEIiBOYW1lRm9ybWF0PSJ1cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6YXR0cm5hbWUtZm9ybWF0OmJhc2ljIj48c2FtbDpBdHRyaWJ1dGVWYWx1ZSB4c2k6dHlwZT0ieHM6c3RyaW5nIj41ODcwMDEwMzAwPC9zYW1sOkF0dHJpYnV0ZVZhbHVlPjwvc2FtbDpBdHRyaWJ1dGU+PC9zYW1sOkF0dHJpYnV0ZVN0YXRlbWVudD48L3NhbWw6QXNzZXJ0aW9uPjwvc2FtbHA6UmVzcG9uc2U+", _header5.SetCookie);
                var _header6 = _tool.ManipulateHeader(_acs.Headers);

                // 7
                var _acceso = await _web.Acceso(_header6.SetCookie);
                var _header7 = _tool.ManipulateHeader(_acceso.Headers);

                var _visor = await _web.Visor($"{_header4.RFC} {_header3.IPC} {_header7.JSession} {_header7.F5}");
                var _vheaders = _visor.Headers;
                var _vcontent = await _visor.Content.ReadAsStringAsync();

            }
        }




        public Search byDataFisica(Work _req)
        {
            var _returns = new Search();
            try
            {

                var _visor = _web.Visor(_req.COOKIES).Result;
                var _newHeaders = _tool.ManipulateHeader(_visor.Headers);
                var _visorContent = _tool.StringHTML(_visor.Content.ReadAsStringAsync().Result);
                var _viewStateVisor = _visorContent.GetElementbyId("javax.faces.ViewState").GetAttributes("value").FirstOrDefault()?.Value;

                var _newCookies = _tool.ReplaceCookie(_req.COOKIES, "F5-CONTENCION-rfcampe-443", _tool.FindCookieValue(_newHeaders.SetCookie, "F5-CONTENCION-rfcampe-443"));
                _newCookies = _tool.ReplaceCookie(_newCookies, "RFC_AMP_CUE_CONT_9080_e", _tool.FindCookieValue(_newHeaders.SetCookie, "RFC_AMP_CUE_CONT_9080_e"));

                _req.COOKIES = _newCookies;
                _req.VIEW_STATE = _viewStateVisor!;
                var _result = _web.BuscarFisica(_req);
                var _headers = _result.Headers;

                var _cookiesSearch = _tool.ManipulateHeader(_headers);

                _newCookies = _tool.ReplaceCookie(_newCookies, "F5-CONTENCION-rfcampe-443", _tool.FindCookieValue(_cookiesSearch.SetCookie, "F5-CONTENCION-rfcampe-443"));
                _newCookies = _tool.ReplaceCookie(_newCookies, "RFC_AMP_CUE_CONT_9080_e", _tool.FindCookieValue(_cookiesSearch.SetCookie, "RFC_AMP_CUE_CONT_9080_e"));

                var _content = _result.Content.ReadAsStringAsync().Result;
                var _html = _tool.StringHTML(_content);

                var _msg = _html.GetElementbyId("messageList")?.InnerHtml;
                try
                {
                    var _tableResult = _html.GetElementbyId("visorForm:tablaResultados")?.InnerHtml;
                    var _viewState = _html.GetElementbyId("javax.faces.ViewState")?.InnerHtml;
                    var _viewData = _tool.ManipulateCDATA(_viewState!);
                    var _resultSearch = _tool.StringHTML(_tool.ManipulateCDATA(_tableResult!));
                    var _rTable = _tool.StringHTML(_resultSearch.GetElementbyId("visorForm:tablaResultados_data")?.InnerHtml!);
                    var _trTable = _rTable.DocumentNode.SelectNodes("//tr");
                    var _labels = _trTable[0].SelectNodes("//label");
                    //Get Main Data
                    var name = "";
                    var pApellido = "";
                    var sApellido = "";
                    var rfc = "";
                    var curp = "";
                    var ciudad = "";
                    var source = "";
                    for (var i = 0; i < _labels.Count - 2; i++)
                    {
                        var _current = _labels[i];
                        var _id = _current.Id;
                        var _label = _current.InnerHtml;
                        switch (i)
                        {
                            case 0:
                                name = _label;
                                break;
                            case 1:
                                pApellido = _label;
                                break;
                            case 2:
                                sApellido = _label;
                                break;
                            case 3:
                                source = _id;
                                rfc = _label;
                                break;
                            case 4:
                                curp = _label;
                                break;
                            case 5:
                                ciudad = _label;
                                break;
                            default:
                                break;
                        }
                    }

                    var _onClick = _web.ClickRFC(_newCookies, _viewData, curp).Result;
                    var _onClickStr = _onClick.Content.ReadAsStringAsync().Result;
                    var _onClickCookies = _tool.ManipulateHeader(_onClick.Headers);
                    _newCookies = _tool.ReplaceCookie(_newCookies, "F5-CONTENCION-rfcampe-443", _tool.FindCookieValue(_onClickCookies.SetCookie, "F5-CONTENCION-rfcampe-443"));
                    _newCookies = _tool.ReplaceCookie(_newCookies, "RFC_AMP_CUE_CONT_9080_e", _tool.FindCookieValue(_onClickCookies.SetCookie, "RFC_AMP_CUE_CONT_9080_e"));
                    var _onInit = _web.InitVisualizador(_newCookies).Result;
                    var _onInitCookies = _tool.ManipulateHeader(_onInit.Headers);
                    var _onInitStr = _onInit.Content.ReadAsStringAsync().Result;
                    _newCookies = _tool.ReplaceCookie(_newCookies, "F5-CONTENCION-rfcampe-443", _tool.FindCookieValue(_onInitCookies.SetCookie, "F5-CONTENCION-rfcampe-443"));
                    _newCookies = _tool.ReplaceCookie(_newCookies, "RFC_AMP_CUE_CONT_9080_e", _tool.FindCookieValue(_onInitCookies.SetCookie, "RFC_AMP_CUE_CONT_9080_e"));
                    var _onResume = _web.HTMLResumen(_newCookies).Result;
                    var _onResumeStr = _onResume.Content.ReadAsStringAsync().Result;
                    var _onResumeCookies = _tool.ManipulateHeader(_onResume.Headers);
                    var _onResumeHTML = _tool.StringHTML(_onResumeStr);
                    var _onResumeView = _onResumeHTML.GetElementbyId("javax.faces.ViewState").InnerHtml;
                    var _sourceResume = _tool.ManipulateCDATA(_onResumeView);
                    _newCookies = _tool.ReplaceCookie(_newCookies, "F5-CONTENCION-rfcampe-443", _tool.FindCookieValue(_onResumeCookies.SetCookie, "F5-CONTENCION-rfcampe-443"));
                    _newCookies = _tool.ReplaceCookie(_newCookies, "RFC_AMP_CUE_CONT_9080_e", _tool.FindCookieValue(_onResumeCookies.SetCookie, "RFC_AMP_CUE_CONT_9080_e"));
                    var _onResumenTrib = _web.ResumenTributario(_newCookies, _sourceResume!).Result;
                    var _onResumenTribStr = _onResumenTrib.Content.ReadAsStringAsync().Result;
                    var _onResumenTribCookies = _tool.ManipulateHeader(_onResumenTrib.Headers);
                    _newCookies = _tool.ReplaceCookie(_newCookies, "F5-CONTENCION-rfcampe-443", _tool.FindCookieValue(_onResumenTribCookies.SetCookie, "F5-CONTENCION-rfcampe-443"));
                    _newCookies = _tool.ReplaceCookie(_newCookies, "RFC_AMP_CUE_CONT_9080_e", _tool.FindCookieValue(_onResumenTribCookies.SetCookie, "RFC_AMP_CUE_CONT_9080_e"));
                    var _onDownload = _web.DownloadRFC(_newCookies, $"{_req.Id}-{rfc}.pdf").Result;
                    var _estado = _tool.STATE(ciudad);
                    if (_onDownload.StatusCode == HttpStatusCode.OK) {
                        var _str = _onDownload.Content.ReadAsStringAsync().Result;
                        if (_str.Contains("PDF"))
                        {
                            var _path = @$"cache/rfcs/{_req.Id}-{rfc}.pdf";

                            var fs = new FileStream(_path, FileMode.CreateNew);
                            _onDownload.Content.CopyToAsync(fs).Wait();
                            fs.Close();
                            var _corte = _web.ADD_CORTE(rfc, "Registro Federal de Contribuyentes", _req.UserId.ToString(), $"{_req.Id}-{rfc}.pdf", $"{name} {pApellido} {sApellido}", _estado).Result;
                            var _addCorte = JsonConvert.DeserializeObject<Modelos.Corte.AddCorte>(_corte);
                            _returns.CorteId = _addCorte.id;
                            _returns.Download = true;
                            _returns.Estado = _estado;

                            var _cookiesDwn = _tool.ManipulateHeader(_onDownload.Headers);
                            var _newAccess = new Access
                            {
                                Cookie = _newCookies,
                                ViewState = _viewStateVisor!
                            };


                            _returns.NewToken = _newAccess;
                            _returns.Found = true;
                            _returns.ValidToken = true;
                            _returns.Names = name;
                            _returns.RFC = rfc;
                            _returns.Apellidos = $"{pApellido} {sApellido}";
                            _returns.CURP = curp;
                            _returns.CIUDAD = ciudad;
                            return _returns;
                        }
                        _returns.NewToken = null;
                        _returns.Found = false;
                        _returns.ValidToken = false;
                        return _returns;
                    }
                    _returns.NewToken = null;
                    _returns.Found = false;
                    _returns.ValidToken = false;
                    return _returns;
                }
                catch
                {
                    var _newAccess = new Access
                    {
                        Cookie = _newCookies,
                        ViewState = _req.VIEW_STATE!
                    };

                    _returns.NewToken = _newAccess;
                    _returns.ValidToken = true;
                    _returns.Found = false;
                    return _returns;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                _returns.ValidToken = false;
                return _returns;
            }



        }

        //public Search byDataMoral(WorkMoral _req)
        //{
        //    var _returns = new Result.Search();
        //    try
        //    {
        //        var _visor = _web.Visor(_req.COOKIES!).Result;
        //        var _newHeaders = _tool.ManipulateHeader(_visor?.Headers!);
        //        var _visorContent = _tool.StringHTML(_visor.Content.ReadAsStringAsync().Result);
        //        var _viewStateVisor = _visorContent?.GetElementbyId("javax.faces.ViewState")?.GetAttributes("value")?.FirstOrDefault()?.Value;

        //        var _newCookies = _tool.ReplaceCookie(_req.COOKIES!, "F5-CONTENCION-rfcampe-443", _tool.FindCookieValue(_newHeaders.SetCookie, "F5-CONTENCION-rfcampe-443"));
        //        _newCookies = _tool.ReplaceCookie(_newCookies, "RFC_AMP_CUE_CONT_9080_e", _tool.FindCookieValue(_newHeaders.SetCookie, "RFC_AMP_CUE_CONT_9080_e"));

        //        _req.COOKIES = _newCookies;
        //        _req.VIEW_STATE = _viewStateVisor;

        //        var _clickRBtn = _web.ClickMoral(_req);
        //        var _clickRBtnHeaders = _clickRBtn.Headers;
        //        var _clickRBtnContent = _clickRBtn.Content.ReadAsStringAsync().Result;

        //        try
        //        {
        //            var _clickRBtnCookies = _tool.ManipulateHeader(_clickRBtnHeaders);
        //            var _clickRBtnViewState = _tool.StringHTML(_clickRBtnContent).GetElementbyId("javax.faces.ViewState").InnerHtml;
        //            _newCookies = _tool.ReplaceCookie(_newCookies, "F5-CONTENCION-rfcampe-443", _tool.FindCookieValue(_clickRBtnCookies.SetCookie, "F5-CONTENCION-rfcampe-443"));
        //            _newCookies = _tool.ReplaceCookie(_newCookies, "RFC_AMP_CUE_CONT_9080_e", _tool.FindCookieValue(_clickRBtnCookies.SetCookie, "RFC_AMP_CUE_CONT_9080_e"));

        //            _req.COOKIES = _newCookies;
        //            _req.VIEW_STATE = _clickRBtnViewState;

        //            var _result = _web.BuscarMoral(_req);
        //            var _headers = _result.Headers;
        //            var _cookiesSearch = _tool.ManipulateHeader(_headers);

        //            _newCookies = _tool.ReplaceCookie(_newCookies, "F5-CONTENCION-rfcampe-443", _tool.FindCookieValue(_cookiesSearch.SetCookie, "F5-CONTENCION-rfcampe-443"));
        //            _newCookies = _tool.ReplaceCookie(_newCookies, "RFC_AMP_CUE_CONT_9080_e", _tool.FindCookieValue(_cookiesSearch.SetCookie, "RFC_AMP_CUE_CONT_9080_e"));

        //            var _content = _result.Content.ReadAsStringAsync().Result;
        //            if (_content.Contains("expires"))
        //            {
        //                _returns.ValidToken = false;
        //                return _returns;
        //            }
        //            else
        //            {
        //                var _html = _tool.StringHTML(_content);


        //                try
        //                {
        //                    var _table = _tool.StringHTML(_html.GetElementbyId("visorForm:tablaResultados_data")?.InnerHtml!);
        //                    var _tds = _table?.DocumentNode.SelectNodes("//td");
        //                    var _resultViewState = _html.GetElementbyId("javax.faces.ViewState")?.InnerHtml;

        //                    for (var i = 0; i < _tds.ToArray().Length; i++)
        //                    {
        //                        var _label = _tds[i].SelectSingleNode("//label");
        //                        var _id = _label.GetAttributes("id").FirstOrDefault().Value;
        //                        if (_id.Contains("resRazonSoc"))
        //                        {
        //                            _returns.Names = _label.InnerHtml;
        //                        }
        //                        else if (_id.Contains("0:j_idt64"))
        //                        {
        //                            _returns.RFC = _label.InnerHtml;
        //                        }
        //                        else if (_id.Contains("0:j_idt67"))
        //                        {
        //                            _returns.CIUDAD = _label.InnerHtml;
        //                            _returns.Estado = _ESTADOS.STATE(_label.InnerHtml);
        //                        }
        //                        else if (_id.Contains("0:j_idt69"))
        //                        {
        //                            _returns.Apellidos = _label.InnerHtml;
        //                        }
        //                    }

        //                    var _clickRfc = _web.ClickRFCMoral(_newCookies, _resultViewState, _returns.RFC).Result;
        //                    var _clickRfcStr = _clickRfc.Content.ReadAsStringAsync().Result;
        //                    var _clickRfcCookies = _tool.ManipulateHeader(_clickRfc.Headers);

        //                    _newCookies = _tool.ReplaceCookie(_newCookies, "F5-CONTENCION-rfcampe-443", _tool.FindCookieValue(_clickRfcCookies.SetCookie, "F5-CONTENCION-rfcampe-443"));
        //                    _newCookies = _tool.ReplaceCookie(_newCookies, "RFC_AMP_CUE_CONT_9080_e", _tool.FindCookieValue(_clickRfcCookies.SetCookie, "RFC_AMP_CUE_CONT_9080_e"));

        //                    var _onInit = _web.InitVisualizador(_newCookies).Result;
        //                    var _onInitCookies = _tool.ManipulateHeader(_onInit.Headers);
        //                    var _onInitStr = _onInit.Content.ReadAsStringAsync().Result;
        //                    _newCookies = _tool.ReplaceCookie(_newCookies, "F5-CONTENCION-rfcampe-443", _tool.FindCookieValue(_onInitCookies.SetCookie, "F5-CONTENCION-rfcampe-443"));
        //                    _newCookies = _tool.ReplaceCookie(_newCookies, "RFC_AMP_CUE_CONT_9080_e", _tool.FindCookieValue(_onInitCookies.SetCookie, "RFC_AMP_CUE_CONT_9080_e"));
        //                    var _onResume = _web.HTMLResumen(_newCookies).Result;
        //                    var _onResumeStr = _onResume.Content.ReadAsStringAsync().Result;
        //                    var _onResumeCookies = _tool.ManipulateHeader(_onResume.Headers);
        //                    var _onResumeHTML = _tool.StringHTML(_onResumeStr);
        //                    var _onResumeView = _onResumeHTML.GetElementbyId("javax.faces.ViewState").InnerHtml;
        //                    var _sourceResume = _tool.ManipulateCDATA(_onResumeView);
        //                    _newCookies = _tool.ReplaceCookie(_newCookies, "F5-CONTENCION-rfcampe-443", _tool.FindCookieValue(_onResumeCookies.SetCookie, "F5-CONTENCION-rfcampe-443"));
        //                    _newCookies = _tool.ReplaceCookie(_newCookies, "RFC_AMP_CUE_CONT_9080_e", _tool.FindCookieValue(_onResumeCookies.SetCookie, "RFC_AMP_CUE_CONT_9080_e"));

        //                    return _returns;
        //                }
        //                catch
        //                {
        //                    _returns.Found = false;
        //                    _returns.ValidToken = true;
        //                    return _returns;
        //                }


        //            }
        //        }
        //        catch
        //        {
        //            _returns.ValidToken = false;
        //            return _returns;
        //        }
        //    }
        //    catch
        //    {
        //        _returns.ValidToken = false;
        //        return _returns;
        //    }
        //}


    }
}