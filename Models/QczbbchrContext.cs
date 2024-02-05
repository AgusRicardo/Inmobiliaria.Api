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
    public virtual DbSet<Estados> Estados { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Propietario>().ToTable("Propietarios");
        modelBuilder.Entity<Propiedad>().ToTable("Propiedades");
        modelBuilder.Entity<Inquilino>().ToTable("Inquilinos");
        modelBuilder.Entity<Contrato>().ToTable("Contratos");
        modelBuilder.Entity<Garante>().ToTable("Garantes");
        modelBuilder.Entity<Estados>().ToTable("Estados");
    }
}
