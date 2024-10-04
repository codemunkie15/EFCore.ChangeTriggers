using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations.Domain;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations.Infrastructure;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations;

public class MigrationsTests : IClassFixture<MigrationsFixture>, IAsyncLifetime
{
    private readonly MigrationsFixture fixture;

    public MigrationsTests(MigrationsFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void MigrateDatabase_SetsCorrectChangeContext()
    {
        var dbContext = fixture.Services.GetRequiredService<MigrationsDbContext>();
        var changeSourceProvider = fixture.Services.GetRequiredService<MigrationsCurrentChangeSourceProvider>();
        var currentUserProvider = fixture.Services.GetRequiredService<MigrationsCurrentUserProvider>();

        var currentChangeSource = ChangeSource.Migrations;
        var currentUser = new MigrationsUser { Id = 1 };

        changeSourceProvider.CurrentChangeSource = currentChangeSource;
        currentUserProvider.CurrentUser = currentUser;

        dbContext.Database.Migrate();

        var users = dbContext.TestUsers.Include(u => u.Changes).ThenInclude(c => c.ChangedBy).ToList();

        Assert.True(users.All(u => u.Changes.All(c => c.ChangeSource == currentChangeSource)));
        Assert.True(users.All(u => u.Changes.All(c => c.ChangedBy.Id == currentUser.Id)));
    }

    [Fact]
    public async Task MigrateDatabase_SetsCorrectChangeContext_Async()
    {
        var dbContext = fixture.Services.GetRequiredService<MigrationsDbContext>();
        var changeSourceProvider = fixture.Services.GetRequiredService<MigrationsCurrentChangeSourceProvider>();
        var currentUserProvider = fixture.Services.GetRequiredService<MigrationsCurrentUserProvider>();

        var currentChangeSource = ChangeSource.Migrations;
        var currentUser = new MigrationsUser { Id = 1 };

        changeSourceProvider.CurrentChangeSource = currentChangeSource;
        currentUserProvider.CurrentUser = currentUser;

        await dbContext.Database.MigrateAsync();

        var users = await dbContext.TestUsers.Include(u => u.Changes).ThenInclude(c => c.ChangedBy).ToListAsync();

        Assert.True(users.All(u => u.Changes.All(c => c.ChangeSource == currentChangeSource)));
        Assert.True(users.All(u => u.Changes.All(c => c.ChangedBy.Id == currentUser.Id)));
    }

    [Fact]
    public void ScriptMigration_SetsCorrectChangeContext()
    {
        var dbContext = fixture.Services.GetRequiredService<MigrationsDbContext>();
        var migrator = dbContext.Database.GetService<IMigrator>();
        var changeSourceProvider = fixture.Services.GetRequiredService<MigrationsCurrentChangeSourceProvider>();
        var currentUserProvider = fixture.Services.GetRequiredService<MigrationsCurrentUserProvider>();

        var currentChangeSource = ChangeSource.Migrations;
        var currentUser = new MigrationsUser { Id = 1 };

        changeSourceProvider.CurrentChangeSource = currentChangeSource;
        currentUserProvider.CurrentUser = currentUser;

        var script = migrator.GenerateScript();
        var scriptLines = script.Split(Environment.NewLine);

        Assert.Equal($"EXEC sp_set_session_context N'ChangeContext.ChangedBy', {currentUser.Id};", scriptLines.FirstOrDefault());
        Assert.Equal($"EXEC sp_set_session_context N'ChangeContext.ChangeSource', {(int)currentChangeSource};", scriptLines.Skip(1).FirstOrDefault());
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        var dbContext = fixture.Services.GetRequiredService<MigrationsDbContext>();
        var migrator = dbContext.Database.GetService<IMigrator>();
        await migrator.MigrateAsync("0");
    }
}