using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceScalar.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Tests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceScalar
{
    public class MutateEntityTests :
        MutateEntityTestBase<ChangeSourceScalarUser, ChangeSourceScalarUserChange, ChangeSourceScalarDbContext>,
        IClassFixture<MutateEntityFixture>
    {
        private readonly ScalarChangeSourceProvider currentChangeSource;

        public MutateEntityTests(MutateEntityFixture fixture) : base(fixture.Services)
        {
            currentChangeSource = scope.ServiceProvider.GetRequiredService<ScalarChangeSourceProvider>();
        }

        protected override Task SetChangeContext(bool useAsync)
        {
            currentChangeSource.CurrentChangeSource.Set(useAsync, ChangeSourceType.Tests);
            return Task.CompletedTask;
        }

        protected override void Assert(ChangeSourceScalarUserChange userChange, bool useAsync)
        {
            userChange.ChangeSource.Should().Be(currentChangeSource.CurrentChangeSource.Get(useAsync));
        }

        // TODO: Test when ChangedBy user has been deleted
        /*
            var userChangeEntry = TestScope.DbContext.Entry(userChange);
            userChangeEntry.Property("ChangedById").CurrentValue.Should().Be(user.Id);
        */
    }
}