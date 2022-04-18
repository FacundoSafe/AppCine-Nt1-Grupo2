using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cine_NT1_Grupo2.Models
{
  public  class Cine
    {

        public int Id { get; set; }
        [Required(ErrorMessage ="Ingrese el nombre del cine")]
        public string Nombre { get; set; }
        public Cliente ClienteActual { get;  set; }
        public List<Cliente> Clientes { get; set; }
        public List<Funcion> Funciones { get; set; }


        /*
        private String nombre;
        private Cliente clienteActual;// PREGUNTAR A MARIANO PARA QUE SERVIA ESTE CLIENTE... ¿EL METODO LOGGUEARSE TIENE ALGO QUE VER?
        private List<Cliente> clientes;
        private List<Funcion> funciones;
        //PODRIA AGREGARSE UN NUMERO O NOMBRE DE SEDE O BARRIO EN EL QUE SE ENCUENTRA CINEMA ORT CABALLITO, CINEMAORT LAVALLE... LO QUE SEA.

        public Cine()
        {
            nombre = "CinemaOrt";//EL NOMBRE ES INVARIABLE PARA EL CINE CON LO CUAL PODRIAMOS INICIALIZARLO EN EL CONSTRUCTOR SIN RECIBIRLO POR PARAMETRO
            clientes = new List<Cliente>();
            funciones = new List<Funcion>();

        }



        /// <summary>
        /// se encarga de la compra de una entrada recibiendo por parametro todos los datos necesrios
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="funcionSeleccionada"></param>
        /// <param name="asientoSeleccionado"></param>
        /// <returns></returns>
        public bool comprarEntrada(Cliente cliente, Funcion funcionSeleccionada, Asiento asientoSeleccionado)
        {
            bool seCompro = false;
            Funcion f = buscarFuncion(funcionSeleccionada);


            if (f != null)
            {
                f.ocuparAsiento(asientoSeleccionado);
                clienteActual = cliente;
                clienteActual.AgregarEntrada(new Entrada (f, asientoSeleccionado));
                seCompro = true;

            }



            return seCompro;
        }




        /// <summary>
        /// Busca una funcion dentro de la lista de funciones que posee el Cine utilizando Sala Hora y Fecha de la funcion seleccionada
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private Funcion buscarFuncion(Funcion f)
        {
            int i = 0;
            Funcion funcionBuscada = null;
            int totalFunciones = funciones.Count;


            while (i < totalFunciones && !funciones[i].mismaFuncion(f.getSala(), f.getHora(), f.getfecha()))
            {

                i++;
            }

            if (i < totalFunciones)
            {
                funcionBuscada = this.funciones[i];
            }


            return funcionBuscada;
        }


        /// <summary>
        /// busca un cliente en la lista de clientes y lo devuelve... o devuelve null si no lo encuentra
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private Cliente buscarCliente(String mail)
        {
            int i = 0;
            Cliente clienteBuscado = null;
            int totalClientes = clientes.Count;
            
            while (i < totalClientes && !clientes[i].mismoMail(mail))
            {
                i++;
            }
           
            if (i < totalClientes)
            {
                clienteBuscado = clientes[i];
            }

            
            return clienteBuscado;

        }

        /// <summary>
        /// Agrega un cliente a la Lista de clientes solo en el caso que sea null
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        public bool registrarse(String nombre, String apellido, String mail, String contraseña)// ACORDARSE DE VALIDAR LA CONTRASEÑA
        {

            bool pudeAgregar = false;
            Cliente clienteAgregar = null;
            
            if (!String.IsNullOrEmpty(nombre) && !String.IsNullOrEmpty(apellido) && !String.IsNullOrEmpty(mail) && !String.IsNullOrEmpty(contraseña))// NO CREO QUE SEA NECESARIO VALIDAR ACA PORQUE SE HARIA EN FRONT END PERO ES SOLO BORRAR DOS LINEAS
               
            {
                if (this.buscarCliente(mail) == null)
                {
                   
                    clienteAgregar = new Cliente(nombre, apellido, mail, contraseña);
                    
                    this.clientes.Add(clienteAgregar);
                    
                    pudeAgregar = true;
                    
                }
            }
            


            return pudeAgregar;
        }




        /// <summary>
        /// devuelve booleano indicando que ha podido iniciarse sesion, despues de verificar los datos ingresados
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="contraseña"></param>
        /// <returns></returns>
        public bool loguearse(String mail, String contraseña) // QUIZAS MAS QUE DEVOLVER UN BOOLEAN DEBERIA SETEAR UN BOOLEAN EN EL CLIENTE
        {
            bool pudoLoguear = false;
            Cliente c = buscarCliente(mail);//Directamente paso a inicializar porque en FRONTEND deberiamos tener validado que los campos no esten vacios

            if (c != null)
            {
                if (c.getPasword(contraseña))//NECESITO UN METODO QUE ME DEJE COMPARAR LA CONTRASEÑA UN GET PASSWORD EN CLIENTE
                {
                    pudoLoguear = true;
                    Console.WriteLine("Ha ingresado Correctamente");
                }
                else
                {
                    Console.WriteLine ("***Contraseña Incorrecta***");
                }
            }
            else
            {
                Console.WriteLine("***el Mail no se encuentra Registrado***");
            }

            return pudoLoguear;
        }












        // CIERRE DE LA CLASE
*/
    }
}