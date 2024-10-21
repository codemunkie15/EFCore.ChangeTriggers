﻿using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Infrastructure;

internal class ChangedByScalarProvider : ChangedByProvider<string>
{
    public bool UseCustomGetMigrationChangedBy { get; set; }

    private readonly ScalarCurrentUserProvider currentUserProvider;

    public ChangedByScalarProvider(ScalarCurrentUserProvider currentUserProvider)
    {
        this.currentUserProvider = currentUserProvider;
    }

    public override Task<string> GetChangedByAsync()
    {
        return Task.FromResult(currentUserProvider.CurrentUserAsync);
    }

    public override string GetChangedBy()
    {
        return currentUserProvider.CurrentUser;
    }

    public override string GetMigrationChangedBy()
    {
        if (UseCustomGetMigrationChangedBy)
        {
            return currentUserProvider.MigrationCurrentUser;
        }

        return base.GetMigrationChangedBy();
    }

    public override Task<string> GetMigrationChangedByAsync()
    {
        if (UseCustomGetMigrationChangedBy)
        {
            return Task.FromResult(currentUserProvider.MigrationCurrentUserAsync);
        }

        return base.GetMigrationChangedByAsync();
    }
}
