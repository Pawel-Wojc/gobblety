using static Application.Program;
using View;
using System.Dynamic;
using System.Runtime.ExceptionServices;

namespace Model
{
    class Model()
    {
        public static Size SelectSizeToPlay(Player player)
        {
            bool selectionCorrect = false;
            Size? selectedSize = null;

            while (!selectionCorrect)
            {
                Console.WriteLine("Select size to play, s - small, m - medium, l - large");
                string userInputSize = Console.ReadLine();
                switch (userInputSize)
                {
                    case "s":
                        selectedSize = Size.small;
                        break;
                    case "m":
                        selectedSize = Size.medium;
                        break;
                    case "l":
                        selectedSize = Size.large;
                        break;
                    case "S":
                        selectedSize = Size.small;
                        break;
                    case "M":
                        selectedSize = Size.medium;
                        break;
                    case "L":
                        selectedSize = Size.large;
                        break;
                    default:
                        Console.WriteLine("Selection incorect!!! Correct => SsMmLl");
                        break;
                }

                if (selectedSize != null)
                {
                    if (!player.goblets.Exists((goblet) => goblet.size == selectedSize))
                    {
                        Console.WriteLine("You dont have goblets in this size. Try another");
                    }
                    else
                    {
                        selectionCorrect = true;
                    }
                }
            }
            return (Size)selectedSize;
        }

        public static bool CheckPlayerHaveGoblets(Player player)
        {

            if (player.goblets.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static void Play()
        {

        }

        internal static bool PlaceGobletOnBoardOk(Size playerSelectedSize, Player currentPlayer, Gobblet[,,] board, Dimension dimension)
        {
            int x = (int)dimension.x;
            int y = (int)dimension.y;
            //check if field is empty, check a size of last goblin, 
            Gobblet gobletToPlace = new Gobblet { size = playerSelectedSize, color = currentPlayer.color };
            if (playerSelectedSize == Size.small) //if in this dimension is any goblet you cant put your goblet
            {
                bool noGobletsInThisDimensionAnySize =
                    board[x, y, 0].size == null
                    && board[x, y, 1].size == null
                    && board[x, y, 2].size == null;
                if (noGobletsInThisDimensionAnySize)
                {
                    board[x, y, 2] = gobletToPlace;
                    Gobblet toDelete = currentPlayer.goblets.Find((goblet) => goblet.size == playerSelectedSize);
                    currentPlayer.goblets.Remove(toDelete);
                }
                else
                {
                    Console.WriteLine("Cant put this goblet here, try another size or dimension");
                    return false;
                }
            }
            if (playerSelectedSize == Size.medium)
            {
                //checking if there is a goblet in the same or bigger size
                bool noLargeOrMediumGobletsInThisDimension = board[x, y, 0].size == null && board[x, y, 1].size == null;

                if (noLargeOrMediumGobletsInThisDimension)
                {
                    board[x, y, 1] = gobletToPlace;
                    Gobblet toDelete = currentPlayer.goblets.Find((goblet) => goblet.size == playerSelectedSize);
                    currentPlayer.goblets.Remove(toDelete);

                }
                else
                {
                    Console.WriteLine("Cant put this goblet here, try another size or dimension");
                    return false;
                }

            }
            if (playerSelectedSize == Size.large)
            {
                bool noLargeGobletsInThisDimension = board[x, y, 0].size == null;
                if (noLargeGobletsInThisDimension)
                {
                    board[x, y, 0] = gobletToPlace;
                    Gobblet toDelete = currentPlayer.goblets.Find((goblet) => goblet.size == playerSelectedSize);
                    currentPlayer.goblets.Remove(toDelete);
                }
                else
                {
                    Console.WriteLine("Cant put this goblet here, try another size or dimension");
                    return false;
                }

            }


            return true;

        }

        internal static void MoveGoblet(Dimension dimensionFrom, Dimension dimensionToPut, Gobblet[,,] board)
        {
            Gobblet gobbletToMove = new Gobblet();
            for (int i = 0; i < 3; i++)
            {
                Gobblet gobblet = board[(int)dimensionFrom.x, (int)dimensionFrom.y, i];
                if (gobblet.size != null)
                {
                    gobbletToMove = gobblet;
                    board[(int)dimensionFrom.x, (int)dimensionFrom.y, i].size = null;
                    board[(int)dimensionFrom.x, (int)dimensionFrom.y, i].color = null;
                    break;
                }
            }

            if (gobbletToMove.size == Size.small)
            {
                board[(int)dimensionToPut.x, (int)dimensionToPut.y, 2] = gobbletToMove;
            }
            if (gobbletToMove.size == Size.medium)
            {
                board[(int)dimensionToPut.x, (int)dimensionToPut.y, 1] = gobbletToMove;
            }
            if (gobbletToMove.size == Size.large)
            {
                board[(int)dimensionToPut.x, (int)dimensionToPut.y, 0] = gobbletToMove;
            }
        }

        internal static Color? CheckWinner(Gobblet[,,] board)
        {



            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (CheckLine(GetTopmostGobblet(x, 0, board), GetTopmostGobblet(x, 1, board), GetTopmostGobblet(x, 2, board)))
                    {
                        return (Color)GetTopmostGobblet(x, 0, board).color;
                    }
                    if (CheckLine(GetTopmostGobblet(0, y, board), GetTopmostGobblet(1, y, board), GetTopmostGobblet(2, y, board)))
                    {
                        return (Color)GetTopmostGobblet(0, y, board).color;
                    }
                }
            }

            //checking diagonals
            if (CheckLine(GetTopmostGobblet(0, 0, board), GetTopmostGobblet(1, 1, board), GetTopmostGobblet(2, 2, board)) ||
            CheckLine(GetTopmostGobblet(0, 2, board), GetTopmostGobblet(1, 1, board), GetTopmostGobblet(2, 0, board)))
            {
                return (Color)GetTopmostGobblet(1, 1, board).color;
            }
            return null;

        }

        public static Gobblet GetTopmostGobblet(int x, int y, Gobblet[,,]? board)
        {
            Gobblet goblet = new Gobblet();
            for (int z = 0; z < 3; z++)
            {
                goblet = board[x, y, z];
                if (goblet.size != null)
                {
                    return goblet;
                }
            }
            return goblet;
        }

        private static bool CheckLine(Gobblet g1, Gobblet g2, Gobblet g3)
        {
            return g1.color == g2.color && g2.color == g3.color && g1.color != null;

        }
    }
}