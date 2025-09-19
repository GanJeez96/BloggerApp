namespace BloggerApp.Tests.Unit.Infrastructure.Helpers;

public static class TestHelpers
{
    public static bool HasParameter(object parameters, string name, object expected)
    {
        var value = parameters.GetType().GetProperty(name)?.GetValue(parameters);
        return Equals(value, expected);
    }
}