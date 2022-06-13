namespace Azure.RestApi.CostManagement;

using System.Text.Json;
using System.Text.Json.Serialization;

internal static class Constants
{
    public static readonly JsonSerializerOptions JsonSerializerOptions
            = new()
              {

                        NumberHandling           = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
                      , DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                      , WriteIndented           = true
                    };
    
    public const string DefaultVersion = "2021-10-01";
}