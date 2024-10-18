﻿using EFCore.ChangeTriggers.Extensions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

[assembly: DesignTimeServicesReference("EFCore.ChangeTriggers.ChangeTriggersDesignTimeServices, EFCore.ChangeTriggers")]
namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Persistence;

public class ChangedByScalarDbContext : DbContext
{
    public DbSet<ChangedByScalarUser> TestUsers { get; set; }

    public DbSet<ChangedByScalarUserChange> TestUserChanges { get; set; }

    public ChangedByScalarDbContext(DbContextOptions<ChangedByScalarDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AutoConfigureChangeTriggers();

        modelBuilder.Entity<ChangedByScalarUser>(u =>
        {
            u.Property(u => u.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ChangedByScalarUserChange>(uc =>
        {
            uc.HasKey(uc => uc.ChangeId);
        });
    }
}
