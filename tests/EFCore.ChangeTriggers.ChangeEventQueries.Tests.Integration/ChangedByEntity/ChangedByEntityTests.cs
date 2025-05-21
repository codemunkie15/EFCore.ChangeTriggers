//using EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.ChangedByEntity.Configuration;
//using EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.ChangedByEntity.Fixtures;
//using Microsoft.Extensions.DependencyInjection;

//namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.ChangedByEntity
//{
//    public class ChangedByEntityTests : IClassFixture<ChangedByEntityFixture>
//    {
//        private readonly ChangedByEntityFixture fixture;

//        public ChangedByEntityTests(ChangedByEntityFixture fixture)
//        {
//            this.fixture = fixture;
//        }

//        [Fact]
//        public void ToChangeEvents_ReturnsCorrectChangeEvents()
//        {
//            var services = new ServiceCollection()
//                .AddChangedByEntity(fixture.GetConnectionString())
//                .BuildServiceProvider();
//        }

//        // Test exceptions

//        // Test passed in config

//        // Test DbContext config

//        // Test include inserts/deletes
//    }
//}
