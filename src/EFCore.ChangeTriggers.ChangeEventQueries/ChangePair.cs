namespace EFCore.ChangeTriggers.ChangeEventQueries
{
    internal class ChangePair<TChange>
    {
        public required TChange Current { get; set; }

        public required TChange Previous { get; set; }
    }
}