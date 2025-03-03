using Autodesk.Revit.UI;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Shared.Services;
public class SimpleDimpleService(ILogger<SimpleDimpleService> _logger)
{
    public void SayHello()
    {
        TaskDialog.Show("Sample", "Hello world");
    }
}

