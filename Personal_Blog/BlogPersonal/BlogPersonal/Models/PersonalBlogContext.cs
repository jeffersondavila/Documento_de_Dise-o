using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlogPersonal.Models;

public partial class PersonalBlogContext : DbContext
{
    public PersonalBlogContext()
    {
    }

    public PersonalBlogContext(DbContextOptions<PersonalBlogContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<EstadoBlog> EstadoBlogs { get; set; }

    public virtual DbSet<EstadoUsuario> EstadoUsuarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-07NSNMOC;Initial Catalog=PersonalBlog;user id=sa;password=loc@del@rea;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.CodigoBlog).HasName("PK__Blog__87433BD2B95583CB");

            entity.ToTable("Blog");

            entity.Property(e => e.Contenido).HasColumnType("text");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("smalldatetime");
            entity.Property(e => e.Titulo)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.CodigoEstadoBlogNavigation).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.CodigoEstadoBlog)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Blog__CodigoEsta__300424B4");

            entity.HasOne(d => d.CodigoUsuarioNavigation).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.CodigoUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Blog__CodigoUsua__2F10007B");
        });

        modelBuilder.Entity<EstadoBlog>(entity =>
        {
            entity.HasKey(e => e.CodigoEstadoBlog).HasName("PK__EstadoBl__D825BC86D53230BF");

            entity.ToTable("EstadoBlog");

            entity.HasIndex(e => e.Estado, "UQ__EstadoBl__36DF552FB3F3FB09").IsUnique();

            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EstadoUsuario>(entity =>
        {
            entity.HasKey(e => e.CodigoEstadoUsuario).HasName("PK__EstadoUs__293148A6FBAF246C");

            entity.ToTable("EstadoUsuario");

            entity.HasIndex(e => e.Estado, "UQ__EstadoUs__36DF552F959EB7B1").IsUnique();

            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.CodigoUsuario).HasName("PK__Usuario__F0C18F582484D445");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Correo, "UQ__Usuario__60695A19168961C8").IsUnique();

            entity.Property(e => e.Correo)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FechaUltimoAcceso).HasColumnType("smalldatetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TokenRecuperacion)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.CodigoEstadoUsuarioNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.CodigoEstadoUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario__CodigoE__2B3F6F97");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
