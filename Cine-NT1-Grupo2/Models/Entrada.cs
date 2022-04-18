using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cine_NT1_Grupo2.Models
{
   public class Entrada
    {

        [Required]
        public int EntradaId { get; set; }


        /* No conozco bien las cuestiones de fondo, pero aparentemente se produjo un efecto cascada porque
         hay muchas FK y si uno quisiera borrar el otro no lo permitiria por eso con ? permito que sea opcional
        https://stackoverflow.com/questions/17127351/introducing-foreign-key-constraint-may-cause-cycles-or-multiple-cascade-paths*/
        [ForeignKey(nameof(Funcion))]
        public int? FuncionId { get; set; }
        public Funcion Funcion { get; set; }


        [ForeignKey(nameof(Cliente))]
        public int ClienteId { get; set; }

        [ForeignKey(nameof(Asiento))]
        public int AsientoId { get; set; }
        public Asiento Asiento { get; set; }
        /*
        public Entrada(Funcion f, Asiento a)
        {
            this.funcion = f;
            this.asiento = a;
        }

        public Funcion Funcion
        {
            get { return funcion; }
            set { SetAtt(this.funcion, value); }
        }

        public Asiento Asiento
        {
            get { return asiento; }
            set { SetAtt(this.asiento, value); }
        }

        /// <summary>
        /// si x es null, se asigna y a x.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void SetAtt(Object x, Object y)
        {
            if(x == null)
            {
                x = y;
            }
        }

        */
    }
}
