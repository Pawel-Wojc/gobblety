using System.Diagnostics;
using System.Reflection.Metadata;
using static Application.Program;

class View()
{
    public static void printWelcomeMessage()
    {
        Console.WriteLine("Welcome in Gobblety");
        Console.WriteLine("Game rules: ");
        Console.WriteLine("To select goblet type -> SsMmLl");
        Console.WriteLine("To select field type dimensions -> 12, 21 (x,y)");
        Console.WriteLine("Starting with player - orange ");
        Console.WriteLine("Press Enter to start");
        Console.ReadLine();
    }

    public static void drawBoard(Board board)
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
                    if (board.board[x, y, z] != null)
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
                    char colorFirstLetter = board.board[x, y, zOfFindedGobblet].color.ToString()[0];
                    char sizeFirstLetter = board.board[x, y, zOfFindedGobblet].size.ToString()[0];

                    Console.Write("| " + colorFirstLetter + sizeFirstLetter + " ");
                }
            }
            Console.WriteLine("");
        }
    }

    public static void drawPlayerGobblets(Player movingPlayer)
    {
        int smallGoblets = 0;
        int mediumGoblets = 0;
        int largeGoblets = 0;

        Console.WriteLine("Moving player - " + movingPlayer.color);
        Console.WriteLine("Your goblets");
        Console.WriteLine("Size    |  S  |  M  |  L  |");

        foreach (Gobblet goblet in movingPlayer.gobblets)
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
        Console.WriteLine(
            "Quantity|  " + smallGoblets + "  |  " + mediumGoblets + "  |  " + largeGoblets + "  |"
        );
    }

    internal static Dimension selectDimensionToTake(List<Dimension> availableDimensions)
    {
        Dimension? dimension = null;
        Console.WriteLine(
            "Select dimensions (x,y) to pick up goblet -> 00 for x:0 y:0, 21 for x:2 y:1, etc."
        );
        string input = Console.ReadLine();
        dimension = TryParseToXY(input);
        bool isInputCorrect = false;
        if (dimension != null)
        {
            if (availableDimensions.Contains((Dimension)dimension))
            {
                isInputCorrect = true;
            }
        }

        while (!isInputCorrect)
        {
            if (TryParseToXY(input) == null)
            {
                Console.WriteLine(
                    "Invalid input! Please enter a two-character string with digits between 1 and 3."
                );
            }
            else if (!availableDimensions.Contains((Dimension)dimension))
            {
                Console.WriteLine("Invalid input! Cant take this dimension");
                continue;
            }

            input = Console.ReadLine();
            isInputCorrect =
                TryParseToXY(input) != null && availableDimensions.Contains((Dimension)dimension);
        }

        return (Dimension)dimension;
    }

    public static Dimension SelectDimensionToPut(List<Dimension> availableDimensions)
    {
        Dimension? dimension = null;
        Console.WriteLine(
            "Select dimensions (x,y) to pick put goblet -> 00 for x:0 y:0, 21 for x:2 y:1, etc."
        );
        string input = Console.ReadLine();
        dimension = TryParseToXY(input);
        bool isInputCorrect = false;

        //check if input is in the list of available dimensions
        if (dimension != null)
        {
            if (availableDimensions.Contains((Dimension)dimension))
            {
                isInputCorrect = true;
            }
        }

        while (!isInputCorrect)
        {
            if (TryParseToXY(input) == null)
            {
                Console.WriteLine(
                    "Invalid input! Please enter a two-character string with digits between 1 and 3."
                );
            }
            else if (!availableDimensions.Contains((Dimension)dimension))
            {
                Console.WriteLine("Invalid input! Cant take this dimension");
                continue;
            }

            input = Console.ReadLine();
            isInputCorrect =
                TryParseToXY(input) != null && availableDimensions.Contains((Dimension)dimension);
        }

        return (Dimension)dimension;
    }

    static Dimension? TryParseToXY(string input)
    {
        Dimension dimension = new Dimension { };
        switch (input[0])
        {
            case '0':
                dimension.x = 0;
                break;
            case '1':
                dimension.x = 1;
                break;
            case '2':
                dimension.x = 2;
                break;
            default:
                return null;
        }
        switch (input[1])
        {
            case '0':
                dimension.y = 0;
                break;
            case '1':
                dimension.y = 1;
                break;
            case '2':
                dimension.y = 2;
                break;
            default:
                return null;
        }
        return dimension;
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

    public static Size SelectSizeToPlay(List<Size> sizes)
    {
        Size? selectedSize = null;

        Console.WriteLine("Select a size to play -> s - small, m - medium, l - large");
        Console.WriteLine("Sizes available to play: ");
        foreach (Size size in sizes)
        {
            Console.WriteLine(size.ToString());
        }

        while (selectedSize == null)
        {
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
        }
        return (Size)selectedSize;
    }
}
