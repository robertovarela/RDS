namespace RDS.Core.Models.ApplicationUser;

public struct DayMonth : IComparable<DayMonth>, IEquatable<DayMonth>
{
    public int Day { get; }
    public int Month { get; }

    public DayMonth(int day, int month)
    {
        if (day < 1 || day > DateTime.DaysInMonth(1, month))
        {
            throw new ArgumentOutOfRangeException(nameof(day), "Dia inválido para o mês especificado.");
        }
        if (month < 1 || month > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month), "O mês deve estar entre 1 e 12.");
        }

        Day = day;
        Month = month;
    }

    public override string ToString()
    {
        return $"{Day.ToString("D2")}/{Month.ToString("D2")}";
    }

    public int CompareTo(DayMonth other)
    {
        if (Month == other.Month)
        {
            return Day.CompareTo(other.Day);
        }
        return Month.CompareTo(other.Month);
    }

    public bool Equals(DayMonth other)
    {
        return Day == other.Day && Month == other.Month;
    }

    public override bool Equals(object? obj)
    {
        return obj is DayMonth other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Day, Month);
    }

    public static bool operator ==(DayMonth left, DayMonth right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(DayMonth left, DayMonth right)
    {
        return !(left == right);
    }

    public static bool operator <(DayMonth left, DayMonth right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator >(DayMonth left, DayMonth right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator <=(DayMonth left, DayMonth right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >=(DayMonth left, DayMonth right)
    {
        return left.CompareTo(right) >= 0;
    }
}