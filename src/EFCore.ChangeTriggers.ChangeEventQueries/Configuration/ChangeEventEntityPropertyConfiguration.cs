using EFCore.ChangeTriggers.ChangeEventQueries.Extensions;
using System.Linq.Expressions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Configuration
{
    public class ChangeEventEntityPropertyConfiguration
    {
        public string Description { get; internal set; }

        public LambdaExpression ValueSelector { get; }

        public ChangeEventEntityPropertyConfiguration(LambdaExpression valueSelector)
        {
            ValueSelector = valueSelector;
            Description = valueSelector.GetRootMemberName();
        }
    }
}