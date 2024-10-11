using Model;
using View;


namespace Application
{

    internal static class Program
    {
        public struct Dimension() { public int? x = null; public int? y = null; };
        static bool winner = false;
        static Gobblet[,,] board = new Gobblet[3, 3, 3];
        public enum MoveType { addGoblet, moveGoblet };
        public enum Size { small, medium, large };
        public enum Color { blue, orange };
        public struct Player() { public Color color; public List<Gobblet> goblets; };
        public struct Gobblet() { public Size? size; public Color? color; };


        public static void Main()
        {
            Player orangePlayer = new Player()
            {
                color = Color.orange,
                goblets = new List<Gobblet> {
                    new Gobblet { size = Size.small, color = Color.orange },
                    new Gobblet { size = Size.small, color = Color.orange },
                    new Gobblet { size = Size.medium, color = Color.orange },
                    new Gobblet { size = Size.medium, color = Color.orange },
                    new Gobblet { size = Size.large, color = Color.orange },
                    new Gobblet { size = Size.large, color = Color.orange }
                }
            };

            Player bluePlayer = new Player()
            {
                color = Color.blue,
                goblets = new List<Gobblet>{
                    new Gobblet { size = Size.small, color = Color.blue },
                    new Gobblet { size = Size.small, color = Color.blue },
                    new Gobblet { size = Size.medium, color = Color.blue },
                    new Gobblet { size = Size.medium, color = Color.blue },
                    new Gobblet { size = Size.large, color = Color.blue },
                    new Gobblet { size = Size.large, color = Color.blue }
                }
            };

            Player currentPlayer = orangePlayer;
            int moveCounter = 0;
            while (!winner)
            {
                if (moveCounter == 0)
                {
                    View.View.printWelcomeMessage();
                }
                View.View.drawBoard(board);
                View.View.drawPlayerGobblets(currentPlayer);
                if (moveCounter < 3) //in first two steps you cant move goblets on board
                {
                    Size playerSelectedSize = Model.Model.SelectSizeToPlay(currentPlayer);
                    Dimension dimesnions = View.View.SelectDimensionToPut();
                    while (!Model.Model.PlaceGobletOnBoardOk(playerSelectedSize, currentPlayer, board, dimesnions))
                    {
                        playerSelectedSize = Model.Model.SelectSizeToPlay(currentPlayer);
                        dimesnions = View.View.SelectDimensionToPut();
                    }
                }
                else
                {
                    MoveType moveType;
                    if (Model.Model.CheckPlayerHaveGoblets(currentPlayer))
                    {
                        moveType = View.View.SelectMoveType();
                        if (moveType == MoveType.addGoblet)
                        {
                            Size playerSelectedSize = Model.Model.SelectSizeToPlay(currentPlayer);
                            Dimension dimesnions = View.View.SelectDimensionToPut();
                            while (!Model.Model.PlaceGobletOnBoardOk(playerSelectedSize, currentPlayer, board, dimesnions))
                            {
                                playerSelectedSize = Model.Model.SelectSizeToPlay(currentPlayer);
                                dimesnions = View.View.SelectDimensionToPut();
                            }
                        }
                        else if (moveType == MoveType.moveGoblet)
                        {
                            Dimension dimensionFrom = View.View.selectDimensionToTake(board, currentPlayer);
                            Dimension dimensionToPut = View.View.SelectDimensionToMove(board, dimensionFrom);
                            Model.Model.MoveGoblet(dimensionFrom, dimensionToPut, board);
                            View.View.drawBoard(board);
                        }
                    }
                    else
                    {
                        moveType = MoveType.moveGoblet;

                    }
                }
                if (Model.Model.CheckWinner(board) != null)
                {
                    winner = true;
                    Console.WriteLine("THE WINNER IS:  " + Model.Model.CheckWinner(board));
                }
                if (currentPlayer.color == Color.orange) //switch current player
                {
                    currentPlayer = bluePlayer;

                }
                else { currentPlayer = orangePlayer; }

                moveCounter++;
            }
        }

    }
}






//sb -small blue, mb - medium blue,  bb - big blue 