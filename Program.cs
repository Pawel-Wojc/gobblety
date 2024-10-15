using System.Numerics;

namespace Application;

internal static class Program
{
    private static bool _winner = false;
    private static Board _board;
    private static Player _currentPlayer;

    public static void Main()
    {
        Player orangePlayer = new Player(Color.orange);
        Player bluePlayer = new Player(Color.blue);
        _board = Board.GetBoard();
        _currentPlayer = orangePlayer;
        int moveCounter = 0;

        while (!_winner)
        {
            if (moveCounter == 0)
            {
                View.printWelcomeMessage();
            }
            View.drawBoard(_board);
            View.drawPlayerGobblets(_currentPlayer);
            if (moveCounter < 2) //in first two steps you cant move goblets on board
            {
                PutGobletOnBoard();
            }
            else
            {
                MoveType moveType;
                if (Player.CheckPlayerHaveGoblets(_currentPlayer))
                {
                    moveType = View.SelectMoveType();
                    if (moveType == MoveType.addGoblet)
                    {
                        PutGobletOnBoard();
                    }
                    else if (moveType == MoveType.moveGoblet)
                    {
                        MoveGobletOnBoard();
                        View.drawBoard(_board);
                    }
                }
                else
                {
                    moveType = MoveType.moveGoblet;
                }
            }
            if (_board.CheckWinnerColor() != null)
            {
                _winner = true;
                Console.WriteLine("THE WINNER IS:  " + _board.CheckWinnerColor());
            }
            if (_currentPlayer.color == Color.orange) //switch current player
            {
                _currentPlayer = bluePlayer;
            }
            else
            {
                _currentPlayer = orangePlayer;
            }

            moveCounter++;
        }
    }

    static void PutGobletOnBoard()
    {
        Size playerSelectedSize;
        Dimension dimensionToPut;

        do
        {
            //which size want to play
            playerSelectedSize = View.SelectSizeToPlay(_currentPlayer.GetAvailableSizes());

            //pass to view available dimensions where player can put goblet
            dimensionToPut = View.SelectDimensionToPut(
                _board.GetAvailableDimensionsToPut(
                    new Gobblet(playerSelectedSize, _currentPlayer.color)
                )
            );
        } while (!_board.PlaceGobletOnBoard(playerSelectedSize, _currentPlayer, dimensionToPut));
    }

    static void MoveGobletOnBoard()
    {
        // get avavailable dimensions from which player can take goblet, can only move goblet in his color
        List<Dimension> dimensionsToTake = _board.GetAvailableDimensionsToTake(_currentPlayer);

        //get user choice from available dimensions
        Dimension dimensionFrom = View.selectDimensionToTake(dimensionsToTake);

        // get available dimensions where player can put goblet, can only put goblet on smaller one or in empty space
        List<Dimension> dimensionsToPut = _board.GetAvailableDimensionsToPut(
            _board.GetTopGobletOnBoard(dimensionFrom.x, dimensionFrom.y)
        );

        // get user choice from available dimensions
        Dimension dimensionToPut = View.SelectDimensionToPut(dimensionsToPut);

        //move goblet
        _board.MoveGoblet(dimensionFrom, dimensionToPut);
    }
}
