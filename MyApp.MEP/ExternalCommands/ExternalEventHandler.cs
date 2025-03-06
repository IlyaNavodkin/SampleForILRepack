using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace MyApp.MEP.ExternalCommands;

public class ExternalEventHandler : IExternalEventHandler
{
    private ExternalEvent _externalEvent;
    private LinesCollectionDto _linesDto;
    private HttpRestService _httpRestService;

    public ExternalEventHandler(HttpRestService httpRestService)
    {
        _httpRestService = httpRestService;
        _externalEvent = ExternalEvent.Create(this);
    }

    public void SetLines(List<LineDto> lines)
    {
        _linesDto = new LinesCollectionDto { Lines = new List<LineDto>(lines) };
        _externalEvent.Raise();
    }

    public void Execute(UIApplication app)
    {
        var json = _linesDto.ToJson();
        TaskDialog.Show("Линии Получены", _linesDto.ToJson());

        _httpRestService.PushHttpMessage(json);
    }

    public string GetName() => "ExternalEventHandler";
}