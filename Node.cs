
public class Node
{
    private static readonly Random random = new Random();

    private TTTGameState state;
    private TicState playingAs;

    private Node? parent;
    private List<Node> children;

    private BoardLoc? moveMade;

    private int visits;
    private int misses;
    private int wins;

    public Node(TTTGameState state, TicState playingAs)
    {
        this.state = state;
        this.playingAs = playingAs;
        children = new List<Node>();
        visits = 0;
        misses = 0;
        wins = 0;
    }

    public Node(Node parent, TTTGameState state, TicState playingAs, BoardLoc moveMade) : this(state, playingAs)
    {
        this.parent = parent;
        this.moveMade = moveMade;
    }

    public void Backpropagate(int result)
    {
        if(result>0) wins += result;
        if(result<0) misses += result;
        visits++;
        if (parent == null) return;
        parent.Backpropagate(result);
    }

    public int Rollout()
    {

        TTTGameState currentState = new TTTGameState(state);
        while (true)
        {
            if (currentState.getBoardState() != TicState.N)
            {
                break;
            }
            List<BoardLoc> moves = currentState.GetLegalMoves();
            if (moves.Count == 0)
            {
                break;
            }
            currentState = currentState.makeMove(moves[random.Next(moves.Count)]);
            }
            if(currentState.getBoardState() == playingAs) return 1;
            if(currentState.getBoardState() == TicState.T) return 0;
            return -1;
        }

    public Node GetBestChild()
    {
        if (children.Count == 0) return this;
        Node bestNode = children[0];
        double bestScore = bestNode.visits;
        for (int index = 1; index < children.Count; index++)
        {
            Node nodeOn = children[index];
            if (nodeOn.visits > bestScore)
            {
                bestNode = nodeOn;
                bestScore = nodeOn.visits;
            }
        }
        return bestNode;
    }

    public BoardLoc GetBestMove()
    {
        if(children.Count == 0){
            return new BoardLoc(-1,-1,-1,-1);
        } 
        return GetBestChild().moveMade;
    }

    private void Expand()
    {
        List<BoardLoc> legalMoves = state.GetLegalMoves();
        foreach (BoardLoc move in legalMoves)
        {
            TTTGameState newState = state.makeMove(move);
            Node childNode = new Node(this, newState, playingAs, move);
            children.Add(childNode);
        }
    }

    public Node Traverse()
    {
        Node node = this;
        Node bestChild = null;
        while (node.children.Count != 0)
        {
            bestChild = node.bestUTC();
            node = bestChild;
        }

        if (node.state.getBoardState() != TicState.N) return node;

        node.Expand();
        return node.children[random.Next(node.children.Count)];
    }

    private Node bestUTC() 
    {
        Node bestChild = children[0];
        double bestUCTValue = double.NegativeInfinity;
        double explorationFactor = Math.Sqrt(2);

        foreach (Node child in children)
        {
            double exploitation = Math.Abs(child.wins) / (child.visits + double.Epsilon);
            double exploration = explorationFactor * Math.Sqrt(Math.Log(this.visits) / (child.visits + double.Epsilon));
            double uctValue = exploitation + exploration;
            if (uctValue > bestUCTValue)
            {
                bestChild = child;
                bestUCTValue = uctValue;
            }
        }
        return bestChild;
    }
}