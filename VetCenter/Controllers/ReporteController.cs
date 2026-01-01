using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetCenter.Data;
using VetCenter.Models;

namespace VetCenter.Controllers
{
    public class ReporteController : Controller
    {
        private readonly AppDbContext _context;

        public ReporteController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Reporte
        // Muestra el formulario de selección (Tu imagen 2)
        public IActionResult Index()
        {
            // Cargamos las mascotas para el Dropdown
            // El formato "Nombre (Tipo)" lo podemos hacer en la vista o proyectarlo aquí
            var mascotas = _context.Mascotas.Select(m => new {
                Id = m.Id,
                Texto = m.Nombre + " (" + m.Tipo + ")"
            }).ToList();

            ViewData["Mascotas"] = new SelectList(mascotas, "Id", "Texto");
            return View();
        }

        // GET: /Reporte/CitasPorMascota
        // Aquí llegarás cuando le des clic a "Ver Citas"
        [HttpGet]
        public async Task<IActionResult> CitasPorMascota(int mascotaId)
        {
            var citas = await _context.Citas
                .Include(c => c.Mascota)
                .Where(c => c.MascotaId == mascotaId)
                .OrderByDescending(c => c.Fecha)
                .ToListAsync();

            // Recargamos la lista por si quiere cambiar de mascota en la misma pantalla
            var mascotas = _context.Mascotas.Select(m => new {
                Id = m.Id,
                Texto = m.Nombre + " (" + m.Tipo + ")"
            }).ToList();

            ViewData["Mascotas"] = new SelectList(mascotas, "Id", "Texto", mascotaId);
            ViewData["SelectedMascotaId"] = mascotaId; // Para saber qué mascota se eligió

            return View("Index", citas); // Reutilizamos la vista Index pero enviando datos
        }
    }
}