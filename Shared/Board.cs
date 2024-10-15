public class Board
{
    private Board() { }

    public Gobblet[,,] board = new Gobblet[3, 3, 3];

    private static Board? boardInstance = null;

    public static Board GetBoard()
    {
        if (boardInstance == null)
        {
            boardInstance = new Board();
        }
        return boardInstance;
    }

    public Gobblet[,,] getBoard()
    {
        return board;
    }

    public Gobblet? GetTopGobletOnBoard(int x, int y)
    {
        for (int z = 0; z < 3; z++)
        {
            if (board[x, y, z] != null)
            {
                return board[x, y, z];
            }
        }
        return null;
    }

    public Color? CheckWinnerColor()
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (
                    CheckLine(
                        GetTopGobletOnBoard(x, 0),
                        GetTopGobletOnBoard(x, 1),
                        GetTopGobletOnBoard(x, 2)
                    )
                )
                {
                    return (Color)GetTopGobletOnBoard(x, 0).color;
                }
                if (
                    CheckLine(
                        GetTopGobletOnBoard(0, y),
                        GetTopGobletOnBoard(1, y),
                        GetTopGobletOnBoard(2, y)
                    )
                )
                {
                    return (Color)GetTopGobletOnBoard(0, y).color;
                }
            }
        }

        //checking diagonals
        if (
            CheckLine(
                GetTopGobletOnBoard(0, 0),
                GetTopGobletOnBoard(1, 1),
                GetTopGobletOnBoard(2, 2)
            )
            || CheckLine(
                GetTopGobletOnBoard(0, 2),
                GetTopGobletOnBoard(1, 1),
                GetTopGobletOnBoard(2, 0)
            )
        )
        {
            return (Color)GetTopGobletOnBoard(1, 1).color;
        }
        return null;
    }

    private static bool CheckLine(Gobblet g1, Gobblet g2, Gobblet g3)
    {
        if (g1 == null || g2 == null || g3 == null)
        {
            return false;
        }
        return g1.color == g2.color && g2.color == g3.color && g1.color != null;
    }

    public bool MoveGoblet(Dimension dimensionFrom, Dimension dimensionToPut)
    {
        Gobblet gobbletToMove = DeleteGobletFromBoard(dimensionFrom);
        if (gobbletToMove != null)
        {
            return PutGobletOnBoard(dimensionToPut, gobbletToMove);
        }
        return false;
    }

    private Gobblet? DeleteGobletFromBoard(Dimension dimensionToDelete)
    {
        for (int i = 0; i < 3; i++)
        {
            Gobblet gobblet = board[dimensionToDelete.x, dimensionToDelete.y, i];
            if (gobblet != null)
            {
                board[dimensionToDelete.x, dimensionToDelete.y, i] = null;
                return gobblet;
            }
        }
        return null;
    }

    private bool PutGobletOnBoard(Dimension dimensionToPut, Gobblet gobblet)
    {
        switch (gobblet.size)
        {
            case Size.small:
                board[dimensionToPut.x, dimensionToPut.y, 2] = gobblet;
                return true;
            case Size.medium:
                board[dimensionToPut.x, dimensionToPut.y, 1] = gobblet;
                return true;
            case Size.large:
                board[dimensionToPut.x, dimensionToPut.y, 0] = gobblet;
                return true;
            default:
                return false;
        }
    }

    internal bool PlaceGobletOnBoard( //return true if you can put this goblet in this dimension
        Size playerSelectedSize,
        Player currentPlayer,
        Dimension dimension
    )
    {
        int x = dimension.x;
        int y = dimension.y;
        //check if field is empty, check a size of last gobblet,
        Gobblet gobletToPlace = new(playerSelectedSize, currentPlayer.color);

        if (GetTopGobletOnBoard(dimension.x, dimension.y) == null)
        {
            PutGobletOnBoard(new Dimension(dimension.x, dimension.y), gobletToPlace);
            return true;
        }

        if (gobletToPlace.size < GetTopGobletOnBoard(dimension.x, dimension.y).size)
        {
            if (playerSelectedSize == Size.small)
            { //only if board cell is empty you can put small goblet
                return false;
            }

            if (gobletToPlace.size < playerSelectedSize)
            {
                PutGobletOnBoard(new Dimension(dimension.x, dimension.y), gobletToPlace);
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    internal List<Dimension> GetAvailableDimensionsToTake(Player currentPlayer)
    {
        List<Dimension> availableDimensions = new List<Dimension>();
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Gobblet? gobblet = GetTopGobletOnBoard(x, y);
                if (gobblet != null && gobblet.color == currentPlayer.color)
                {
                    availableDimensions.Add(new Dimension(x, y));
                }
            }
        }
        return availableDimensions;
    }

    internal List<Dimension> GetAvailableDimensionsToPut(Gobblet gobblet)
    {
        List<Dimension> availableDimensions = new List<Dimension>();
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Gobblet? gobbletToCompare = GetTopGobletOnBoard(x, y);
                if (gobbletToCompare == null)
                {
                    availableDimensions.Add(new Dimension(x, y));
                    continue;
                }
                if (gobblet.size > gobbletToCompare.size)
                {
                    availableDimensions.Add(new Dimension(x, y));
                }
            }
        }
        return availableDimensions;
    }
}
