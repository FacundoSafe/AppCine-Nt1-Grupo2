using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cine_NT1_Grupo2.Models
{
    public class Asiento
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength (1)]
        [RegularExpression("[a-zA-Z]",ErrorMessage="solamente se admiten Letras")]
        public char Fila { get; set; }

        [Required]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [StringLength(2, ErrorMessage = "El número de Asiento es demasiado largo")]
        [Range(1, 15 ,ErrorMessage = "solo se admiten numeros entre {1} y {2}")]
        public int Numero { get; set; }


        // relacion otra entidad


        [ForeignKey(nameof(Cliente))]
        public int ClienteId { get; set; }


        [ForeignKey(nameof(Funcion))]
        public int FuncionId { get; set; }


        /*
       private char fila;
       private int numero;



       public Asiento(char fila, int nro)
       {
           SetFila(fila);
           SetNro(nro);
       }

       public char GetFila()
       {
           return this.fila;
       }

       public int GetNumero()
       {
           return this.numero;
       }

       private void SetFila(char fila)
       {
           this.fila = ValidarFila(fila);
       }

       private char ValidarFila(char fila)
       {
           char[] filasPosibles = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j' };
           bool isValid = false;

           foreach(char x in filasPosibles)
           {
               if(fila == x)
               {
                   isValid = true;
               }
           }

           if (!isValid)
           {
               throw new ArgumentException("Ingrese una fila válida (a - j)");
           }

           return fila;
       }

       private void SetNro(int nro)
       {
           this.numero = ValidarNro(nro);
       }

       private int ValidarNro(int nro)
       {
           if(nro <= 0 || nro > 15)
           {
               throw new ArgumentException("Ingrese un número válido (1-15)");
           }

           return nro;
       }

       public bool MismoAsiento(char fila, int numero)
       {
           return this.fila == fila && this.numero == numero;
       }

       */
    }
}
