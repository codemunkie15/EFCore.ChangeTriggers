using System;

namespace TestHarness
{
    internal class ChangeEvent
    {
        public string Description { get; set; }

        public string? OldValue { get; set; }

        public string? NewValue { get; set; }

        public DateTimeOffset ChangedAt { get; set; }
    }
}
