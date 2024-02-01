using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Inmobiliaria.Models;

public partial class QczbbchrContext : DbContext
{
    private readonly IConfiguration _configuration;
    public QczbbchrContext()
    {
    }

    public QczbbchrContext(DbContextOptions<QczbbchrContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<Propietario> Propietarios { get; set; }
    public virtual DbSet<Propiedad> Propiedades { get; set; }
    public virtual DbSet<Inquilino> Inquilinos { get; set; }
    public virtual DbSet<Contrato> Contratos { get; set; }
    public virtual DbSet<Garante> Garantes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Propietario>().ToTable("Propietarios");
        modelBuilder.Entity<Propiedad>().ToTable("Propiedades");
        modelBuilder.Entity<Inquilino>().ToTable("Inquilinos");
        modelBuilder.Entity<Contrato>().ToTable("Contratos");
        modelBuilder.Entity<Garante>().ToTable("Garantes");
    }
}
/*
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("btree_gin")
            .HasPostgresExtension("btree_gist")
            .HasPostgresExtension("citext")
            .HasPostgresExtension("cube")
            .HasPostgresExtension("dblink")
            .HasPostgresExtension("dict_int")
            .HasPostgresExtension("dict_xsyn")
            .HasPostgresExtension("earthdistance")
            .HasPostgresExtension("fuzzystrmatch")
            .HasPostgresExtension("hstore")
            .HasPostgresExtension("intarray")
            .HasPostgresExtension("ltree")
            .HasPostgresExtension("pg_stat_statements")
            .HasPostgresExtension("pg_trgm")
            .HasPostgresExtension("pgcrypto")
            .HasPostgresExtension("pgrowlocks")
            .HasPostgresExtension("pgstattuple")
            .HasPostgresExtension("tablefunc")
            .HasPostgresExtension("unaccent")
            .HasPostgresExtension("uuid-ossp")
            .HasPostgresExtension("xml2");

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.PersonaId).HasName("personas_pkey");

            entity.ToTable("personas");

            entity.Property(e => e.PersonaId).HasColumnName("persona_id");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .HasColumnName("apellido");
            entity.Property(e => e.Dni)
                .HasMaxLength(20)
                .HasColumnName("dni");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.User_id).HasName("usuarios_pkey");

            entity.ToTable("usuarios");

            entity.Property(e => e.User_id).HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .HasColumnName("rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
*/