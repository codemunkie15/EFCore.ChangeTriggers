namespace EFCore.ChangeTriggers.Tests.Integration.Common
{
    public class SyncOrAsyncValue<T>
    {
        public T SyncValue { get; set; }

        public T AsyncValue { get; set; }

        public T Get(bool useAsync) => useAsync ? AsyncValue : SyncValue;

        public void Set(bool useAsync, T value)
        {
            if (useAsync)
            {
                AsyncValue = value;
            }
            else
            {
                SyncValue = value;
            }
        }
    }
}