using Microsoft.EntityFrameworkCore;
using SistemaLavanderia.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<LavanderiaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();      // TEM QUE VIR ANTES
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Welcome}/{id?}");

// Seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LavanderiaContext>();

    context.Database.Migrate();

    if (!context.Usuarios.Any())
    {
        context.Usuarios.Add(new SistemaLavanderia.Models.Usuario
        {
            Nome = "Administrador",
            Login = "admin",
            Senha = "1234",
            Perfil = "Administrador"
        });

        context.Usuarios.Add(new SistemaLavanderia.Models.Usuario
        {
            Nome = "Usuário Comum",
            Login = "usuario",
            Senha = "1234",
            Perfil = "Usuario"
        });

        context.SaveChanges();
    }
}

app.Run();