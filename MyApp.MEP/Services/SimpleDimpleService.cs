using Autodesk.Revit.UI;
using Autodesk.Windows;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace MyApp.MEP.Services;
public class SimpleDimpleService(ILogger _logger)
{
    public void SayHello()
    {
        TaskDialog.Show("Sample", "Hello world");
    }
}

