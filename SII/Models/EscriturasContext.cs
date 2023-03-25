using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SII.models;

public partial class EscriturasContext : DbContext
{
    public EscriturasContext()
    {
    }

    public EscriturasContext(DbContextOptions<EscriturasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Adquirente> Adquirentes { get; set; }

    public virtual DbSet<Enajenante> Enajenantes { get; set; }

    public virtual DbSet<Escritura> Escrituras { get; set; }

    public virtual DbSet<Multipropietario> Multipropietarios { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Adquirente>(entity =>
        {
            entity.HasKey(e => e.IdAdquirente).HasName("PK__Adquiren__AAEC4FDB48E843BF");

            entity.ToTable("Adquirente");

            entity.Property(e => e.RunRut)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RUN_RUT");

            entity.HasOne(d => d.NumAtencionNavigation).WithMany(p => p.Adquirentes)
                .HasForeignKey(d => d.NumAtencion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Adquirent__NumAt__3C69FB99");
        });

        modelBuilder.Entity<Enajenante>(entity =>
        {
            entity.HasKey(e => e.IdEnajenante).HasName("PK__Enajenan__2664FAC4DDAF1CA5");

            entity.ToTable("Enajenante");

            entity.Property(e => e.RunRut)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RUN_RUT");

            entity.HasOne(d => d.NumAtencionNavigation).WithMany(p => p.Enajenantes)
                .HasForeignKey(d => d.NumAtencion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Enajenant__NumAt__398D8EEE");
        });

        modelBuilder.Entity<Escritura>(entity =>
        {
            entity.HasKey(e => e.NumAtencion).HasName("PK__Escritur__C0692259767AB5C6");

            entity.ToTable("Escritura");

            entity.Property(e => e.Cne)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CNE");
            entity.Property(e => e.Comuna)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaInscripcion).HasColumnType("date");
            entity.Property(e => e.Manzana)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NumeroInscripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Predio)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Multipropietario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MULTIPRO__3214EC07275953BA");

            entity.ToTable("MULTIPROPIETARIO");

            entity.Property(e => e.Comuna)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaInscripcion).HasColumnType("date");
            entity.Property(e => e.RunRut)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("RUN_RUT");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
