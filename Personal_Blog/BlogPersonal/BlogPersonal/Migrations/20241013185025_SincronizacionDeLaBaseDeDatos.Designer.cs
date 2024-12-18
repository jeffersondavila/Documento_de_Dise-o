﻿// <auto-generated />
using System;
using BlogPersonal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlogPersonal.Migrations
{
    [DbContext(typeof(PersonalBlogContext))]
    [Migration("20241013185025_SincronizacionDeLaBaseDeDatos")]
    partial class SincronizacionDeLaBaseDeDatos
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BlogPersonal.Models.Blog", b =>
                {
                    b.Property<int>("CodigoBlog")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodigoBlog"));

                    b.Property<int>("CodigoEstadoBlog")
                        .HasColumnType("int");

                    b.Property<int>("CodigoUsuario")
                        .HasColumnType("int");

                    b.Property<string>("Contenido")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("FechaCreacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smalldatetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<DateTime?>("FechaModificacion")
                        .HasColumnType("smalldatetime");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.HasKey("CodigoBlog")
                        .HasName("PK__Blog__87433BD2B95583CB");

                    b.HasIndex("CodigoEstadoBlog");

                    b.HasIndex("CodigoUsuario");

                    b.ToTable("Blog", (string)null);
                });

            modelBuilder.Entity("BlogPersonal.Models.EstadoBlog", b =>
                {
                    b.Property<int>("CodigoEstadoBlog")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodigoEstadoBlog"));

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("CodigoEstadoBlog")
                        .HasName("PK__EstadoBl__D825BC86D53230BF");

                    b.HasIndex(new[] { "Estado" }, "UQ__EstadoBl__36DF552FB3F3FB09")
                        .IsUnique();

                    b.ToTable("EstadoBlog", (string)null);
                });

            modelBuilder.Entity("BlogPersonal.Models.EstadoUsuario", b =>
                {
                    b.Property<int>("CodigoEstadoUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodigoEstadoUsuario"));

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("CodigoEstadoUsuario")
                        .HasName("PK__EstadoUs__293148A6FBAF246C");

                    b.HasIndex(new[] { "Estado" }, "UQ__EstadoUs__36DF552F959EB7B1")
                        .IsUnique();

                    b.ToTable("EstadoUsuario", (string)null);
                });

            modelBuilder.Entity("BlogPersonal.Models.Usuario", b =>
                {
                    b.Property<int>("CodigoUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodigoUsuario"));

                    b.Property<int>("CodigoEstadoUsuario")
                        .HasColumnType("int");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<DateTime?>("FechaUltimoAcceso")
                        .HasColumnType("smalldatetime");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("TokenRecuperacion")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.HasKey("CodigoUsuario")
                        .HasName("PK__Usuario__F0C18F582484D445");

                    b.HasIndex("CodigoEstadoUsuario");

                    b.HasIndex(new[] { "Correo" }, "UQ__Usuario__60695A19168961C8")
                        .IsUnique();

                    b.ToTable("Usuario", (string)null);
                });

            modelBuilder.Entity("BlogPersonal.Models.Blog", b =>
                {
                    b.HasOne("BlogPersonal.Models.EstadoBlog", "CodigoEstadoBlogNavigation")
                        .WithMany("Blogs")
                        .HasForeignKey("CodigoEstadoBlog")
                        .IsRequired()
                        .HasConstraintName("FK__Blog__CodigoEsta__300424B4");

                    b.HasOne("BlogPersonal.Models.Usuario", "CodigoUsuarioNavigation")
                        .WithMany("Blogs")
                        .HasForeignKey("CodigoUsuario")
                        .IsRequired()
                        .HasConstraintName("FK__Blog__CodigoUsua__2F10007B");

                    b.Navigation("CodigoEstadoBlogNavigation");

                    b.Navigation("CodigoUsuarioNavigation");
                });

            modelBuilder.Entity("BlogPersonal.Models.Usuario", b =>
                {
                    b.HasOne("BlogPersonal.Models.EstadoUsuario", "CodigoEstadoUsuarioNavigation")
                        .WithMany("Usuarios")
                        .HasForeignKey("CodigoEstadoUsuario")
                        .IsRequired()
                        .HasConstraintName("FK__Usuario__CodigoE__2B3F6F97");

                    b.Navigation("CodigoEstadoUsuarioNavigation");
                });

            modelBuilder.Entity("BlogPersonal.Models.EstadoBlog", b =>
                {
                    b.Navigation("Blogs");
                });

            modelBuilder.Entity("BlogPersonal.Models.EstadoUsuario", b =>
                {
                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("BlogPersonal.Models.Usuario", b =>
                {
                    b.Navigation("Blogs");
                });
#pragma warning restore 612, 618
        }
    }
}
