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
using System.Security.Claims;

namespace Cine_NT1_Grupo2.Controllers
{
    [Authorize]
    public class EntradaController : Controller
    {
        private readonly CineContext _context;

        public EntradaController(CineContext context)
        {
            _context = context;
        }

        // GET: Entrada
        public async Task<IActionResult> Index()
        {
            /* para buscar un id directamente usar metodo User.FindFirstValue */

            /*Creo anteriormente la lista porque sino me envia un error */
            List<Entrada> entradasModel;
            if (User.FindFirstValue(ClaimTypes.Role).ToString() != Rol.ADMIN.ToString())
            {
                var idCliente = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

                entradasModel = await _context.Entrada.Where(s => s.ClienteId.ToString() == idCliente).Include(entrada => entrada.Funcion.Pelicula)
                    .Include(entrada => entrada.Funcion.Asientos).ToListAsync();
            }
            else
            {
                /* Si es admin busco la lista total*/
                entradasModel = await _context.Entrada.Include(entrada => entrada.Funcion.Pelicula)
                   .Include(entrada => entrada.Funcion.Asientos).ToListAsync();
            }
            
           

            return View(entradasModel);
        }

        // GET: Entrada/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entrada = await _context.Entrada.Include(s =>s.Funcion).Include(s => s.Funcion.Pelicula).Include(s=> s.Asiento)
                .FirstOrDefaultAsync(m => m.EntradaId == id);
            if (entrada == null)
            {
                return NotFound();
            }

            return View(entrada);
        }

        // GET: Entrada/Create
        public IActionResult Create()
        {

            var entradasAElegir = from s in _context.Pelicula
                               select s;

            /* var entradasAElegir = _context.Funcion.Include(funcion => funcion.Pelicula).ToList();*/
            /*ViewBag.Funciones = (from item in entradasAElegir 
                                 select new SelectListItem
                                 {
                                     Text= item.Pelicula.Nombre,
                                     Value= item.Pelicula.Id.ToString()
                                 }
                );*/
            ViewBag.FuncionesPelis = new SelectList(entradasAElegir.ToList(), "Id", "Nombre");


            return View();
        }

        // POST: Entrada/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FuncionId,AsientoId")] Entrada entrada)
        {
            if (ModelState.IsValid)
            {


                /*Como el usuario podia inspeccionar el elemento y modificarlo decidimos obtener el id directo en la accion
                 *  https://www.delftstack.com/es/howto/csharp/how-to-convert-string-to-int-in-csharp/
                 * Tuve que pasarlo a int*/

                var texto = User.FindFirstValue(ClaimTypes.NameIdentifier);

                entrada.ClienteId = (int)Int64.Parse(texto);

                /* Previamente  a agregar la entrada agrego al asiento el Id del cliente*/
                var idAsientoABuscar = entrada.AsientoId;
         
                if (idAsientoABuscar != null)
                {

                    /* al ser async estamos obligados a usar await que se refiere a tareas asincronicas
                     https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/
                    */
                    var asiento = await _context.Asiento.FirstOrDefaultAsync(m => m.Id == idAsientoABuscar);
                    
                    
                    /*Valido 1) que el asiento este libre. 2) que el asiento pertenezca a la funcion.*/

                    if (asiento.ClienteId==0 && asiento.FuncionId==entrada.FuncionId)
                    {
                        asiento.ClienteId = entrada.ClienteId;
                        

                    }
                    else
                    {
                        /*Si hay un error informo el error al cliente */
                        TempData["Mensaje"] = "hubo un error, intente nuevamente";
                        return RedirectToAction(nameof(Index));
                     
                    }
                }
                             
                _context.Add(entrada);
                await _context.SaveChangesAsync();

                /*Uso del viewbag para alerts... https://es.stackoverflow.com/questions/350669/alert-en-mvc-c
                 * https://www.c-sharpcorner.com/blogs/alert-message-from-controller-view-using-javascript-alert-messagebox*/

                /*Como antes hace un request a index debo usar TempData que dura mas
                 * https://stackoverflow.com/questions/47291712/tempdata-not-displaying-success-message-asp-net-mvc */

                TempData["Mensaje"] = "Creacion de entrada exitosa";

                return RedirectToAction(nameof(Index));
            }
            return View(entrada);
        }
             
       

        // GET: Entrada/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entrada = await _context.Entrada
                .FirstOrDefaultAsync(m => m.EntradaId == id);

            if (User.FindFirstValue(ClaimTypes.Role).ToString() != Rol.ADMIN.ToString())
            {
                var idDelCliente = (int)Int64.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                /* Valido que la entrada pertenezca a este cliente*/
                if (entrada == null || entrada.ClienteId != idDelCliente)
                {
                    return NotFound();
                }

            }
            else if (entrada == null)
            {
                return NotFound();
            }

