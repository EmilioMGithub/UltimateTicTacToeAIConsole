public class BoardLoc
{
    private readonly int outRow;
    private readonly int outCol;
    private readonly int inRow;
    private readonly int inCol;

    public BoardLoc(int outRow, int outCol, int inRow, int inCol)
    {
        this.outRow = outRow;
        this.outCol = outCol;
        this.inRow = inRow;
        this.inCol = inCol;
    }
    
    public int GetOutRow() => outRow; 
    public int GetOutCol() => outCol;
    public int GetInRow() => inRow;
    public int GetInCol() => inCol;
    
    public override string ToString()
    {
        return "Row:"+(outRow*3+inRow)+" Col:"+(outCol*3+inCol);
    }
}
