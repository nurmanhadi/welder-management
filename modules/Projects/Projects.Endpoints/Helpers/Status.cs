using System.Text.Json.Serialization;

namespace Projects.Endpoints.Helpers;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Status
{
    Draft = 1,
    Survey = 2,
    Negotiation = 3,
    Approved = 4,
    InProgres = 5,
    Finished = 6,
    Cencelled = 7
}