            return View(entrada);
        }

        // POST: Entrada/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            /*Por algun motivo no parece ser necesario validar esto. aunque el usuario cambie algo con inspeccionar sigue estando el mismo id que deberia haber */

            var entrada = await _context.Entrada.FindAsync(id);

            if (User.FindFirstValue(ClaimTypes.Role).ToString() != Rol.ADMIN.ToString())
            {

                var idDelCliente = (int)Int64.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (entrada != null && entrada.ClienteId == idDelCliente)
                {
                    /*Antes de eliminar la entrada busco su asiento y lo vuelvo 0 para que se pueda seleccionar */
                    var asiento = await _context.Asiento.FirstOrDefaultAsync(m => m.Id == entrada.AsientoId);

                    if (asiento != null)
                    {

                        asiento.ClienteId = 0;
                        _context.Update(asiento);
                    }
                }
                else
                {
                    TempData["Mensaje"] = "EY, esa entrada no le pertenece :( ";
                    return RedirectToAction(nameof(Index));
                }


                _context.Entrada.Remove(entrada);
                await _context.SaveChangesAsync();


                TempData["Mensaje"] = "Su entrada fue eliminada ";
                return RedirectToAction(nameof(Index));
            }
            else
            {

                if (entrada != null)
                {
                    /*Antes de eliminar la entrada busco su asiento y lo vuelvo 0 para que se pueda seleccionar */
                    var asiento = await _context.Asiento.FirstOrDefaultAsync(m => m.Id == entrada.AsientoId);

                    if (asiento != null)
                    {

                        asiento.ClienteId = 0;
                    }
                }


                _context.Entrada.Remove(entrada);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

                
        }

        private bool EntradaExists(int id)
        {
            return _context.Entrada.Any(e => e.EntradaId == id);
        }


        /*Este metodo deberia pasar un id de peli y traer todas las fechas posibles 
         Notar que el nombre del parametro debe coincidir con lo que se le envio del viewbag
         */ 
        public async Task<IActionResult> SelectFecha(int? IdPelicula)
        {
            if (IdPelicula == null)
            {
                return NotFound();
            }
      
            var pelicula = await _context.Pelicula.FindAsync(IdPelicula);
            if (pelicula == null)
            {
                return NotFound();
            }

            var fechas = from s in _context.Funcion
                         where s.IdPelicula== IdPelicula
                         select s;

            ViewBag.FechasPelis = new SelectList(fechas.ToList(), "Id", "Fecha");

            return View(pelicula);
        }


        /*El siguiente metodo tiene por objetivo enviar los asientos de la funcion seleccionada */

        public async Task<IActionResult> SelectAsiento(int? IdFuncion)
        {
            if (IdFuncion == null)
            {
                return NotFound();
            }

            /* Por algun motivo el include no se lleva bien con 
             * find async https://stackoverflow.com/questions/40360512/findasync-and-include-linq-statements */
            var funcion = await _context.Funcion.Include(f =>f.Pelicula).FirstOrDefaultAsync(s => s.Id == IdFuncion);
            if (funcion == null)
            {
                return NotFound();
            }
            /*trate de usar first or default pero me devuelve un solo elemento */
            /* var asientosTotales = from s in _context.Funcion
                                   where s.Id == IdFuncion
                                   select s.Asientos;
            */

            /*Al final era mas sencillo hacerlo de la forma tradicional 
             * https://www.tutorialsteacher.com/linq/linq-joining-operator-join
             * */

            /* Creo que la consulta puede armarse mejor, pero al menos funciona*/

            var asientosTotales = from s in _context.Funcion
                                  join st in _context.Asiento
                                  on s.Id equals st.FuncionId
                                  where (s.Id == IdFuncion && st.ClienteId==0)
                                  orderby st.Fila ascending, st.Numero ascending
                                  select st;


          
       

            /* Al ser una lista creo que se devuelve distinto*/
            /* https://stackoverflow.com/questions/668589/how-can-i-add-an-item-to-a-selectlist-in-asp-net-mvc 
             https://stackoverflow.com/questions/14663971/using-linq-select-list-inside-list
             */


            List<SelectListItem> listaAsientos = new List<SelectListItem>();

            foreach(var  item in asientosTotales)
            {

                listaAsientos.Add(new SelectListItem()
                {
                    Text = item.Fila.ToString() + item.Numero.ToString(),
                    Value = item.Id.ToString()
                });


            }


            ViewBag.AsientoPeli = new SelectList(listaAsientos, "Value", "Text");

            return View(funcion);
        }


    }
}
