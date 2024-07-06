using System.Text.Json;

namespace PRN231.API.Extensions;

public class KebabCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return string.Concat(name.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x : x.ToString())).ToLower();
    }
}