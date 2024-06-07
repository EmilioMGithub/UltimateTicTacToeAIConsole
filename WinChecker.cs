public class WinChecker
{
    public static TicState checkWin(TicState[,] board)
    {
        if(checkWinForPlayer(board, TicState.X)) return TicState.X;
        if(checkWinForPlayer(board, TicState.O)) return TicState.O;
        return TicState.N;
    }

    private static bool checkWinForPlayer(TicState[,] board, TicState state)
    {   
        for (int rowCol = 0; rowCol < 3; rowCol++)
        {
            bool winInRow = board[rowCol, 0] == state && board[rowCol, 1] == state && board[rowCol, 2] == state;
            bool winInCol = board[0, rowCol] == state && board[1, rowCol] == state && board[2, rowCol] == state;
            if(winInRow || winInCol) return true;
        }

        bool WinInTopLeftDiganol = board[0, 0] == state && board[1, 1] == state && board[2, 2] == state;
        bool WinInTopRightDiganol = board[0, 2] == state && board[1, 1] == state && board[2, 0] == state;
        if (WinInTopLeftDiganol || WinInTopRightDiganol)  return true;

        return false;
    }
}
