public class TTTGameState
{
    private readonly TTTBoard[,] board;

    private TicState nextPlayer;
    private TicState boardState;

    private int lastRow;
    private int lastCol;

    public TicState GetBoardState() => boardState;
    public TicState GetNextPlayer() => nextPlayer;

    public TTTGameState()
    {
        board = new TTTBoard[3, 3];
        TicUtils.GridTraverse((row, col) => board[row, col] = new TTTBoard());

        lastRow = -1;
        lastCol = -1;

        nextPlayer = TicState.X;
        boardState = TicState.N;
    }

    public TTTGameState(TTTGameState other)
    {
        TTTBoard[,] otherBoard = other.board;
        board = new TTTBoard[3, 3];
        TicUtils.GridTraverse((row, col) => board[row, col] = otherBoard[row, col].CloneBoard());

        lastRow = other.lastRow;
        lastCol = other.lastCol;

        nextPlayer = other.nextPlayer;
        boardState = other.boardState;
    }
    
    public bool IsLegalMove(BoardLoc loc)
    {
        bool moveInNextBoard = loc.GetOutRow() == lastRow && loc.GetOutCol() == lastCol;
        if(!moveInNextBoard && !CanMoveAnywhere()) return false;

        bool boardClickedWon = board[loc.GetOutRow(), loc.GetOutCol()].GetBoardState() != TicState.N;
        bool isOpenSquare = board[loc.GetOutRow(), loc.GetOutCol()].IsOpenSqaure(loc.GetInRow(), loc.GetInCol());

        if(boardClickedWon || !isOpenSquare) return false;
        return true;
    }

    private bool CanMoveAnywhere()
    {
        bool isFirstMove = lastRow == -1 && lastCol == -1;

        if(isFirstMove) return true;

        bool boardWon = board[lastRow, lastCol].GetBoardState() != TicState.N;
        bool boardFilled = IsBoardFilled(lastRow, lastCol);
        return boardWon || boardFilled;
    }

    private bool IsBoardFilled(int row, int col)
    {
        for(int inRow = 0; inRow<3; inRow++)
        {
            for(int inCol = 0; inCol<3; inCol++)
            {
                if(board[row, col].GetStateAtPoint(inRow, inCol) == TicState.N) return false;
            }
        }
        return true;
    }

    public TTTGameState MakeMove(BoardLoc loc)
    {
        TTTGameState newState = new TTTGameState(this)
        {
            lastRow = loc.GetInRow(),
            lastCol = loc.GetInCol()
        };

        TTTBoard boardOn = newState.board[loc.GetOutRow(), loc.GetOutCol()];
        boardOn.MakeMove(loc.GetInRow(), loc.GetInCol(), newState.nextPlayer);

        newState.boardState = TicState.T;
        TicState[,] stateOfBoards = new TicState[3,3];
        TicUtils.GridTraverse((row, col) => {
            bool boardWon = newState.board[row, col].GetBoardState() != TicState.N;
            bool isOpenBoard = !boardWon && !newState.IsBoardFilled(row, col);
            if(isOpenBoard) newState.boardState = TicState.N;
            stateOfBoards[row, col] = newState.board[row, col].GetBoardState();
        });

        if(newState.boardState != TicState.T)
        {
            newState.boardState = TicUtils.checkWin(stateOfBoards);
        }

        newState.nextPlayer = newState.nextPlayer == TicState.X ? TicState.O: TicState.X;
        
        return newState;
    }

    public List<BoardLoc> GetLegalMoves()
    {
        List<BoardLoc> legalMoves = new List<BoardLoc>();

        if(!CanMoveAnywhere())
        {
            CheckInnerMoves(lastRow, lastCol, legalMoves);
            return legalMoves;
        }

        TicUtils.GridTraverse((row, col) => {
            CheckInnerMoves(row, col, legalMoves);  
        });
            
        return legalMoves;
    }
    
    private void CheckInnerMoves(int row, int col, List<BoardLoc> legalMoves){
        if(board[row, col].GetBoardState() != TicState.N) return;

        TicUtils.GridTraverse((innerRow, innerCol) => {
            var loc = new BoardLoc(row, col, innerRow, innerCol);
            if (IsLegalMove(loc)) legalMoves.Add(loc);
        });
    }

    public override string ToString()
    {
        string returnValue = "  | 0 1 2 | 3 4 5 | 6 7 8\n";
        returnValue+="--------------------------\n";
        for(int outerRow = 0; outerRow<3; outerRow++)
        {
            for(int row = 0; row<3; row++)
            {
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
}