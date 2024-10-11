
using System.Diagnostics;
using System.Reflection.Metadata;
using static Application.Program;


namespace View
{
    class View()
    {
        static public void printWelcomeMessage()
        {
            Console.WriteLine("Welcome in Gobblety");
            Console.WriteLine("Game rules: ");
            Console.WriteLine("To select goblet type -> SsMmLl");
            Console.WriteLine("To select field type dimensions -> 12, 21 (x,y)");
            Console.WriteLine("Starting with player - orange ");
            Console.WriteLine("Press Enter to start");
            Console.ReadLine();

        }
        static public void drawBoard(Gobblet[,,] board)

        {
            Console.WriteLine("Board");
            Console.WriteLine("| xy | 0  | 1  | 2  |");
            for (int x = 0; x < 3; x++)
            {
                Console.WriteLine("---------------------");
                for (int y = 0; y < 3; y++)
                {
                    bool isFieldEmpty = true;
                    int zOfFindedGobblet = 4;
                    for (int z = 0; z < 3; z++)
                    {
                        if (board[x, y, z].color != null)
                        {
                            isFieldEmpty = false;
                            zOfFindedGobblet = z;
                            break;
                        }

                    }
                    if (y == 0)
                    {
                        Console.Write("| " + x + "  ");
                    }
                    if (isFieldEmpty)
                    {
                        Console.Write("|    ");
                    }
                    else
                    {
                        char colorFirstLetter = board[x, y, zOfFindedGobblet].color.ToString()[0];
                        char sizeFirstLetter = board[x, y, zOfFindedGobblet].size.ToString()[0];

                        Console.Write("| " + colorFirstLetter + sizeFirstLetter + " ");
                    }
                }
                Console.WriteLine("");
            }
        }

        static public void drawPlayerGobblets(Player movingPlayer)
        {
            int smallGoblets = 0;
            int mediumGoblets = 0;
            int largeGoblets = 0;

            Console.WriteLine("Moving player - " + movingPlayer.color);
            Console.WriteLine("Your goblets");
            Console.WriteLine("Size    |  S  |  M  |  L  |");


            foreach (Gobblet goblet in movingPlayer.goblets)
            {
                if (goblet.size == Size.small)
                {
                    smallGoblets++;
                }
                if (goblet.size == Size.medium)
                {
                    mediumGoblets++;
                }
                if (goblet.size == Size.large)
                {
                    largeGoblets++;
                }
            }
            Console.WriteLine("Quantity|  " + smallGoblets + "  |  " + mediumGoblets + "  |  " + largeGoblets + "  |");
        }

        internal static Dimension selectDimensionToTake(Gobblet[,,] board, Player player)
        {

            Dimension dimension = new Dimension { };
            bool dimensionCorrect = false;

            while (!dimensionCorrect)
            {
                Console.WriteLine("Select dimensions (x,y) to pick up goblet");

                string input = Console.ReadLine();
                if (input.Length == 2 && TryParseToXY(input, out int x, out int y))
                {
                    dimension.x = x;
                    dimension.y = y;
                }
                else
                {
                    Console.WriteLine("Invalid input! Please enter a two-character string with digits between 1 and 3.");
                    input = Console.ReadLine();
                    continue;
                }
                if (dimension.x != null && dimension.y != null)
                {
                    bool canTakeThisGoblet = false;
                    for (int i = 0; i < 3; i++)
                    {

                        if (board[(int)dimension.x, (int)dimension.y, i].color != player.color)
                        {
                            if (board[(int)dimension.x, (int)dimension.y, i].color != null)
                            {
                                break;
                            }
                        }
                        else
                        {
                            canTakeThisGoblet = true;

                        }

                    }
                    if (!canTakeThisGoblet)
                    {
                        Console.WriteLine("Cant pick goblet in this dimension. Try another");
                    }
                    else
                    {
                        dimensionCorrect = true;
                    }
                }
            }
            return dimension;
        }
        public static Dimension SelectDimensionToMove(Gobblet[,,] board, Dimension dimensionFrom)
        {
            Gobblet gobbletToMove = new Gobblet();
            //gobblet to move 
            for (int i = 0; i < 3; i++)
            {
                gobbletToMove = board[(int)dimensionFrom.x, (int)dimensionFrom.y, i];
                if (gobbletToMove.size != null)
                {
                    break;
                }
            }

            Console.WriteLine("Select destination dimensions (x,y)");
            string input = Console.ReadLine();
            Dimension dimension = new Dimension { };
            bool dimensionOk = false;
            while (!dimensionOk)
            {
                if (input.Length == 2 && TryParseToXY(input, out int x, out int y))
                {
                    dimension.x = x;
                    dimension.y = y;
                }

                //check the destination is valid
                if (gobbletToMove.size == Size.small)
                {
                    bool noGobletInThisField =
                        board[(int)dimension.x, (int)dimension.y, 0].size == null ||
                        board[(int)dimension.x, (int)dimension.y, 1].size == null ||
                        board[(int)dimension.x, (int)dimension.y, 2].size == null;

                    if (noGobletInThisField)
                    {
                        dimensionOk = true;
                    }
                }

                if (gobbletToMove.size == Size.medium)
                {
                    bool noLargeGobletInThisField =
                        board[(int)dimension.x, (int)dimension.y, 0].size == null ||
                        board[(int)dimension.x, (int)dimension.y, 1].size == null;
                    if (noLargeGobletInThisField)
                    {
                        dimensionOk = true;
                    }

                }
                if (gobbletToMove.size == Size.large)
                {
                    bool noLargeGobletInThisField =
                        board[(int)dimension.x, (int)dimension.y, 0].size == null;
                    if (noLargeGobletInThisField)
                    {
                        dimensionOk = true;
                    }
                }
                if (!dimensionOk)
                {
                    Console.WriteLine("Invalid input! Please enter a two-character string with digits between 1 and 3.");
                    input = Console.ReadLine();
                }
            }
            return dimension;
        }

        internal static Dimension SelectDimensionToPut()
        {
            Console.WriteLine("Select dimensions (x,y) where you want to put gobblet");

            string input = Console.ReadLine();
            Dimension dimension = new Dimension { };

            while (dimension.x == null && dimension.y == null)
            {
                if (input.Length == 2 &&
                                    TryParseToXY(input, out int x, out int y))
                {
                    dimension.x = x;
                    dimension.y = y;

                }
                else
                {
                    Console.WriteLine("Invalid input! Please enter a two-character string with digits between 1 and 3.");
                    input = Console.ReadLine();
                }

            }
            return dimension;
        }

        static bool TryParseToXY(string input, out int x, out int y)
        {
            // Initialize output values
            x = y = 0;

            // Parse first character
            switch (input[0])
            {
                case '0': x = 0; break;
                case '1': x = 1; break;
                case '2': x = 2; break;
                default: return false;
            }

            // Parse second character
            switch (input[1])
            {
                case '0': y = 0; break;
                case '1': y = 1; break;
                case '2': y = 2; break;
                default: return false;
            }
            Console.WriteLine("Z view x:" + x + " y: " + y);

            return true;
        }

        internal static MoveType SelectMoveType()
        {
            while (true)
            {
                Console.WriteLine("Select: Add another gobler - 0, Move Goblet on board - 1");
                string input = Console.ReadLine();
                if (input == "0")
                    return MoveType.addGoblet;
                else if (input == "1")
                    return MoveType.moveGoblet;
                else
                    Console.WriteLine("Inccorect ");
            }

        }
    }

}


