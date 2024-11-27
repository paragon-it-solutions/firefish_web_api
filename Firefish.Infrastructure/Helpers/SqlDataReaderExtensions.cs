using Microsoft.Data.SqlClient;

namespace Firefish.Infrastructure.Helpers
{
    // Helper extension method
    public static class SqlDataReaderExtensions
    {
        /// <summary>
        ///     Retrieves a nullable string value from a SqlDataReader for the specified field.
        /// </summary>
        /// <param name="reader">The SqlDataReader containing the data.</param>
        /// <param name="fieldName">The name of the field to retrieve the value from.</param>
        /// <returns>
        ///     The string value of the specified field if it exists and is not null;
        ///     null if the field is null; or an empty string if the field does not exist.
        /// </returns>
        public static string GetNullableString(this SqlDataReader reader, string fieldName)
        {
            int ordinal = reader.GetOrdinal(fieldName);
            return (reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal)) ?? string.Empty;
        }
    }
}
