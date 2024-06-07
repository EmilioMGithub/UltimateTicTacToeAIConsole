public class TTTGameState
{
    private TTTBoard[,] board;

    private TicState nextPlayer;

    public TicState GetNextPlayer(){
        return nextPlayer;
    }
    private TicState boardState;

    private int lastRow;
    private int lastCol;

    public TTTGameState()
    {
        board = new TTTBoard[3, 3];
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                board[row, col] = new TTTBoard();
            }
        }

        lastRow = -1;
        lastCol = -1;

        nextPlayer = TicState.X;
        boardState = TicState.N;
    }

    public TTTGameState(TTTGameState other)
    {
        TTTBoard[,] otherBoard = other.board;
        board = new TTTBoard[3, 3];
        for(int row = 0; row < 3; row++)
        {
            for(int col = 0; col < 3; col++)
            {
                board[row, col] = otherBoard[row, col].CloneBoard();
            }
        }

        lastRow = other.lastRow;
        lastCol = other.lastCol;

        nextPlayer = other.nextPlayer;
        boardState = other.boardState;
    }

    public bool isLegalMove(BoardLoc loc)
    {
        bool moveInNextBoard = loc.getOutRow() == lastRow && loc.getOutCol() == lastCol;
        if(!moveInNextBoard && !canMoveAnywhere()) return false;

        bool boardClickedWon = board[loc.getOutRow(), loc.getOutCol()].GetBoardState() != TicState.N;
        bool isOpenSquare = board[loc.getOutRow(), loc.getOutCol()].IsOpenSqaure(loc.getInRow(), loc.getInCol());

        if(boardClickedWon || !isOpenSquare) return false;
        return true;
    }

    private bool canMoveAnywhere(){
        bool isFirstMove = lastRow == -1 && lastCol == -1;

        if(isFirstMove) return true;

        bool boardWon = board[lastRow, lastCol].GetBoardState() != TicState.N;
        bool boardFilled = isBoardFilled(lastRow, lastCol);
        return boardWon || boardFilled;
    }

    private bool isBoardFilled(int row, int col){
        for(int inRow = 0; inRow<3; inRow++){
            for(int inCol = 0; inCol<3; inCol++){
                if(board[row, col].GetStateAtPoint(inRow, inCol) == TicState.N) return false;
            }
        }
        return true;
    }

    public TicState getBoardState()
    {
        return boardState;
    }

    public TicState getStatetAtPoint(BoardLoc loc)
    {
        return board[loc.getOutRow(), loc.getOutCol()].GetStateAtPoint(loc.getInRow(), loc.getInCol());
    }

    public TTTGameState makeMove(BoardLoc loc)
    {
        if(!isLegalMove(loc)) Console.WriteLine("ILLEGAL MOVE MADE");

        TTTGameState newState = new TTTGameState(this)
        {
            lastRow = loc.getInRow(),
            lastCol = loc.getInCol()
        };

        TTTBoard boardOn = newState.board[loc.getOutRow(), loc.getOutCol()];
        boardOn.MakeMove(loc.getInRow(), loc.getInCol(), newState.nextPlayer);

        newState.boardState = TicState.T;
        TicState[,] stateOfBoards = new TicState[3,3];
        for(int row = 0; row < 3; row++)
        {
            for(int col = 0; col < 3; col++)
            {
                bool boardWon = newState.board[row, col].GetBoardState() != TicState.N;
                bool isOpenBoard = !boardWon && !newState.isBoardFilled(row, col);
                if(isOpenBoard) newState.boardState = TicState.N;
                stateOfBoards[row, col] = newState.board[row, col].GetBoardState();
            }
        }

        if(newState.boardState != TicState.T){
            newState.boardState = WinChecker.checkWin(stateOfBoards);
        }

        newState.nextPlayer = newState.nextPlayer == TicState.X ? TicState.O: TicState.X;
        
        return newState;
    }

    public override string ToString()
    {
        string returnValue = "  | 0 1 2 | 3 4 5 | 6 7 8\n";
        returnValue+="--------------------------\n";
        for(int outerRow = 0; outerRow<3; outerRow++){
            for(int row = 0; row<3; row++){
                string left = board[outerRow, 0].GetRowAsString(row);
                string center = board[outerRow, 1].GetRowAsString(row);
                string right = board[outerRow, 2].GetRowAsString(row);  
                returnValue += outerRow*3+row+" |"+left+"|"+center+"|"+right+"\n";
            }
            returnValue+="--------------------------\n";
        }
        returnValue+=$"      Next Player is:{nextPlayer}";
        return returnValue;
    }

    public List<BoardLoc> GetLegalMoves(){
        List<BoardLoc> legalMoves = new List<BoardLoc>();

        if(!canMoveAnywhere()){
            checkInnerMoves(lastRow, lastCol, legalMoves);
            return legalMoves;
        }

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                checkInnerMoves(row, col, legalMoves);
            }
        }
        return legalMoves;
    }
    
    private void checkInnerMoves(int row, int col, List<BoardLoc> legalMoves){
        if(board[row, col].GetBoardState() != TicState.N){
            return;
        }

        for (int innerRow = 0; innerRow < 3; innerRow++){
            for (int innerCol = 0; innerCol < 3; innerCol++)
            {
                var loc = new BoardLoc(row, col, innerRow, innerCol);
                if (isLegalMove(loc)) legalMoves.Add(loc);
            }
        }
    }
}