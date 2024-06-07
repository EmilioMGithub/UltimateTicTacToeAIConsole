TTTGameState gameState = new TTTGameState();
Console.WriteLine(gameState);

//Feel free to change this value to increase the diffculty of the AI
int time = 2500;

while(true){
    BoardLoc playerMove = MakePlayerMove();
    if(gameState.isLegalMove(playerMove)){
        gameState = gameState.makeMove(playerMove);
        Console.WriteLine(gameState);
        if(HandelWins()) break;
        MakeAIMove();
        if(HandelWins()) break;
    } else {
        Console.WriteLine("Illegal Move");
    }
}

BoardLoc MakePlayerMove(){
    Console.Write("row:");
    string row = Console.ReadLine();
    
    Console.Write("col: ");
    string col = Console.ReadLine();
    try{
        int rowInt = Convert.ToInt32(row);
        int colInt = Convert.ToInt32(col);
        return new BoardLoc(rowInt/3, colInt/3, rowInt%3, colInt%3);
    } catch {
        return MakePlayerMove();
    }
}

bool HandelWins(){
    if(gameState.getBoardState() == TicState.N) return false;
    if(gameState.getBoardState() == TicState.T){
        Console.WriteLine("It is a tie");
        return true;
    }
    Console.WriteLine(gameState.getBoardState()+" Wins");
    return true;
}

void MakeAIMove(){
    BoardLoc move = MCTS.FindMove(gameState, time);
    gameState = gameState.makeMove(move);
    Console.WriteLine(move.ToString());
    Console.WriteLine(gameState);
}