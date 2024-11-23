namespace Backend.Common.Models;

public class Dataset
{
    private string myName;
    private string myPath;

    public required string Name
    {
        get { return myName; }
        set { myName = value; }
    }

    public required string Path
    {
        get { return myPath; }
        set { myPath = value; }
    }

    public Dataset()
    {
        myName = string.Empty;
        myPath = string.Empty;
    }
}
