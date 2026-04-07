using System;
using System.Data.Entity;
using System.Linq;

namespace ClothingStoreNew
{
    public partial class ClothingStoreEntities2 : DbContext
    {
        public ClothingStoreEntities2()
            : base("name=ClothingStoreEntities2")
        {
        }

        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Products> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasKey(u => u.Id)
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Users>()
                .Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Users>()
                .Property(u => u.FullName)
                .HasMaxLength(100);

            modelBuilder.Entity<Products>()
                .HasKey(p => p.Id)
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Products>()
                .Property(p => p.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<Products>()
                .Property(p => p.Price)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }

    public partial class Users
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
    }

    public partial class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}