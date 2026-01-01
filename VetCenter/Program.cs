using VetCenter.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Configurar conexión a la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 34)) // Ajusta la versión si es necesario
    )
);

// 🔹 Registrar servicios
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // habilitar sesiones
builder.Services.AddHttpContextAccessor(); // para leer sesión en _Layout.cshtml

var app = builder.Build();

// 🔹 Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();   // ⚠️ Aquí se cargan los assets, revisa duplicados en wwwroot

app.UseRouting();

app.UseSession();       // usar sesiones aquí
app.UseAuthorization();

// 🔹 Ruta por defecto: ir al login
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cuenta}/{action=Login}/{id?}");

app.Run();