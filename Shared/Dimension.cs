public struct Dimension()
{
    public int x;
    public int y;

    public Dimension(int x, int y)
        : this()
    {
        this.x = x;
        this.y = y;
    }

    public static bool operator ==(Dimension d1, Dimension d2)
    {
        return d1.x == d2.x && d1.y == d2.y;
    }

    public static bool operator !=(Dimension d1, Dimension d2)
    {
        return !(d1 == d2);
    }
}
