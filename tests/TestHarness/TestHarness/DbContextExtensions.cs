using Microsoft.EntityFrameworkCore;

namespace TestHarness
{
    internal static class DbContextExtensions
    {
        public static ChangeEventQueryBuilder CreateChangeEventQueryBuilder(this DbContext context)
        {
            return new ChangeEventQueryBuilder(context);
        }
    }
}
