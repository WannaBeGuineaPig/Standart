using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Shestorka_API.Models;

public partial class ShesterochkaContext : DbContext
{
    public ShesterochkaContext()
    {
    }

    public ShesterochkaContext(DbContextOptions<ShesterochkaContext> options)
        : base(options)
    {
    }

    private static ShesterochkaContext _context;
    public static ShesterochkaContext Context
    {
        get
        {
            if (_context == null)
                _context = new ShesterochkaContext();
            return _context;
        }
    }

    public virtual DbSet<CategoryEat> CategoryEats { get; set; }

    public virtual DbSet<Eat> Eats { get; set; }

    public virtual DbSet<Manafacturer> Manafacturers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<PickupPoint> PickupPoints { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<TypeEat> TypeEats { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=root;database=shesterochka", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.39-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<CategoryEat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("category_eat");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Category)
                .HasMaxLength(100)
                .HasColumnName("category");
        });

        modelBuilder.Entity<Eat>(entity =>
        {
            entity.HasKey(e => e.Articul).HasName("PRIMARY");

            entity.ToTable("eat");

            entity.HasIndex(e => e.IdCategory, "id_category_idx");

            entity.HasIndex(e => e.IdManafacturer, "id_manafacturer_idx");

            entity.HasIndex(e => e.IdSupplier, "id_supplier_idx");

            entity.HasIndex(e => e.IdType, "id_type_idx");

            entity.Property(e => e.Articul)
                .HasMaxLength(45)
                .HasColumnName("articul");
            entity.Property(e => e.AmountInStorage).HasColumnName("amount_in_storage");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DiscountPercent).HasColumnName("discount_percent");
            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.IdManafacturer).HasColumnName("id_manafacturer");
            entity.Property(e => e.IdSupplier).HasColumnName("id_supplier");
            entity.Property(e => e.IdType).HasColumnName("id_type");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.UnitOfMeasurement)
                .HasColumnType("enum('шт.')")
                .HasColumnName("unit_of_measurement");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Eats)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_category");

            entity.HasOne(d => d.IdManafacturerNavigation).WithMany(p => p.Eats)
                .HasForeignKey(d => d.IdManafacturer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_manafacturer");

            entity.HasOne(d => d.IdSupplierNavigation).WithMany(p => p.Eats)
                .HasForeignKey(d => d.IdSupplier)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_supplier");

            entity.HasOne(d => d.IdTypeNavigation).WithMany(p => p.Eats)
                .HasForeignKey(d => d.IdType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_type");
        });

        modelBuilder.Entity<Manafacturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("manafacturer");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Manafacturer1)
                .HasMaxLength(100)
                .HasColumnName("manafacturer");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("order");

            entity.HasIndex(e => e.IdPickupPoint, "id_pickup_point_idx");

            entity.HasIndex(e => e.IdUser, "id_user_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CodePickup).HasColumnName("code_pickup");
            entity.Property(e => e.DateDelivery).HasColumnName("date_delivery");
            entity.Property(e => e.DateOrdering).HasColumnName("date_ordering");
            entity.Property(e => e.IdPickupPoint).HasColumnName("id_pickup_point");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Status)
                .HasColumnType("enum('Новый','Завершен')")
                .HasColumnName("status");

            entity.HasOne(d => d.IdPickupPointNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdPickupPoint)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_pickup_point");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_user");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => new { e.IdOrder, e.ArticulItem })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("order_item");

            entity.HasIndex(e => e.ArticulItem, "articul_item_idx");

            entity.Property(e => e.IdOrder)
                .ValueGeneratedOnAdd()
                .HasColumnName("id_order");
            entity.Property(e => e.ArticulItem)
                .HasMaxLength(100)
                .HasColumnName("articul_item");
            entity.Property(e => e.AmountItem).HasColumnName("amount_item");

            entity.HasOne(d => d.ArticulItemNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ArticulItem)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("articul_item");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_order");
        });

        modelBuilder.Entity<PickupPoint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pickup_point");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("supplier");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Supplier1)
                .HasMaxLength(100)
                .HasColumnName("supplier");
        });

        modelBuilder.Entity<TypeEat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("type_eat");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .HasColumnName("type");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.Mail)
                .HasMaxLength(255)
                .HasColumnName("mail");
            entity.Property(e => e.MidleName)
                .HasMaxLength(100)
                .HasColumnName("midle_name");
            entity.Property(e => e.Password)
                .HasMaxLength(45)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasColumnType("enum('Администратор','Менеджер','Авторизированный клиент')")
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
