namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration
{
    [CollectionDefinition("ChangedByEntity")]
    public class ChangedByEntityCollection : ICollectionFixture<ContainerFixture>
    {
    }

    [CollectionDefinition("ChangedByScalar")]
    public class ChangedByScalarCollection : ICollectionFixture<ContainerFixture>
    {
    }

    [CollectionDefinition("ChangeSourceEntity")]
    public class ChangeSourceEntityCollection : ICollectionFixture<ContainerFixture>
    {
    }

    [CollectionDefinition("ChangeSourceScalar")]
    public class ChangeSourceScalarCollection : ICollectionFixture<ContainerFixture>
    {
    }
}
