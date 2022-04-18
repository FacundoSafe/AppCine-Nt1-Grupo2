using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cine_NT1_Grupo2.Context;
using Cine_NT1_Grupo2.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cine_NT1_Grupo2.Controllers
{

    [Authorize]
    public class FuncionController : Controller
    {
        private readonly CineContext _context;

        public FuncionController(CineContext context)
        {
            _context = context;
        }

        // GET: Funcion
        public async Task<IActionResult> Index()
        {
            // El include permite hacer un join entre distintas tablas.. example.
            // Estimo que automaticamente une las keys para obtener el nombre?

            /*https://www.it-swarm-es.com/es/sql/que-hace-include-en-linq/1048745635/ */
            return View(await _context.Funcion
                .Include(funcion => funcion.Pelicula)
                 .ToListAsync());
        }

        // GET: Funcion/Details/5
     
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // Aca hago lo mismo. un join con Pelicula.
            var funcion = await _context.Funcion.Include(funcion => funcion.Pelicula)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (funcion == null)
            {
                return NotFound();
            }

            return View(funcion);
        }

        // GET: Funcion/Create
        [Authorize(Roles = nameof(Rol.ADMIN))]
        public IActionResult Create()
        {
            var entradasAElegir = from s in _context.Pelicula
                                  select s;

            ViewBag.FuncionesPelis = new SelectList(entradasAElegir.ToList(), "Id", "Nombre");


            return View();
        }

        // POST: Funcion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(Rol.ADMIN))]
        /*  Aca se agregan los elementos necesarios*/
        public async Task<IActionResult> Create([Bind("Id,Fecha,Sala,IdPelicula")] Funcion funcion)
        {
            

            if (ModelState.IsValid)
            {

                //*** valida que la fecha sea en el dia actual o posterior.. no pueden cargarse fechas pasadas. La comentada verificaria que alguna funcion en
                //la bd sea anterior. debemos verifiar la que esta por crearse ****
              
                /*  bool esfechaPrevia = _context.Funcion.Any(func => (func.Fecha <= DateTime.Today));*/

                bool esfechaPrevia = funcion.Fecha <= DateTime.Today;

                if (esfechaPrevia)
                {
                    ModelState.AddModelError("Fecha", "¡¡¡no podemos viajar al pasado!!");

                    var entradasAElegir = from s in _context.Pelicula
                                          select s;

                    ViewBag.FuncionesPelis = new SelectList(entradasAElegir.ToList(), "Id", "Nombre");

                    return View(funcion);
                }

                /* Chequeamos que no se repita la sala*/ 
                bool funcionExiste = _context.Funcion.Any(f => ((f.Fecha ==funcion.Fecha) &&( f.Sala == funcion.Sala )) );


                if (funcionExiste)
                {
                    ModelState.AddModelError("", "****Ya existe una funcion para esa fecha y sala****");

                    var entradasAElegir = from s in _context.Pelicula
                                          select s;

                    ViewBag.FuncionesPelis = new SelectList(entradasAElegir.ToList(), "Id", "Nombre");

                    return View(funcion);
                }

                _context.Add(funcion);

                /* Aca deberia encontrar como agregar asientos , ¿como podre indicarle el ID de mi funcion?
                 Tuve que agregarle la foreign key en el modelo para encontrar como indicarle.
                 */

                await _context.SaveChangesAsync();



                for ( char fila='A'; fila < 'E'; fila++)
                {
                    for (int numero=1; numero < 7; numero++)
                    {
                        Asiento asiento = new Asiento();
                        asiento.Fila = fila;
                        asiento.Numero = numero;
                        asiento.FuncionId = funcion.Id;
                        
                        /*la base de datos acepta null pero C# no porque un int no puede ser null. no entiendo porque la bd pondra null
                         * asiento.ClienteId = null;*/
                        _context.Add(asiento);
                    }

                }


               




                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(funcion);
        }

        // GET: Funcion/Edit/5
        [Authorize(Roles = nameof(Rol.ADMIN))]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            /* Que pasaria si hago un include en vez de un view bag? estaria bien , trate pero envia error*/
            var funcion = await _context.Funcion.FindAsync(id);
            if (funcion == null)
            {
                return NotFound();
            }

            var entradasAElegir = from s in _context.Pelicula
                                  select s;

            ViewBag.FuncionesPelis = new SelectList(entradasAElegir.ToList(), "Id", "Nombre");

            return View(funcion);
        }

        // POST: Funcion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(Rol.ADMIN))]
        /* Modificado para que pida el IdPelicula */
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,Sala,IdPelicula")] Funcion funcion)
        {
            if (id != funcion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(funcion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FuncionExists(funcion.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(funcion);
        }

        // GET: Funcion/Delete/5
        [Authorize(Roles = nameof(Rol.ADMIN))]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            /*Modifico la consulta para buscar el nombre de la pelicula */
            var funcion = await _context.Funcion.Include(funcion => funcion.Pelicula)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (funcion == null)
            {
                return NotFound();
            }

            return View(funcion);
        }

        // POST: Funcion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(Rol.ADMIN))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var funcion = await _context.Funcion.FindAsync(id);
            _context.Funcion.Remove(funcion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FuncionExists(int id)
        {
            return _context.Funcion.Any(e => e.Id == id);
        }
    }
}
