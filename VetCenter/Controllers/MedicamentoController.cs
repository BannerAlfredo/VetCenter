using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetCenter.Data;
using VetCenter.Models;
using X.PagedList;

namespace VetCenter.Controllers
{
    public class MedicamentoController : Controller
    {
        private readonly AppDbContext _context;
        public MedicamentoController(AppDbContext context)
        {
            _context = context;
        }

        //Listar (Index)
        public IActionResult Index(int pageNumber = 1)
        {
            int pageSize = 5;
            var query = _context.Medicamentos.OrderBy(m => m.Id);

            var totalCount = query.Count();

            var medicamentos = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var pagedList = new StaticPagedList<Medicamento>(medicamentos, pageNumber, pageSize, totalCount);

            return View(pagedList);
        }

        //Detalles de un Medicamento
        public async Task<IActionResult> Detalles (int id)
        {
            var medicamento = await _context.Medicamentos.FirstOrDefaultAsync(m => m.Id == id);

            if (medicamento == null)  return NotFound();

            return View(medicamento);
        }

        //Crear Medicamento
        public IActionResult Nuevo()
        {
            var model = new Medicamento
            {
                NombreDelMedicamento = "",
                Descripcion = "",
                DosisRecomendada = "",
                FechaDeVencimiento = DateTime.Today,
                Stock = "0"
            };

            return View("MedicamentoForm", model);
        }

        //Guardar Medicamento (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guardar(Medicamento medicamento)
        {
            if (!ModelState.IsValid)
            {
                return View("MedicamentoForm", medicamento);
            }

            if (medicamento.Id == 0)
                _context.Medicamentos.Add(medicamento);
            else
                _context.Medicamentos.Update(medicamento);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Editar Medicamento
        public async Task<IActionResult> Editar(int id)
        {
            var medicamento = await _context.Medicamentos.FindAsync(id);
            if (medicamento == null) return NotFound();

            return View("MedicamentoForm", medicamento);
        }

        //Eliminar Medicamento
        public async Task<IActionResult> Eliminar(int id)
        {
            var medicamento = await _context.Medicamentos.FindAsync(id);
            if (medicamento != null)
            {
                _context.Medicamentos.Remove(medicamento);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
