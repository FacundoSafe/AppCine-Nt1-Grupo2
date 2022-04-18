using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cine_NT1_Grupo2.Models
{
   public class Pelicula
    {
    
        public int Id { get; set; }

        [Required(ErrorMessage = "Por favor, ingrese el nombre de la pelicula"),MaxLength(30)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Por favor, seleccione una calificacion")]
        public Calificacion Calificacion { get; set; }

        [Required(ErrorMessage = "Por favor, ingrese el tipo de genero")]
        public Genero Genero { get; set; }

        /*
        private string nombre;
        private Calificacion calificacion;
        private Genero genero;


        public Pelicula(string nombre, Calificacion calificacion, Genero genero)
        {
            setNombre(nombre);
            setCalificacion(calificacion);
            setGenero(genero);
        }

        /// <summary>
        /// valida que el nombre no sea null o vacio
        /// </summary>
        /// <param name="nombre"></param>
        private void setNombre(string nombre)
        {

            if (nombre == "" || nombre == null)
            {
                throw new ArgumentException("El nombre no puede estar vacio");
            }

            this.nombre = nombre;
        }

        /// <summary>
        /// Aparentemente C# no acepta que los enums sean nulos, dado que en definitiva representan un int por lo que no se que puedo validar.
        /// </summary>
        /// <param name="calificacion"></param>
        private void setCalificacion(Calificacion calificacion)
        {
            this.calificacion = calificacion;
        }

        /// <summary>
        ///  Aparentemente C# no acepta que los enums sean nulos, dado que en definitiva representan un int por lo que no se que puedo validar.
        /// </summary>
        /// <param name="genero"></param>
        private void setGenero(Genero genero)
        {
            this.genero = genero;
        }

        */
    }
}
