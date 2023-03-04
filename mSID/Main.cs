using BackRobotTDM.Models.PeticionesActasReqs;
using BackRobotTDM.Scripts.SID.Models;
using Newtonsoft.Json;

namespace BackRobotTDM.mSID
{
    /// <summary>
    /// @CarlosMontoya - CMEnterprise *SID REQUESTS SCRIPT*
    /// </summary>
    public class Main
    {
        private readonly WebServices APIConnect = new WebServices();



        public async Task<Modelos.SID.ResponseModel> MAIN(Modelos.SID.New _req, Modelos.Robots.RobotModel _robotUsage)
        {

                dynamic? _CURP = null;
                dynamic? _Cadena = null;
                if (_req.Search == "CURP") _CURP = _req.Data;
                else if (_req.Search == "Cadena") _Cadena = _req.Data;
                var _Id = Guid.NewGuid();
                var _res = new Modelos.SID.ResponseModel();
                _res.Id = _Id;
                string _resultData = await APIConnect.OBTAIN_DATA(_req.Type, _req.Search, _req.Data, _robotUsage?.AccessToken!, _robotUsage?.UserId!, _robotUsage?.Username!);
                if (!_resultData.Contains("Unauthorized"))
                {
                    if (_resultData.Contains("},{"))
                    {
                        var _first = _resultData.Split('}');
                        _resultData = _first[0]+'}';
                    }
                    var _Data = JsonConvert.DeserializeObject<Modelos.SID.ResultDataModel>(_resultData);

                    if (_Data != null)
                    {
                        var _downPDF = APIConnect.DOWNLOAD_PDF(_Data.folio.ToString(), _Data.cadena, _robotUsage?.UserId!, _robotUsage?.AccessToken!, _robotUsage?.Username!);
                        _res.StatusResponse = _downPDF.StatusCode;
                        if (_downPDF.StatusCode == System.Net.HttpStatusCode.OK)
                        {

                            var fs = new FileStream($@"cache/{_Id}-{_Data.cadena}.pdf", FileMode.CreateNew);
                            _downPDF.Content.CopyToAsync(fs).Wait();
                            _res.Tipo = _req.Type;
                            _res.Busqueda = _req.Search;
                            _res.Cadena = _Data.cadena;
                            _res.CURP = _Data.curp != null ? _Data.curp : _Data.curpEl != null ? _Data.curpEl : _Data.curpElla != null ? _Data.curpElla : "";
                            _res.Nombres = _Data.nombre!=null? _Data.nombre: _Data.nombreEl;
                            _res.Apellidos = _Data.primerApellido!= null?_Data.primerApellido: _Data.primerApellidoEl+' '+ _Data.segundoApellido != null ? _Data.segundoApellido : _Data.segundoApellidoEl;
                            //_res.Estado = _Data.estadoNacNombre != null ? _Data.estadoNacNombre : _Data.estadoRegNombre;
                            _res.Estado = _Data.estadoRegNombre != null ? _Data.estadoRegNombre : _Data.estadoNacNombre;
                            _res.FechaNac = _Data.fechaNacimiento != null ? _Data.fechaNacimiento : _Data.fechaNacimientoEl != null ? _Data.fechaNacimientoEl : "";
                            _res.Found = true;
                            _res.Comments = null;
                            var _corte = await APIConnect.ADD_CORTE(_res.CURP!, Document(_req.Type), _req.UserId.ToString(), $"{_Id}-{_Data.cadena}.pdf", $"{_res.Nombres} {_res.Apellidos}", _res.Estado);
                            var _addCorte = JsonConvert.DeserializeObject<Modelos.Corte.AddCorte>(_corte);
                            _res.CorteId = _addCorte!.id;
                            return _res;
                        }
                        else if (_downPDF.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            _res.Found = false;
                            _res.Comments = "Token";
                            return _res;
                        }
                        else
                        {
                            _res.Found = false;
                            _res.Comments = "No found";
                            return _res;
                        }
                    }
                    else
                    {
                        _res.Found = false;
                        _res.Comments = "No found";
                        return _res;
                    }
                }
                else
                {
                    _res.Found = false;
                    _res.Comments = "Token";
                    return _res;
                };

        }






        public async Task<dynamic> SOLICITAR_ACTA(Work _work)
        {

            string _result = APIConnect.OBTAIN_DATA(_work.Type, _work.Search, _work.Data, _work.AccessToken, _work.UserId, _work.Username).Result;
            if (!_result.Contains("Unauthorized"))
            {
                var _Data = JsonConvert.DeserializeObject<ResultModel>(_result);
                Console.WriteLine(_result.ToString());
                if (_Data != null)
                {
                    bool _download = await APIConnect.DOWNLOAD_FILE(_work.Id, _Data.folio.ToString(), _Data.cadena, _work.UserId, _work.AccessToken, _work.Username);
                    if (_download)
                    {

                        var _res = new ResponseModel()
                        {
                            Id = _work.Id,
                            Tipo = _work.Type,
                            Busqueda = _work.Search,
                            Cadena = _Data.cadena,
                            CURP = _Data.curp != null ? _Data.curp : _Data.curpEl != null ? _Data.curpEl : _Data.curpElla != null ? _Data.curpElla : "",
                            Nombres = _Data.nombre,
                            Apellidos = $"{_Data.primerApellido} {_Data.segundoApellido}",
                            Estado = _Data.estadoNacNombre != null ? _Data.estadoNacNombre : _Data.estadoRegNombre,
                            FechaNac = _Data.fechaNacimiento != null ? _Data.fechaNacimiento : _Data.fechaNacimientoEl != null ? _Data.fechaNacimientoEl : ""
                        };
                        await APIConnect.ADD_CORTE(_Data.curp, Document(_work.Type), _work.Level0.ToString(), $"{_work.Id}-{_Data.cadena}.pdf", $"{_res.Nombres} {_res.Apellidos}", _work.Estado);
                        return _res;
                    }
                    else
                    {
                        return null;
                    }
                }
                else return null;
            }
            else
            {
                return "TOKEN CADUCADO";
            }
        }

        private string Document(string Type)
        {
            return $"Acta de {Type.Substring(0, 1).ToUpper() + Type.Substring(1).ToLower()}";
        }




    }
}
