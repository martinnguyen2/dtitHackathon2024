using System;

namespace Backend.Common.Models;

public class ResponseDataset
{
    private string myName;

    public required string Name
    {
        get { return myName; }
        set { myName = value; }
    }

    public ResponseDataset()
    {
        myName = string.Empty;
    }
}
