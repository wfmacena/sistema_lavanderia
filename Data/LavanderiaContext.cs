using Microsoft.EntityFrameworkCore;
using SistemaLavanderia.Models;

namespace SistemaLavanderia.Data
{
    public class LavanderiaContext : DbContext
    {
        public LavanderiaContext(DbContextOptions<LavanderiaContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }
        public DbSet<Servico> Servicos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração para PostgreSQL usar Identity nas chaves primárias
            if (Database.IsNpgsql())
            {
                modelBuilder.UseIdentityByDefaultColumns();
            }

            // Mapeamento explícito para PostgreSQL lidar com tipos numéricos e booleanos
            modelBuilder.Entity<Servico>(entity =>
            {
                entity.Property(e => e.PrecoBase).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Ativo).HasColumnType("boolean");
            });

            modelBuilder.Entity<ItemPedido>(entity =>
            {
                entity.Property(e => e.ValorUnitario).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Subtotal).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.Property(e => e.Valor).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany()
                .HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Cliente)
                .WithMany()
                .HasForeignKey(u => u.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ItemPedido>()
                .HasOne(i => i.Pedido)
                .WithMany(p => p.ItensPedido)
                .HasForeignKey(i => i.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}