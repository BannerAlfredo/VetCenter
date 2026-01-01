using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetCenter.Data;
using VetCenter.Models;
using X.PagedList;
using X.PagedList.Extensions;

namespace VetCenter.Controllers
{
    public class CitaController : Controller
    {
        private readonly AppDbContext _context;

        public CitaController(AppDbContext context)
        {
            _context = context;
        }

        // LISTAR (Index) con paginación
        public IActionResult Index(int pageNumber = 1)
        {
            int pageSize = 5; // ✅ queremos 5 registros por página

            var citas = _context.Citas
                .Include(c => c.Mascota)
                .ThenInclude(m => m.Cliente)
                .OrderBy(c => c.Id)
                .ToPagedList(pageNumber, pageSize); // ✅ aquí se limita a 5

            return View(citas);
        }

        // NUEVO
        public IActionResult Nuevo()
        {
            ViewData["Mascotas"] = new SelectList(_context.Mascotas, "Id", "Nombre");

            return View("CitaForm", new Cita
            {
                Fecha = DateTime.Now,
                Hora = DateTime.Now.TimeOfDay
            });
        }

        // GUARDAR (Create/Update)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guardar(Cita cita)
        {
            if (ModelState.IsValid)
            {
                if (cita.Id == 0)
                    _context.Citas.Add(cita);
                else
                    _context.Citas.Update(cita);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Mascotas"] = new SelectList(_context.Mascotas, "Id", "Nombre", cita.MascotaId);
            return View("CitaForm", cita);
        }

        // DETALLES
        public async Task<IActionResult> Detalles(int id)
        {
            var cita = await _context.Citas
                .Include(c => c.Mascota)
                .ThenInclude(m => m.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cita == null) return NotFound();

            return View(cita);
        }

        // EDITAR
        public async Task<IActionResult> Editar(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null) return NotFound();

            ViewData["Mascotas"] = new SelectList(_context.Mascotas, "Id", "Nombre", cita.MascotaId);

            return View("CitaForm", cita);
        }

        // ELIMINAR
        public async Task<IActionResult> Eliminar(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita != null)
            {
                _context.Citas.Remove(cita);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
