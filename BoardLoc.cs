public class BoardLoc
{
    private int outRow;
    private int outCol;
    private int inRow;
    private int inCol;

    public BoardLoc(int outRow, int outCol, int inRow, int inCol)
    {
        this.outRow = outRow;
        this.outCol = outCol;
        this.inRow = inRow;
        this.inCol = inCol;
    }
    
    public int getOutRow()
    { 
        return outRow; 
    }

    public int getOutCol()
    {
        return outCol;
    }

    public int getInRow()
    {
        return inRow;
    }

    public int getInCol()
    {
        return inCol;
    }

    public override string ToString(){
        return "Row:"+(outRow*3+inRow)+" Col:"+(outCol*3+inCol);
    }
}
