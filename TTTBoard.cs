public class TTTBoard
{
    private TicState[,] board;
    private TicState boardState;

    public TTTBoard()
    {
        board = new TicState[3, 3];
        for(int row = 0; row < 3; row++)
        {
            for(int col = 0; col < 3; col++)
            {
                board[row, col] = TicState.N;
            }
        }

        boardState = TicState.N;
    }

    private TTTBoard(TTTBoard other)
    {
        board = new TicState[3,3];
        for(int row = 0; row<3; row++){
            for(int col = 0; col<3; col++){
                board[row,col] = other.board[row,col];
            }
        }
        boardState = other.boardState;
    }

    public TicState GetStateAtPoint(int row, int col)
    {
        return board[row, col];
    }

    public bool IsOpenSqaure(int row, int col)
    {
        return GetStateAtPoint(row, col) == TicState.N;
    }

    public TicState MakeMove(int row, int col, TicState player)
    {
        board[row, col] = player;
        boardState = WinChecker.checkWin(board);
        return boardState;
    }
    
    public string GetRowAsString(int row)
    {   
        if(boardState != TicState.N){
            if(row == 1) return "   "+boardState+"   ";
            return "       ";
        }

        if(isBoardFilled()){
            if(row == 1) return "   "+TicState.T+"   ";
            return "       ";
        }
        
        return " "+board[row, 0]+" "+board[row, 1]+" "+board[row, 2]+" ";
    }

    private bool isBoardFilled(){
        for(int row = 0; row<3; row++){
            for(int col = 0; col<3; col++){
                if(board[row, col] == TicState.N) return false;
            }
        }
        return true;
    }
    public TicState GetBoardState()
    {
        return boardState;
    }

    public TTTBoard CloneBoard()
    {
        return new TTTBoard(this);
    }

    public bool isEqualTo(TTTBoard other){
        for(int row = 0; row<3; row++){
            for(int col = 0; col<3; col++){
                if(board[row, col] != other.board[row, col]) return false;
            }
        }
        return true;
    }
}
