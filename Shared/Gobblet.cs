public enum Size
{
    small,
    medium,
    large,
};

public enum Color
{
    blue,
    orange,
};

public class Gobblet : IComparable<Gobblet>
{
    public Size size;
    public Color color;

    // public Gobblet()
    // {
    //     size = null;
    //     color = null;
    // }
    public Gobblet(Size size, Color color)
    {
        this.size = size;
        this.color = color;
    }

    public static List<Gobblet> getGobletForPlayer(Color color)
    {
        return new List<Gobblet>
        {
            new(Size.small, color),
            new(Size.small, color),
            new(Size.medium, color),
            new(Size.medium, color),
            new(Size.large, color),
            new(Size.large, color),
        };
    }

    public int CompareTo(Gobblet other)
    {
        return size.CompareTo(other.size);
    }
}
