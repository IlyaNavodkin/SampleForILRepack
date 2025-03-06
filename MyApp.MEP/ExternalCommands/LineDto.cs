using Autodesk.Revit.DB;

namespace MyApp.MEP.ExternalCommands;

public class LineDto
{
    public XYZ StartPoint { get; set; }
    public XYZ EndPoint { get; set; }

    public LineDto(XYZ start, XYZ end)
    {
        StartPoint = start;
        EndPoint = end;
    }
}
