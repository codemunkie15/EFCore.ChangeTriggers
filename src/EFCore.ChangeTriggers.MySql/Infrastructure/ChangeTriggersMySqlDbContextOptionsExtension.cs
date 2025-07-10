using EFCore.ChangeTriggers.Infrastructure;
using EFCore.ChangeTriggers.MySql.Interceptors;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EFCore.ChangeTriggers.MySql.Infrastructure
{
    public class ChangeTriggersMySqlDbContextOptionsExtension : ChangeTriggersDbContextOptionsExtension
    {
        private ChangeTriggersMySqlDbContextOptionsExtensionInfo? info;

        public override ChangeTriggersMySqlDbContextOptionsExtensionInfo Info =>
            info ??= new ChangeTriggersMySqlDbContextOptionsExtensionInfo(this);

        public ChangeTriggersMySqlDbContextOptionsExtension()
        {

        }

        public ChangeTriggersMySqlDbContextOptionsExtension(ChangeTriggersMySqlDbContextOptionsExtension copyFrom)
            : base(copyFrom)
        {
        }

        protected override void AddChangedByServices<TChangedByProvider, TChangedBy>()
        {
            AddService<IInterceptor, ChangedByMySqlDbConnectionInterceptor<TChangedBy>>();

            base.AddChangedByServices<TChangedByProvider, TChangedBy>();
        }

        protected override void AddChangeSourceServices<TChangeSourceProvider, TChangeSource>()
        {
            AddService<IInterceptor, ChangeSourceMySqlDbConnectionInterceptor<TChangeSource>>();

            base.AddChangeSourceServices<TChangeSourceProvider, TChangeSource>();
        }

        protected override ChangeTriggersDbContextOptionsExtension Clone()
        {
            return new ChangeTriggersMySqlDbContextOptionsExtension(this);
        }
    }
}
