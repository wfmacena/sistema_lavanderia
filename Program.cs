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
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".SistemaLavanderia.Session";
});
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
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<LavanderiaContext>();

    try 
    {
        // Garante que o banco existe e as tabelas estão criadas via migrações
        context.Database.Migrate();
        Console.WriteLine("Banco de dados migrado com sucesso.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro na migração: {ex.Message}");
        // Se a migração falhar, tenta garantir que o banco pelo menos exista
        try {
            if (context.Database.EnsureCreated()) {
                Console.WriteLine("Banco de dados criado via EnsureCreated (Migração falhou).");
            }
        } catch (Exception ex2) {
            Console.WriteLine($"Erro ao criar banco via EnsureCreated: {ex2.Message}");
        }
    }

    try 
    {
        // Verifica se existem usuários, se não, adiciona os padrões
        if (!context.Usuarios.Any())
        {
            Console.WriteLine("Semeando usuários padrões...");
            context.Usuarios.Add(new SistemaLavanderia.Models.Usuario
            {
                Nome = "Administrador",
                Login = "admin",
                Senha = "1234",
                Perfil = "Administrador",
                Cpf = "000.000.000-00",
                Email = "admin@lavanderia.com",
                Telefone = "11999999999"
            });

            context.Usuarios.Add(new SistemaLavanderia.Models.Usuario
            {
                Nome = "Usuário Comum",
                Login = "usuario",
                Senha = "1234",
                Perfil = "Usuario",
                Cpf = "111.111.111-11",
                Email = "usuario@lavanderia.com",
                Telefone = "11888888888"
            });

            context.SaveChanges();
            Console.WriteLine("Usuários semeados com sucesso.");
        }

        if (!context.Servicos.Any())
        {
            Console.WriteLine("Semeando serviços padrões...");
            context.Servicos.AddRange(new List<SistemaLavanderia.Models.Servico>
            {
                new SistemaLavanderia.Models.Servico { Nome = "Camisa", Descricao = "Lavagem e passadoria de camisa social", PrecoBase = 12.00m, UnidadeMedida = "Peça", Ativo = true },
                new SistemaLavanderia.Models.Servico { Nome = "Calça", Descricao = "Lavagem e passadoria de calça jeans ou sarja", PrecoBase = 15.00m, UnidadeMedida = "Peça", Ativo = true },
                new SistemaLavanderia.Models.Servico { Nome = "Vestido", Descricao = "Lavagem de vestido simples", PrecoBase = 25.00m, UnidadeMedida = "Peça", Ativo = true },
                new SistemaLavanderia.Models.Servico { Nome = "Terno", Descricao = "Lavagem a seco de terno completo (Paletó + Calça)", PrecoBase = 45.00m, UnidadeMedida = "Peça", Ativo = true },
                new SistemaLavanderia.Models.Servico { Nome = "Edredom Casal", Descricao = "Lavagem profunda de edredom de casal", PrecoBase = 35.00m, UnidadeMedida = "Peça", Ativo = true },
                new SistemaLavanderia.Models.Servico { Nome = "Lavagem por Quilo", Descricao = "Roupas do dia a dia (mínimo 5kg)", PrecoBase = 15.00m, UnidadeMedida = "Kg", Ativo = true },
                new SistemaLavanderia.Models.Servico { Nome = "Tênis", Descricao = "Lavagem e higienização de calçados esportivos", PrecoBase = 20.00m, UnidadeMedida = "Par", Ativo = true },
                new SistemaLavanderia.Models.Servico { Nome = "Cortina", Descricao = "Lavagem de cortina (preço por metro quadrado)", PrecoBase = 18.00m, UnidadeMedida = "M²", Ativo = true }
            });
            context.SaveChanges();
            Console.WriteLine("Serviços semeados com sucesso.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro no Seed: {ex.Message}");
    }
}

app.Run();