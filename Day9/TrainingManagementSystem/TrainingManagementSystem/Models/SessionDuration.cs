namespace TrainingManagementSystem.Models;

public struct SessionDuration : IEquatable<SessionDuration>
{
    public int Days { get; }

    public SessionDuration(int days)
    {
        if (days <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(days),
                "Duration must be positive.");
        }

        Days = days;
    }

    public bool Equals(SessionDuration other)
    {
        return Days == other.Days;
    }

    public override bool Equals(object? obj)
    {
        return obj is SessionDuration other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Days.GetHashCode();
    }

    public override string ToString()
    {
        return $"{Days} days";
    }
}