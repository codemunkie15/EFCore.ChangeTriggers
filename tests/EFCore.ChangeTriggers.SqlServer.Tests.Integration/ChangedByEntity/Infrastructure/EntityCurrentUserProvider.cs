﻿using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Infrastructure;

internal class EntityCurrentUserProvider
{
    public ChangedByEntityUser CurrentUser { get; set; }
}