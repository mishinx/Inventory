using Microsoft.EntityFrameworkCore;

namespace Inventory_Context
{
    public class InventoryContext : DbContext
    {
        public DbSet<Administrator> administrators { get; set; }

        public DbSet<Warehouse> warehouses { get; set; }

        public DbSet<Goods> goods { get; set; }

        public DbSet<Operator> operators { get; set; }

        public string DbPath { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseNpgsql("Host=localhost;Port=5432;Database=inventarium;Username=postgres;Password=bochka2004;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrator>(entity =>
            {
                entity.HasKey(e => e.admin_id);
                entity.Property(e => e.company_name).IsRequired();
                entity.Property(e => e.email_address).IsRequired();
                entity.Property(e => e.admin_password).IsRequired();
                entity.Property(e => e.full_name).IsRequired();
                entity.Property(e => e.phone_number).IsRequired();
            });

            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.HasKey(e => e.warehouse_id);
                entity.Property(e => e.addres).IsRequired();
                entity.HasOne<Administrator>()
                    .WithMany()
                    .HasForeignKey(e => e.admin_id_ref)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Goods>(entity =>
            {
                entity.HasKey(e => e.goods_id);
                entity.Property(e => e.full_name).IsRequired();
                entity.Property(e => e.category).IsRequired();
                entity.Property(e => e.subcategory).IsRequired();
                entity.Property(e => e.short_description).IsRequired();
                entity.Property(e => e.quantity).IsRequired();
                entity.Property(e => e.price).HasColumnType("numeric(10, 2)").IsRequired();
                entity.HasOne<Warehouse>()
                    .WithMany()
                    .HasForeignKey(e => e.warehouse_id_ref)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Operator>(entity =>
            {
                entity.HasKey(e => e.operator_id);
                entity.Property(e => e.email_address).IsRequired();
                entity.Property(e => e.operator_password).IsRequired();
                entity.Property(e => e.full_name).IsRequired();
                entity.Property(e => e.phone_number).IsRequired();
                entity.HasOne<Warehouse>()
                    .WithMany()
                    .HasForeignKey(e => e.warehouse_id_ref)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne<Administrator>()
                    .WithMany()
                    .HasForeignKey(e => e.admin_id_ref)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }
    }

    public interface IEmployee
    { }

    public class Administrator : IEmployee
    {
        public int admin_id { get; set; }
        public string company_name { get; set; }
        public string email_address { get; set; }
        public string admin_password { get; set; }
        public string full_name { get; set; }
        public string phone_number { get; set; }
    }

    public partial class Warehouse
    {
        public int warehouse_id { get; set; }
        public string addres { get; set; }
        public int admin_id_ref { get; set; }
    }
    public partial class Goods
    {
        public int goods_id { get; set; }
        public string full_name { get; set; }
        public string category { get; set; }
        public string subcategory { get; set; }
        public string short_description { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public int warehouse_id_ref { get; set; }
        public byte[] photo { get; set; }
    }

    public partial class Operator : IEmployee
    {
        public int operator_id { get; set; }
        public string email_address { get; set; }
        public string operator_password { get; set; }
        public string full_name { get; set; }
        public string phone_number { get; set; }
        public int warehouse_id_ref { get; set; }
        public int admin_id_ref { get; set; }
        public byte[] photo { get; set; }
    }
}