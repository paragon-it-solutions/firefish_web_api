using Microsoft.Data.SqlClient;

namespace Firefish.Infrastructure.Helpers;

public static class SqlParameterExtensions
{
    /// <summary>
    ///     Adds a parameter to a SqlParameterCollection with a value that can be null.
    ///     If the value is null, it adds DBNull.Value instead.
    /// </summary>
    /// <param name="parameters">The SqlParameterCollection to add the parameter to.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="value">The value of the parameter. Can be null.</param>
    public static void AddWithNullableValue(
        this SqlParameterCollection parameters,
        string parameterName,
        object value
    )
    {
        parameters.AddWithValue(parameterName, value ?? DBNull.Value);
    }
}
