namespace EFCore.ChangeTriggers.Tests.Integration.Common.Domain
{
    public interface IHasChangeId
    {
        public int ChangeId { get; set; }
    }
}
