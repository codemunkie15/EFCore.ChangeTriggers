using EFCore.ChangeTriggers.Infrastructure;
using EFCore.ChangeTriggers.SqlServer.Interceptors;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EFCore.ChangeTriggers.SqlServer.Infrastructure
{
    public class ChangeTriggersSqlServerDbContextOptionsExtension : ChangeTriggersDbContextOptionsExtension
    {
        private ChangeTriggersSqlServerDbContextOptionsExtensionInfo? info;

        public override ChangeTriggersSqlServerDbContextOptionsExtensionInfo Info =>
            info ??= new ChangeTriggersSqlServerDbContextOptionsExtensionInfo(this);

        public ChangeTriggersSqlServerDbContextOptionsExtension()
        {

        }

        public ChangeTriggersSqlServerDbContextOptionsExtension(ChangeTriggersSqlServerDbContextOptionsExtension copyFrom)
            : base(copyFrom)
        {
        }

        protected override void AddChangedByServices<TChangedByProvider, TChangedBy>()
        {
            AddService<IInterceptor, ChangedBySqlServerDbConnectionInterceptor<TChangedBy>>();

            base.AddChangedByServices<TChangedByProvider, TChangedBy>();
        }

        protected override void AddChangeSourceServices<TChangeSourceProvider, TChangeSource>()
        {
            AddService<IInterceptor, ChangeSourceSqlServerDbConnectionInterceptor<TChangeSource>>();

            base.AddChangeSourceServices<TChangeSourceProvider, TChangeSource>();
        }

        protected override ChangeTriggersDbContextOptionsExtension Clone()
        {
            return new ChangeTriggersSqlServerDbContextOptionsExtension(this);
        }
    }
}
