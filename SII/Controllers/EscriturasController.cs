using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SII.models;


namespace SII.Controllers
{
    public class EscriturasController : Controller
    {
        private readonly EscriturasContext _context;

        public EscriturasController(EscriturasContext context)
        {
            _context = context;
        }

        // GET: Escrituras
        public async Task<IActionResult> Index()
        {
            return _context.Escrituras != null ?
                        View(await _context.Escrituras.ToListAsync()) :
                        Problem("Entity set 'EscriturasContext.Escrituras'  is null.");
        }

        // GET: Escrituras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Escrituras == null)
            {
                return NotFound();
            }

            var escritura = await _context.Escrituras
                .FirstOrDefaultAsync(m => m.NumAtencion == id);
            if (escritura == null)
            {
                return NotFound();
            }

            return View(escritura);
        }

        // GET: Escrituras/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Escrituras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cne, Comuna, Predio, Manzana, NumeroInscripcion, FechaEscritura, Protocolo, Notario, Direccion, TipoDocumento")] Escritura escritura, List<Enajenante> enajenantes, List<Adquirente> adquirentes)
        {

            try
            {
                Console.WriteLine("Entro al try");
                Console.WriteLine(ModelState.IsValid);
                if (ModelState.IsValid)
                {
                    Console.WriteLine("Entro al if");
                    // guarda la escritura en la base de datos
                    _context.Escrituras.Add(escritura);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("guardo escritura");
                    // guarda los enajenantes asociados a la escritura en la base de datos
                    Console.WriteLine("imprimiendo enajenates");
                    Console.WriteLine(enajenantes);
                    foreach (Enajenante enajenante in enajenantes)
                    {
                        enajenante.NumAtencion = escritura.NumAtencion; // asigna el id de la escritura al enajenante
                        _context.Enajenantes.Add(enajenante);
                        Console.WriteLine("Guardo enajenante");
                    }


                    // guarda los adquirentes asociados a la escritura en la base de datos
                    foreach (Adquirente adquirente in adquirentes)
                    {
                        adquirente.NumAtencion = escritura.NumAtencion; // asigna el id de la escritura al adquirente
                        _context.Adquirentes.Add(adquirente);
                    }

                    await _context.SaveChangesAsync();  // guarda los cambios en la base de datos

                    return RedirectToAction("Index", "Home"); // redirige a la página principal
                }
            }
            catch (DbUpdateException)
            {
                Console.WriteLine("Paso a error");
                // Log the error (uncomment ex variable name and write a log.)
                ModelState.AddModelError("", "No se pudo guardar los cambios. " +
            "Intente de nuevo, y si el problema persiste " +
            "consulte al administrador del sistema.");
            }

            return View(escritura);
        }
        // GET: Escrituras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Escrituras == null)
            {
                return NotFound();
            }

            var escritura = await _context.Escrituras.FindAsync(id);
            if (escritura == null)
            {
                return NotFound();
            }
            return View(escritura);
        }

        // POST: Escrituras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NumAtencion,Cne,Comuna,Manzana,Predio,Fojas,FechaInscripcion,NumeroInscripcion")] Escritura escritura)
        {
            if (id != escritura.NumAtencion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(escritura);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EscrituraExists(escritura.NumAtencion))
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
            return View(escritura);
        }

        // GET: Escrituras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Escrituras == null)
            {
                return NotFound();
            }

            var escritura = await _context.Escrituras
                .FirstOrDefaultAsync(m => m.NumAtencion == id);
            if (escritura == null)
            {
                return NotFound();
            }

            return View(escritura);
        }

        // POST: Escrituras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Escrituras == null)
            {
                return Problem("Entity set 'EscriturasContext.Escrituras'  is null.");
            }
            var escritura = await _context.Escrituras.FindAsync(id);
            if (escritura != null)
            {
                _context.Escrituras.Remove(escritura);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EscrituraExists(int id)
        {
            return (_context.Escrituras?.Any(e => e.NumAtencion == id)).GetValueOrDefault();
        }
    }
}
