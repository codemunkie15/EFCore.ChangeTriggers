using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity
{
    public static class ChangeEntityAssert
    {
        public static void HasEqualProperties(ChangedByEntityUser expected, ChangedByEntityUserChange actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Username, actual.Username);
        }
    }
}