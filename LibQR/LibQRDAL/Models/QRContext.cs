using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LibQRDAL.Models
{
    public partial class QRContext : DbContext
    {
        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<SimpleUser> SimpleUser { get; set; }
        public QRContext(DbContextOptions<QRContext> options)
    : base(options)
        { }
//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer(@"Server=.;Database=QR;Trusted_Connection=True;");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Idbook);

                entity.Property(e => e.Idbook).HasColumnName("IDBook");

                entity.Property(e => e.Creator)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.ErrorMessage)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Identifier)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.IdtinRead)
                    .IsRequired()
                    .HasColumnName("IDTinRead")
                    .HasMaxLength(50);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<SimpleUser>(entity =>
            {
                entity.HasKey(e => e.Iduser);

                entity.Property(e => e.Iduser).HasColumnName("IDUser");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(150);
            });
        }
    }
}
