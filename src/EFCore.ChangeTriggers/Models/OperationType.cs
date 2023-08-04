namespace EFCore.ChangeTriggers.Models
{
    /// <summary>
    /// Indicates the type of change operation.
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// Data inserted.
        /// </summary>
        Insert = 1,

        /// <summary>
        /// Data updated.
        /// </summary>
        Update,

        /// <summary>
        /// Data deleted.
        /// </summary>
        Delete
    }
}
