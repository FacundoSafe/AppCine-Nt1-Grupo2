using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cine_NT1_Grupo2.Models
{
  public  class Cliente
    {
        [Required]
        public int id { get; set; }

        // chequea que los usuarios carguen sus datos completos y correctos, no admitiendo formatos de numeros en nombre
        [Required(ErrorMessage = "El nombre es requerido, no admitimos usuarios Anonomios"), MaxLength(20)]
        [RegularExpression("[a-zA-Z ]*", ErrorMessage = "el campo {0} no admite numeros")]
        public string Nombre { get;set;}
         
        // el apellido debe estar con el campo completo y no admite numeros
        [Required(ErrorMessage = "El Apellido es requerido"), MaxLength(20)]
        [RegularExpression("[a-zA-Z ]*", ErrorMessage = "el campo {0} no admite numeros")]
        public string Apellido { get; set; }

        //el campo mail tiene formato valido de mail
        [Required(ErrorMessage = "El mail es requerido"), MaxLength(20)] // deberiamos aumentar el tamaño permitido tambien en la base de datos
        [EmailAddress(ErrorMessage = "Debe ser un Mail con formato VALIDO")]
        public string Mail { get; set; }

        // la contraseña requerida no solo debe cumplir con un minimo de caracteres sino que no puede superar un maximo de ellos
        
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        
        public string pass { get; set; }
        
        /*
        [Compare("Password", ErrorMessage = "La contraseña no coincide, VOLVE A INTENTAR !")]
        public string ConfirmarContraseña { get; set; }
        */


        // Aca no se si iria un notation...
        public List<Entrada> entradas { get; set; }
        public List<Tarjeta> tarjetas{ get; set; }

        /* Se agrega un rol*/ 
        [Required]
        public Rol Rol { get; set; }

        /*
        private string nombre;
        private string apellido;
        private string email;
        private string pass;
        private List<Entrada> entradas;
        private List<MedioDePago> mediosDePago;

        public Cliente(string nombre, string apellido, string email, string pass)
        {
            entradas = new List<Entrada>();
            SetMediosDePago();
            SetNombre(nombre);
            SetApellido(apellido);
            SetEmail(email);
            SetPassword(pass);
        }

        private void SetNombre(string nombre)
        {
            this.nombre = ValidarApenom(nombre);
        }

        private void SetApellido(string apellido)
        {
            this.apellido = ValidarApenom(apellido);
        }

        private void SetEmail(string email)
        {
            this.email = ValidarEmail(email);
        }

        private void SetPassword(string pass)
        {
            this.pass = ValidarPass(pass);
        }

        public bool getPasword(string pass)
        {
            return this.pass == pass;
        }

        /// <summary>
        /// instancia mediosdepago y agrega un Efectivo por default
        /// </summary>
        private void SetMediosDePago()
        {
            this.mediosDePago = new List<MedioDePago>();

            this.mediosDePago.Add(new EnEfectivo());
        }

        /// <summary>
        /// me falta este
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private string ValidarEmail(string email)
        {
            //me falta la lógica

            return email;
        }

        private string ValidarPass(string pass)
        {
            char[] vPass = pass.ToCharArray();

            if (!ValidarLargoPass(vPass))
            {
                throw new ArgumentException("El largo de la contrasenia debe ser de 8 o más caracteres.");
            }
            if (!ValidarPassTieneMay(vPass))
            {
                throw new ArgumentException("Debe contener al menos 1 mayúscula.");
            }

            return pass;
        }

        /// <summary>
        /// devuelve true si al menos un elemento del char[] esta en mayusculas.
        /// </summary>
        /// <param name="vPass"></param>
        /// <returns></returns>
        private Boolean ValidarPassTieneMay(char[] vPass)
        {
            bool res = false;
            int i = 0;

            while(i < vPass.Length && !res)
            {
                if (Char.IsUpper(vPass[i])){
                    res = true;
                }

                i++;
            }

            return res;
        }

        /// <summary>
        /// devuelve true si es de 8 o mas
        /// </summary>
        /// <param name="vPass"></param>
        /// <returns></returns>
        private bool ValidarLargoPass(char[] vPass)
        {
            const int LARGO_MIN = 8;

            return vPass.Length >= LARGO_MIN;
        }

        /// <summary>
        /// throws argument exception si el parametro contiene numeros o esta vacio
        /// </summary>
        /// <param name="n">texto</param>
        /// <returns></returns>
        private string ValidarApenom(string n)
        {
            char[] str = n.ToCharArray();
            int i = 0;
            
            if(!string.IsNullOrEmpty(n))
            {
                while (i < str.Length)
                {
                    if (Char.IsDigit(str[i]))
                    {
                        throw new ArgumentException("No puede contener números.");
                    }
                }
            }
            else
            {
                throw new ArgumentException("No puede estar vacío");
            }

            return n;
        }

        public bool mismoMail(string str)
        {
            return this.email.Equals(str);
        }

        /// <summary>
        /// busca tarjeta segun el nro pasado por parametro. devuelve el objeto o null si no la encuentra
        /// </summary>
        /// <param name="nroTarjeta"></param>
        /// <returns></returns>
        private MedioDePago BuscarTarjeta(int nroTarjeta)
        {
            MedioDePago m = null;
            int i = 0;

            while (i < this.mediosDePago.Count && m == null){

                MedioDePago medioActual= this.mediosDePago[i];

                if(medioActual is Tarjeta) 
                {
                    Tarjeta tarjeta = (Tarjeta) medioActual;

                    if (tarjeta.mismoNumeroTarjeta(nroTarjeta))
                    {
                        
                        m = this.mediosDePago[i];
                    }
                }
                else
                {
                    i++;
                  
                }
              
            }

            return m;
        }

        //Si le paso medio de pago, despues tengo que hacer un get numero, ¿eso se podria? en caso de hacer una sobrecarga, hay alguna forma de optimizarlo

        public bool AgregarMedioDePago(int numero, DateTime fechaDeVencimiento, int codigoSeguridad, string nombre)
        {
            MedioDePago m = BuscarTarjeta(numero);
            bool resultado = false;

            if(m == null)
            {
                this.mediosDePago.Add(new Debito(numero , fechaDeVencimiento, codigoSeguridad, nombre));
                resultado = true;
            }

            return resultado;
        }


        public bool AgregarMedioDePago(int numero, DateTime fechaDeVencimiento, int codigoSeguridad, string nombre, int cantidadCuotas)
        {
            MedioDePago m = BuscarTarjeta(numero);
            bool resultado = false;

            if (m == null)
            {
                this.mediosDePago.Add(new Credito(numero, fechaDeVencimiento, codigoSeguridad, nombre, cantidadCuotas));
                resultado = true;
            }

            return resultado;
        }

        public void AgregarEntrada(Entrada e)
        {
            this.entradas.Add(e);
        }

        public void AgregarEntrada(Entrada e, int cant)
        {
            for(int i = 0; i < cant; i++)
            {
                this.entradas.Add(e);
            }
        }
    
        public bool RealizarPago()
        {
            bool res = false;

            if(this.entradas.Count > 0)
            {
                res = true;
            }

            return res;
        }
        */
    }
}
