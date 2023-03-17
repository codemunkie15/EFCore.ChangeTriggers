namespace EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Operations
{
    public class ChangeContextColumn : IEquatable<ChangeContextColumn>
    {
        public ChangeContextColumn(string name)
        {
            Name = name;
        }

        public ChangeContextColumn(string name, string type)
            :this(name)
        {
            Type = type;
        }

        public string Name { get; set; }

        public string? Type { get; set; }

        public bool Equals(ChangeContextColumn? other)
        {
            if (other == null)
            {
                return false;
            }

            return
                string.Equals(Name, other.Name) &&
                string.Equals(Type, other.Type);
        }

        public override bool Equals(object? obj) => this.Equals(obj as ChangeContextColumn);

        public override int GetHashCode() => HashCode.Combine(Name, Type);

        public static bool operator ==(ChangeContextColumn? left, ChangeContextColumn? right) => Equals(left, right);

        public static bool operator !=(ChangeContextColumn? left, ChangeContextColumn? right) => !Equals(left, right);
    }
}
