using Microsoft.AspNetCore.Mvc;
using VetCenter.Models;
using VetCenter.Data;

namespace VetCenter.Controllers
{
    public class PanelController : Controller
    {
        private readonly AppDbContext _context;

        public PanelController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var usuario = HttpContext.Session.GetString("usuario");
            if (usuario == null)
            {
                return RedirectToAction("Login", "Cuenta");
            }

            var model = new PanelViewModel
            {
                Usuario = usuario,
                Citas = _context.Citas.Count(),
                Mascotas = _context.Mascotas.Count(),
                Clientes = _context.Clientes.Count(),
                Medicamentos = _context.Medicamentos.Count()
            };

            return View(model);
        }
    }
}
