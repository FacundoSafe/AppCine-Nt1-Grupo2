using Cine_NT1_Grupo2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cine_NT1_Grupo2.Context
{
    public class CineContext : DbContext

    {

        public CineContext(DbContextOptions<CineContext> options) : base(options)
        {
        }

        public DbSet<Asiento> Asiento { get; set; }
        public DbSet<Cine> Cine { get; set; }
        
        public DbSet<Cliente> Cliente { get; set; }
   
        public DbSet<Entrada> Entrada { get; set; }
        public DbSet<Funcion> Funcion { get; set; }

        public DbSet<Pelicula> Pelicula { get; set; }
        public DbSet<Tarjeta> Tarjeta { get; set; }
    }
}
