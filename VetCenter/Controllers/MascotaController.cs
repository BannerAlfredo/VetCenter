using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetCenter.Data;
using VetCenter.Models;
using X.PagedList;
using X.PagedList.Extensions;

namespace VetCenter.Controllers
{
    public class MascotaController : Controller
    {
        private readonly AppDbContext _context;

        public MascotaController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int pageNumber = 1)
        {
            int pageSize = 5;

            var query = _context.Mascotas
                .Include(m => m.Cliente)
                .OrderBy(m => m.Id);

            var totalCount = query.Count();

            var mascotas = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var pagedList = new StaticPagedList<Mascota>(mascotas, pageNumber, pageSize, totalCount);

            return View(pagedList);
        }


        // GET: Mascotas/Detalles/5
        public async Task<IActionResult> Detalles(int id)
        {
            var mascota = await _context.Mascotas
                .Include(m => m.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mascota == null) return NotFound();

            return View(mascota);
        }

        // GET: Mascotas/Nuevo
        public IActionResult Nuevo()
        {
            ViewData["Clientes"] = new SelectList(_context.Clientes, "Id", "Nombre");
            return View("MascotaForm", new Mascota { Estado = true });
        }

        // POST: Mascotas/Guardar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guardar(Mascota mascota)
        {
            if (mascota.Id == 0)
                _context.Mascotas.Add(mascota);
            else
                _context.Mascotas.Update(mascota);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Mascotas/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            var mascota = await _context.Mascotas.FindAsync(id);
            if (mascota == null) return NotFound();

            ViewData["Clientes"] = new SelectList(_context.Clientes, "Id", "Nombre", mascota.ClienteId);
            return View("MascotaForm", mascota);
        }

        // GET: Mascotas/Eliminar/5
        public async Task<IActionResult> Eliminar(int id)
        {
            var mascota = await _context.Mascotas.FindAsync(id);
            if (mascota != null)
            {
                _context.Mascotas.Remove(mascota);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
