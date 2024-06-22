TTTGameState gameState = new TTTGameState();
int defaultTime = 2500;
int time;

Console.Write("Enter AI thinking time in milliseconds: ");
string inputTime = Console.ReadLine();
if (!int.TryParse(inputTime, out time))
{
    Console.WriteLine("Invalid input. Using default thinking time (2500 milliseconds).");
    time = defaultTime;
}

Console.Write("Choose your symbol (X or O): ");
char playerSymbol = Console.ReadLine().ToUpper()[0];
if (playerSymbol != 'X' && playerSymbol != 'O')
{
    Console.WriteLine("Invalid symbol choice. Defaulting to X.");
    playerSymbol = 'X';
}

Console.WriteLine(gameState);

if (playerSymbol == 'O')
    MakeAIMove();

while (true)
{
    BoardLoc playerMove = MakePlayerMove();

    if (gameState.IsLegalMove(playerMove))
    {
        gameState = gameState.MakeMove(playerMove);
        Console.WriteLine(gameState);

        if (HandleWins())
            break;

        MakeAIMove();

        if (HandleWins())
            break;
    }
    else
    {
        Console.WriteLine("Illegal Move");
    }
}

BoardLoc MakePlayerMove()
{
    Console.Write("row:");
    string row = Console.ReadLine();
    
    Console.Write("col: ");
    string col = Console.ReadLine();
    try
    {
        int rowInt = Convert.ToInt32(row);
        int colInt = Convert.ToInt32(col);
        return new BoardLoc(rowInt/3, colInt/3, rowInt%3, colInt%3);
    } 
    catch 
    {
        return MakePlayerMove();
    }
}

bool HandleWins()
{
    TicState boardState = gameState.GetBoardState();

    if (boardState == TicState.N) return false;

    if (boardState == TicState.T)
    {
        Console.WriteLine("It is a tie");
    }
    else
    {
        Console.WriteLine(boardState + " Wins");
    }

    return true;
}

void MakeAIMove()
{
    BoardLoc move = MCTS.FindMove(gameState, time);
    gameState = gameState.MakeMove(move);
    Console.WriteLine(move);
    Console.WriteLine(gameState);
}
