namespace BackRobotTDM.mSID
{
    public class Tools
    {

        public bool _PATHER_(string _PATH)
        {
            try
            {
                if (Directory.Exists(_PATH))
                {
                    return true;
                }
                else
                {
                    Directory.CreateDirectory(_PATH);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }


        public void _TABLE_RESULT_()
        {
            string _html = "<table>\r\n<tr class=\"header\">\r\n<th>Folio</th>\r\n<th>Tipo</th>\r\n<th>Busqueda</th>\r\n<th>Cadena</th>\r\n<th>CURP</th>\r\n<th>Nombres</th>\r\n<th>Apellidos</th>\r\n<th>Estado</th>\r\n<th>Acciones</th>\r\n    </tr>\r\n    <tr class=\"Result\">\r\n      <td>7587db25-83ad-4dbf-b42e-16a30c6de97b</td>\r\n      <td>NACIMIENTO</td>\r\n      <td>CURP</td>\r\n      <td>N/A</td>\r\n      <td>GEPA010912HVZRRNA1</td>\r\n      <td>N/A</td>\r\n      <td>N/A</td>\r\n      <td>CHIAPAS</td>\r\n      <td>\r\n        <button>Descargar</button>\r\n      </td>\r\n    </tr>\r\n  </table>";
        }

        public bool _RM_(string _PATH)
        {
            try
            {
                if (Directory.Exists(_PATH))
                {
                    string[] files = Directory.GetFiles(_PATH);
                    string[] dirs = Directory.GetDirectories(_PATH);
                    foreach (string file in files)
                    {
                        File.SetAttributes(file, FileAttributes.Normal);
                        File.Delete(file);
                    }
                    Directory.Delete(_PATH, false);
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

    }
}
