using A_AspNetCore.Models;
using EFCore.ChangeTriggers.Abstractions;

namespace A_AspNetCore
{
    public class SampleChangeSourceProvider : ChangeSourceProvider<ChangeSource>
    {
        public override ChangeSource GetChangeSource()
        {
            return ChangeSource.AspNetCore;
        }

        public override Task<ChangeSource> GetChangeSourceAsync()
        {
            return Task.FromResult(ChangeSource.AspNetCore);
        }

        public override ChangeSource GetMigrationChangeSource()
        {
            return ChangeSource.Migration;
        }

        public override Task<ChangeSource> GetMigrationChangeSourceAsync()
        {
            return Task.FromResult(ChangeSource.Migration);
        }
    }
}
