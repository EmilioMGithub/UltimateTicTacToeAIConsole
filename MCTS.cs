public class MCTS{
    public static BoardLoc FindMove(TTTGameState state, int timeMiliSeconds){
        Thread timeThread = new Thread(new ThreadStart(()=>Thread.Sleep(timeMiliSeconds)));
        timeThread.Start();

        Node rootNode = new Node(state, state.GetNextPlayer());//, simCount);//, stateToNode, false);

        int simulationsDone = 0;
        while(timeThread.IsAlive){
            Node leaf = rootNode.Traverse();
            int simulationResult = leaf.Rollout();
            leaf.Backpropagate(simulationResult);
            simulationsDone++;
        }
        Console.WriteLine(simulationsDone);

        return rootNode.GetBestMove();
    }
}