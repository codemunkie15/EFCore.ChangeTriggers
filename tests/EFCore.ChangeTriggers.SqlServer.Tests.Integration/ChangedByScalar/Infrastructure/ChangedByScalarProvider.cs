﻿using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Infrastructure;

internal class ChangedByScalarProvider : ChangedByProvider<string>
{
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
}
