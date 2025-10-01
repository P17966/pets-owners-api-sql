using Microsoft.Data.Sqlite;

namespace PetsOwnersApiSql;

public record Owner(int Id, string Name, string PhoneNumber);
public record Pet(int Id, string Name, int OwnerId);

public class PetsDB
{
    private static string _connectionString = "Data Source=pets.db";

    public PetsDB()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var commandForCreateOwnersTable = connection.CreateCommand();
            commandForCreateOwnersTable.CommandText = @"
                CREATE TABLE IF NOT EXISTS Owners (
                    id INTEGER PRIMARY KEY,
                    name TEXT,
                    phoneNumber TEXT);";
            commandForCreateOwnersTable.ExecuteNonQuery();

            var commandForCreatePetsTable = connection.CreateCommand();
            commandForCreatePetsTable.CommandText = @"
                CREATE TABLE IF NOT EXISTS Pets (
                    id INTEGER PRIMARY KEY,
                    name TEXT,
                    ownerId INTERGER FOREIGN KEY REFERENCES Owners(id));";
            commandForCreatePetsTable.ExecuteNonQuery();
        }
    }
}
