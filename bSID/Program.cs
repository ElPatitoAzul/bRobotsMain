using BackRobotTDM.bSID;
using BackRobotTDM.Models.PeticionesActasReqs;
using BackRobotTDM.Scripts.SID.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace bSID
{
    internal class Program
    {
        private readonly static WebServices APIConnect = new WebServices();
        private readonly static PDFManipulate PDF = new PDFManipulate();
        private readonly static Tools tools = new Tools();
        static void Main(string[] args)
        {
        }


        public async Task<dynamic> SOLICITAR_ACTA(Work _work)
        {

            /**
             * Type: NACIMIENTO
             * Search: CURP
             * Data: *data*
             * 
             * Access_token: *robotToken*
             * 
             * User_Id: *sid id user*
             * SID_User: *sid User*
             */
            string _result = APIConnect.OBTAIN_DATA(_work.Type, _work.Search, _work.Data, _work.AccessToken, _work.UserId, _work.Username).Result;
            if (!_result.Contains("Unauthorized"))
            {
                var _Data = JsonConvert.DeserializeObject<BackRobotTDM.Scripts.SID.Models.ResultModel>(_result);
                if (_Data != null)
                {
                    bool _download = await APIConnect.DOWNLOAD_FILE(_work.Id, _Data.folio.ToString(), _Data.cadena, _work.UserId, _work.AccessToken, _work.Username);
                    if (_download)
                    {
                        tools._PATHER_("states");
                        string src = $"cache/{_work.Id.ToString()}-{_Data.cadena}.pdf";
                        PDF.Enmarcar(src);
                        PDF.PrefsManagement(src, _work.Preferences, _work.Estado);
                        var _res = new ResponseModel()
                        {
                            Id = _work.Id,
                            Tipo = _work.Type,
                            Busqueda = _work.Search,
                            Cadena = _Data.cadena,
                            CURP = _Data.curp,
                            Nombres = _Data.nombre,
                            Apellidos = $"{_Data.primerApellido} {_Data.segundoApellido}",
                            Estado = _work.Estado,
                            FechaNac = _Data.fechaNacimiento
                        };

                        await APIConnect.ADD_CORTE(_Data.curp, Document(_work.Type), _work.Level0.ToString(), $"{_work.Id.ToString()}-{_Data.cadena}.pdf", $"{_res.Nombres} {_res.Apellidos}", _work.Estado);


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
