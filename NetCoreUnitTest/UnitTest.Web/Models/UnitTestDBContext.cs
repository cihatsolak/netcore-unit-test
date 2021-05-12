using Microsoft.EntityFrameworkCore;

#nullable disable

namespace UnitTest.Web.Models
{
    public partial class UnitTestDBContext : DbContext
    {
        public UnitTestDBContext()
        {
        }

        public UnitTestDBContext(DbContextOptions<UnitTestDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<School> Schools { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Turkish_CI_AS");

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.HasIndex(e => e.Name, "IX_Product_Name");

                entity.Property(e => e.Color).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Student>().HasKey(p => p.Id);
            modelBuilder.Entity<School>().HasKey(p => p.Id);
            modelBuilder.Entity<School>().HasMany(p => p.Students).WithOne(p => p.School).HasForeignKey(p => p.SchoolId).OnDelete(DeleteBehavior.Cascade);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

       
    }
}
