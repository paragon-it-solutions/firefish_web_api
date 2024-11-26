﻿using Microsoft.Data.SqlClient;

namespace Firefish.Infrastructure.Helpers;

public static class SqlIdentityHelper
{
    // Static method to generate Identity based on last highest ID in db
    public static async Task<int> GenerateIdentityAsync(string table)
    {
        try
        {
            await using var connection = new SqlConnection(SqlConnectionHelper.ConnectionString);
            await connection.OpenAsync();

            await using var command = new SqlCommand(
                $"SELECT MAX(ID) + 1 FROM [{table}]",
                connection
            );

            int identity = (int)await command.ExecuteScalarAsync();
            return identity;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error generating identity for: {ex.Message}", ex);
        }
    }
}
