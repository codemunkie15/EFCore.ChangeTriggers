using BenchmarkDotNet.Attributes;
using Benchmarks.DbModels.Users;
using EFCore.ChangeTriggers.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Benchmarks
{
    [EvaluateOverhead(false)]
    //[RunOncePerIteration]
    //[MaxIterationCount(5000)]
    public class ChangeTriggersBenchmarks
    {
        private MyDbContext dbContext;
        private User user;

        [GlobalSetup]
        public void Setup()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection
                .AddDbContext<MyDbContext>(options =>
                 {
                     options
                         .UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=ChangeTriggers;Integrated Security=True;Encrypt=False;TrustServerCertificate=False")
                         .UseSqlServerChangeTriggers<ChangedByProvider, User, ChangeSourceProvider, ChangeSourceType>();
                 });

            var serviceProvider = serviceCollection.BuildServiceProvider();
            dbContext = serviceProvider.GetRequiredService<MyDbContext>();
            user = new User
            {
                Column1 = Guid.NewGuid().ToString(),
                Column2 = Guid.NewGuid().ToString(),
                Column3 = Guid.NewGuid().ToString(),
                Column4 = Guid.NewGuid().ToString(),
                Column5 = Guid.NewGuid().ToString(),
                Column6 = Guid.NewGuid().ToString(),
                Column7 = Guid.NewGuid().ToString(),
                Column8 = Guid.NewGuid().ToString(),
                Column9 = Guid.NewGuid().ToString(),
                Column10 = Guid.NewGuid().ToString(),
                Column11 = Guid.NewGuid().ToString(),
                Column12 = Guid.NewGuid().ToString(),
                Column13 = Guid.NewGuid().ToString(),
                Column14 = Guid.NewGuid().ToString(),
                Column15 = Guid.NewGuid().ToString(),
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }

        [Benchmark]
        public void CreateUser()
        {
            var user = new User
            {
                Column1 = Guid.NewGuid().ToString(),
                Column2 = Guid.NewGuid().ToString(),
                Column3 = Guid.NewGuid().ToString(),
                Column4 = Guid.NewGuid().ToString(),
                Column5 = Guid.NewGuid().ToString(),
                Column6 = Guid.NewGuid().ToString(),
                Column7 = Guid.NewGuid().ToString(),
                Column8 = Guid.NewGuid().ToString(),
                Column9 = Guid.NewGuid().ToString(),
                Column10 = Guid.NewGuid().ToString(),
                Column11 = Guid.NewGuid().ToString(),
                Column12 = Guid.NewGuid().ToString(),
                Column13 = Guid.NewGuid().ToString(),
                Column14 = Guid.NewGuid().ToString(),
                Column15 = Guid.NewGuid().ToString(),
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }

        //[Benchmark]
        //public void UpdateUser()
        //{
        //    user.Column1 = Guid.NewGuid().ToString();
        //    user.Column2 = Guid.NewGuid().ToString();
        //    user.Column3 = Guid.NewGuid().ToString();
        //    user.Column4 = Guid.NewGuid().ToString();
        //    user.Column5 = Guid.NewGuid().ToString();
        //    user.Column6 = Guid.NewGuid().ToString();
        //    user.Column7 = Guid.NewGuid().ToString();
        //    user.Column8 = Guid.NewGuid().ToString();
        //    user.Column9 = Guid.NewGuid().ToString();
        //    user.Column10 = Guid.NewGuid().ToString();
        //    user.Column11 = Guid.NewGuid().ToString();
        //    user.Column12 = Guid.NewGuid().ToString();
        //    user.Column13 = Guid.NewGuid().ToString();
        //    user.Column14 = Guid.NewGuid().ToString();
        //    user.Column15 = Guid.NewGuid().ToString();

        //    dbContext.SaveChanges();
        //}
    }
}
