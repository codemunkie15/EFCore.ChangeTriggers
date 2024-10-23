﻿using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Infrastructure;

internal class EntityCurrentUserProvider
{
    public ChangedByEntityUser CurrentUser { get; set; }

    public ChangedByEntityUser CurrentUserAsync { get; set; }

    public ChangedByEntityUser MigrationCurrentUser { get; set; }

    public ChangedByEntityUser MigrationCurrentUserAsync { get; set; }
}
