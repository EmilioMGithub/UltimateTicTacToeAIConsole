public class TTTBoard
{
    private TicState[,] board;
    private TicState boardState;

    public TTTBoard()
    {
        board = new TicState[3, 3];
        TicUtils.GridTraverse((row, col) => board[row, col] = TicState.N);
        boardState = TicState.N;
    }

    private TTTBoard(TTTBoard other)
    {
        board = new TicState[3,3];
        TicUtils.GridTraverse((row, col) => board[row,col] = other.board[row,col]);
        boardState = other.boardState;
    }

    public TicState MakeMove(int row, int col, TicState player)
    {
        board[row, col] = player;
        boardState = TicUtils.checkWin(board);
        return boardState;
    }

    private bool isBoardFilled()
    {
        for(int row = 0; row<3; row++)
        {
            for(int col = 0; col<3; col++)
            {
                if(board[row, col] == TicState.N) return false;
            }
        }
        return true;
    }

    public TicState GetStateAtPoint(int row, int col) => board[row, col];
    public bool IsOpenSqaure(int row, int col) => GetStateAtPoint(row, col) == TicState.N;

    public TicState GetBoardState() => boardState;
    public TTTBoard CloneBoard() => new TTTBoard(this);

    public string GetRowAsString(int row)
    {   
        if(boardState != TicState.N)
        {
            if(row == 1) return $"   {boardState}   ";
            return "       ";
        }

        if(isBoardFilled())
        {
            if(row == 1) return "   T   ";
            return "       ";
        }
        
        return $" {board[row, 0]} {board[row, 1]} {board[row, 2]} ";
    }
}
