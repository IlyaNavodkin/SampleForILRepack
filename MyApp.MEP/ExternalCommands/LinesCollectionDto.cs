using System.Collections.Generic;
using Newtonsoft.Json;

namespace MyApp.MEP.ExternalCommands;

public class LinesCollectionDto
{
    public List<LineDto> Lines { get; set; } = new List<LineDto>();

    public string ToJson() => JsonConvert.SerializeObject(this, Formatting.Indented);
}
