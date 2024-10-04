﻿using EFCore.ChangeTriggers.Extensions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

[assembly: DesignTimeServicesReference("EFCore.ChangeTriggers.ChangeTriggersDesignTimeServices, EFCore.ChangeTriggers")]
namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Persistence;

public class ChangeSourceEntityDbContext : DbContext
{
    public DbSet<ChangeSourceEntityUser> TestUsers { get; set; }

    public DbSet<ChangeSourceEntityUserChange> TestUserChanges { get; set; }

    public DbSet<ChangeSource> ChangeSources { get; set; }

    public ChangeSourceEntityDbContext(DbContextOptions<ChangeSourceEntityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChangeSourceEntityUserChange>(uc =>
        {
            uc.HasKey(uc => uc.ChangeId);
        });

        modelBuilder.Entity<ChangeSource>(cs =>
        {
            cs.HasData(new[]
            {
                new { Id = 1, Name = "Migrations"},
                new { Id = 2, Name = "Tests"},
                new { Id = 3, Name = "Web API"},
                new { Id = 4, Name = "Console"},
                new { Id = 5, Name = "SQL"},
                new { Id = 6, Name = "Mobile"},
                new { Id = 7, Name = "Public API"},
                new { Id = 8, Name = "Email Service"},
                new { Id = 9, Name = "Data Retention Service"},
                new { Id = 10, Name = "Maintenance"}
            });
        });

        modelBuilder.AutoConfigureChangeTriggers();
    }
}