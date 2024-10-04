﻿using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations.Domain;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations.Infrastructure;

internal class MigrationsChangeSourceProvider : ChangeSourceProvider<ChangeSource>
{
    private readonly MigrationsCurrentChangeSourceProvider changeSourceProvider;

    public MigrationsChangeSourceProvider(MigrationsCurrentChangeSourceProvider changeSourceProvider)
    {
        this.changeSourceProvider = changeSourceProvider;
    }

    public override Task<ChangeSource> GetChangeSourceAsync()
    {
        return Task.FromResult(changeSourceProvider.CurrentChangeSource);
    }

    public override ChangeSource GetChangeSource()
    {
        return changeSourceProvider.CurrentChangeSource;
    }
}
