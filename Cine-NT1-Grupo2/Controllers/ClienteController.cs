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
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Cine_NT1_Grupo2.Controllers
{

    [Authorize]
    public class ClienteController : Controller
    {
        private readonly CineContext _context;
        private readonly Seguridad seguridad = new VerificacionBasica();
        public ClienteController(CineContext context)
        {
            _context = context;
        }


        // GET: Cliente
        [Authorize]
        public async Task<IActionResult> Index()

        {

            List<Cliente> clienteDevolver;

            if (User.FindFirstValue(ClaimTypes.Role).ToString() != Rol.ADMIN.ToString())
            {
                var idCliente = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

                clienteDevolver = await _context.Cliente.Where(s => s.id.ToString() == idCliente).ToListAsync();
            }
            else
            {
                clienteDevolver = await _context.Cliente.ToListAsync();
            }


                return View(clienteDevolver);
        }


        // GET: Cliente/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           

               

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            if (User.FindFirstValue(ClaimTypes.Role).ToString() != Rol.ADMIN.ToString())
            {
                var idDelCliente = (int)Int64.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (cliente.id != idDelCliente)
                {
                    return NotFound();
                }

            }



            return View(cliente);
        }

        // GET: Cliente/Create

       [AllowAnonymous]
        public IActionResult Registrarse()
        {
            return View();
        }


        // POST: Cliente/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Registrarse([Bind("id,Nombre,Apellido,Mail,pass")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                // Vamos a chequear si el mail existe
                bool mailExiste = _context.Cliente.Any(c => c.Mail == cliente.Mail);

                if (mailExiste)
                {

                    /* Agregar validaciones al model: ver 
                     * https://desarrolloweb.com/articulos/validaciones-aspnet-mvc-donet.html
                     */
                    /*En el caso de que no queremos que se le asigne a una propiedad, debeboms poner simplemente ""
                     * en el primer parametro. (ej , aqui donde dice Mail, iria ""*/
                    ModelState.AddModelError("Mail", "Ya existe un usuario con este mail");

                    return View(cliente);
                }
                //valida la contraseña a travez de la clase que implementa la interfaz segruidad
                if (seguridad.validarPass(cliente.pass))
                {
                    cliente.Rol = Rol.USUARIO;
                    _context.Add(cliente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "contraseña NO VALIDA: NO puede estar vacia, ser menor a 8 caracteres y debe contener una mayuscula una minúscula y un numero");
                }
                
            }
            return View(cliente);
        }

        // GET: Cliente/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var cliente = await _context.Cliente.FindAsync(id);

            

                if (cliente == null)
            {
                return NotFound();
            }


            if (User.FindFirstValue(ClaimTypes.Role).ToString() != Rol.ADMIN.ToString())
            {
                var idDelCliente = (int)Int64.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (cliente.id != idDelCliente)
                {
                    return NotFound();
                }

            }


            return View(cliente);
        }

        // POST: Cliente/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Nombre,Apellido,Mail,pass")] Cliente cliente)
        {

            if (User.FindFirstValue(ClaimTypes.Role).ToString() != Rol.ADMIN.ToString())
            {
                var idDelCliente = (int)Int64.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (cliente.id != idDelCliente)
                {
                    
                    TempData["MensajeCliente"] = "hubo un error, intente nuevamente";
                    return RedirectToAction(nameof(Index));
                }

            }


            if (id != cliente.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                bool mailExiste = _context.Cliente.Any(c => c.Mail == cliente.Mail);

                if (mailExiste)
                {
                                        
                    ModelState.AddModelError("Mail", "Ya existe un usuario con este mail");

                    return View(cliente);
                }
               
                if (seguridad.validarPass(cliente.pass))
                {
                    /* Sino lo aclaro lo modifica a 0*/
                    cliente.Rol = Rol.USUARIO;
                                      
                }
                else
                {
                    ModelState.AddModelError("", "contraseña NO VALIDA: NO puede estar vacia, ser menor a 8 caracteres y debe contener una mayuscula una minúscula y un numero");
                    return View(cliente);
                }


                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.id))
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
            return View(cliente);
        }

        // GET: Cliente/Delete/5
        [Authorize(Roles = nameof(Rol.ADMIN))]
        public async Task<IActionResult> Delete(int? id)
        {




            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(m => m.id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }



        // POST: Cliente/Delete/5
        [Authorize(Roles = nameof(Rol.ADMIN))]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);

            if (cliente != null)
            {

                /*Antes de borrar al cliente seteo los asientos en 0 de sus entradas */
                var entradasModel = await _context.Entrada.Where(s => s.ClienteId.ToString() == cliente.id.ToString()).Include(entrada => entrada.Funcion.Pelicula)
                 .Include(entrada => entrada.Funcion.Asientos).ToListAsync();

                foreach (var ent in entradasModel)
                {
                    ent.Asiento.ClienteId = 0;
                }
            }



            _context.Cliente.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = nameof(Rol.ADMIN))]
        private bool ClienteExists(int id)
        {
            return _context.Cliente.Any(e => e.id == id);
        }

        /*No entiendo el motivo pero si o si el string debe llamarse returnUrl */
        /*RTA https://www.it-swarm-es.com/es/asp.net/como-funciona-la-redireccion-returnurl-en-asp.net-mvc5/1044284456/ */
        /* https://stackoverflow.com/questions/20123612/how-am-i-supposed-to-use-returnurl-viewbag-returnurl-in-mvc-4 */

        [AllowAnonymous]
        public IActionResult Loguearse(string returnUrl)
        {
            TempData["UrlAEnviar"] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Loguearse(string mail, string pass)
        {
            var UrlAEnviar = TempData["UrlAEnviar"] as string;

            if (!string.IsNullOrEmpty(mail) && !string.IsNullOrEmpty(pass))
            {

                var cliente = _context.Cliente.FirstOrDefault(s => s.Mail == mail);

                if (cliente != null)
                {
                    
                    if (cliente.pass == pass)
                    {
                        /* Esto es como la tarjeta*/
                        ClaimsIdentity identidad = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                        /* https://docs.microsoft.com/en-us/dotnet/api/system.security.claims.claimtypes?view=net-6.0*/
                        identidad.AddClaim(new Claim(ClaimTypes.Email, mail));
                        identidad.AddClaim(new Claim(ClaimTypes.GivenName, cliente.Nombre));
                        identidad.AddClaim(new Claim(ClaimTypes.Role, cliente.Rol.ToString()));
                        /*Le paso el id asi... no se si es correcto https://stackoverflow.com/questions/24892222/using-claims-types-properly-in-owin-identity-and-asp-net-mvc */
                        identidad.AddClaim(new Claim(ClaimTypes.NameIdentifier, cliente.id.ToString()));
                        
                        ClaimsPrincipal principal = new ClaimsPrincipal(identidad);

                        
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        if (!string.IsNullOrEmpty(UrlAEnviar))
                        {
                            return Redirect(UrlAEnviar);
                        }
                        else
                        {
                            // Redirigimos a la pagina principal
                            return RedirectToAction("Index", "Home");
                        }
                    }

                }

            }


            ViewBag.ErrorEnLogin = "Intente nuvamente. Mail y/o Contraseña erróneos";
            TempData["UrlAEnviar"] = UrlAEnviar; // Volvemos a enviarla en el TempData para no perderla
            return View();

        }



        // POST: Cliente/DarseBaja/5
        [Authorize(Roles = nameof(Rol.USUARIO))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DarseBaja()
        {

            var idCliente = User.FindFirstValue(ClaimTypes.NameIdentifier);

            /*No encontre otra manera de obtener el cliente, dado que el idCliente no es un int sino un string */
          var  cliente= await _context.Cliente.Where(s => s.id.ToString() == idCliente).FirstOrDefaultAsync();

            /*Desafortunadamente es la unica manera que encontre de vovler a cero los asientos */
          var  entradasModel = await _context.Entrada.Where(s => s.ClienteId.ToString() == idCliente).Include(entrada => entrada.Funcion.Pelicula)
                   .Include(entrada => entrada.Funcion.Asientos).ToListAsync();

            foreach(var ent in entradasModel)
            {
                ent.Asiento.ClienteId = 0;
            }


            _context.Cliente.Remove(cliente);
            await _context.SaveChangesAsync();

            /*Intente redirigir la accion pero exploto por lo que tuve que volver a copiar codigo */
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            /*Por algun motivo me indica que el viewbag es null, tuve que usar TempData */
            TempData["MensajeHome"] = "El usuario ha sido dado de baja ";
      
            return RedirectToAction("Index", "Home");


        }


        /* Preguntas porque no se puede ingresar a este metodo directamente a traves de la Url... Estimo que al ser un metodo post eso no es posible
         * dado que justamente la idea es que no sea visible.. a su vez entiendo que no puedo usar un anchor para llamarlo debiendo usar un form?
         es correcto*/

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Desloguearse()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

    }
}
