namespace EFCore.ChangeTriggers.Tests.Integration.Common.Repositories
{
    public interface IUserReadRepository<TUser, TUserChange>
    {
        IQueryable<TUser> GetUsers();

        IQueryable<TUserChange> GetUserChanges();
    }
}