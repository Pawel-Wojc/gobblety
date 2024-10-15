public class Player
{
    public List<Gobblet> gobblets;
    public readonly Color color;
    public Player(Color color)
    {
        gobblets = Gobblet.getGobletForPlayer(color);
        this.color = color;
    }


    public static bool CheckPlayerHaveGoblets(Player player)
    {
        return player.gobblets.Count() > 0;
    }

    public List<Size> GetAvailableSizes()
    {
        List<Size> availableSizes = new List<Size>();

        foreach (Gobblet gobblet in gobblets)
        {
            if (!availableSizes.Contains(gobblet.size))
            {
                availableSizes.Add(gobblet.size);
            }
        }

        return availableSizes;
    }
}