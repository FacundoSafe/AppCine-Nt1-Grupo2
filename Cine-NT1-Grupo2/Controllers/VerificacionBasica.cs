using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Cine_NT1_Grupo2.Controllers
{

    //**ESTA CLASE NOS PERMITE VALIDAR Y ENCRIPTAR LA CONTRASEÑA IMPLEMENTA UNA
    //INTERFAZ PREVIAMENTE ESTABLECIDA (Seguridad)SEPARANDO LA LOGICA DE NEGOCIOS
    public class VerificacionBasica : Seguridad
    {
        /*
         * AQUI SE ENCRIPTA DEVOLVIENDO UN BYTE ARRAY (TODAVIA NO FUE IMPLEMENTADA EN NINGUN LADO
         * */
        public byte[] EncriptarPass(string pass)
        {
            return new SHA256Managed().ComputeHash(Encoding.ASCII.GetBytes(pass));
        }
        /* AQUI SE VALIDAN LAS CONTRASEÑAS QUE LE PASAMOS POR PARAMETRO
         */
        public bool validarPass(string pass)
        {
            if (string.IsNullOrEmpty(pass))
            {
                return false;
            }
           
            //uso de RegEx para validar la contraseña
            var tieneMay = Regex.Match(pass, $"[a-z]").Success;
            var tieneMin = Regex.Match(pass, $"[A-Z]").Success;
            var tieneNum = Regex.Match(pass, $"[0-9]").Success;

            return pass.Length > 7 && tieneMay && tieneMin && tieneNum;
        }
    }
}