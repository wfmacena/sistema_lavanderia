using Microsoft.EntityFrameworkCore;
using SistemaLavanderia.Data;
using SistemaLavanderia.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<AuthFilter>();
});

builder.Services.AddDbContext<LavanderiaContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

    if (!string.IsNullOrEmpty(databaseUrl))
    {
        // Se houver DATABASE_URL (Render), usa PostgreSQL
        options.UseNpgsql(ParseDatabaseUrl(databaseUrl));
    }
    else
    {
        // Caso contrário, usa SQLite local
        options.UseSqlite(connectionString);
    }
});

// Helper para converter DATABASE_URL do Render para formato Npgsql
string ParseDatabaseUrl(string url)
{
    var uri = new Uri(url);
    var db = uri.AbsolutePath.Trim('/');
    var user = uri.UserInfo.Split(':')[0];
    var passwd = uri.UserInfo.Split(':')[1];
    var port = uri.Port > 0 ? uri.Port : 5432;
    var connStr = $"Server={uri.Host};Database={db};User Id={user};Password={passwd};Port={port};SSL Mode=Require;Trust Server Certificate=True;";
    return connStr;
}

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
            Nome = "Usu�rio Comum",
            Login = "usuario",
            Senha = "1234",
            Perfil = "Usuario"
        });

        context.SaveChanges();
    }

    if (!context.Servicos.Any())
    {
        context.Servicos.AddRange(new List<SistemaLavanderia.Models.Servico>
        {
            new SistemaLavanderia.Models.Servico { Nome = "Camisa", Descricao = "Lavagem e passadoria de camisa social", PrecoBase = 12.00m, UnidadeMedida = "Peça" },
            new SistemaLavanderia.Models.Servico { Nome = "Calça", Descricao = "Lavagem e passadoria de calça jeans ou sarja", PrecoBase = 15.00m, UnidadeMedida = "Peça" },
            new SistemaLavanderia.Models.Servico { Nome = "Vestido", Descricao = "Lavagem de vestido simples", PrecoBase = 25.00m, UnidadeMedida = "Peça" },
            new SistemaLavanderia.Models.Servico { Nome = "Terno", Descricao = "Lavagem a seco de terno completo (Paletó + Calça)", PrecoBase = 45.00m, UnidadeMedida = "Peça" },
            new SistemaLavanderia.Models.Servico { Nome = "Edredom Casal", Descricao = "Lavagem profunda de edredom de casal", PrecoBase = 35.00m, UnidadeMedida = "Peça" },
            new SistemaLavanderia.Models.Servico { Nome = "Lavagem por Quilo", Descricao = "Roupas do dia a dia (mínimo 5kg)", PrecoBase = 15.00m, UnidadeMedida = "Kg" },
            new SistemaLavanderia.Models.Servico { Nome = "Tênis", Descricao = "Lavagem e higienização de calçados esportivos", PrecoBase = 20.00m, UnidadeMedida = "Par" },
            new SistemaLavanderia.Models.Servico { Nome = "Cortina", Descricao = "Lavagem de cortina (preço por metro quadrado)", PrecoBase = 18.00m, UnidadeMedida = "M²" }
        });
        context.SaveChanges();
    }
}

app.Run();