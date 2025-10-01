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
    public void AddOwner(string name, string phoneNumber)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var commandForInsert = connection.CreateCommand();
            commandForInsert.CommandText = @"
                INSERT INTO Owners (name, phoneNumber)
                VALUES ($name, $phoneNumber);";
            commandForInsert.Parameters.AddWithValue("$name", name);
            commandForInsert.Parameters.AddWithValue("$phoneNumber", phoneNumber);

            commandForInsert.ExecuteNonQuery();
        }
    }
    public void AddPet(string name, int ownerId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var commandForInsert = connection.CreateCommand();
            commandForInsert.CommandText = @"
                INSERT INTO Pets (name, ownerId)
                VALUES ($name, $ownerId);";
            commandForInsert.Parameters.AddWithValue("$name", name);
            commandForInsert.Parameters.AddWithValue("$ownerId", ownerId);

            commandForInsert.ExecuteNonQuery();
        }
    }
    public void UpdateOwnerPhoneNumber(int ownerId, string newPhoneNumber)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var commandForUpdate = connection.CreateCommand();
            commandForUpdate.CommandText = @"
                UPDATE Owners
                SET phoneNumber = $newPhoneNumber
                WHERE id = $ownerId;";
            commandForUpdate.Parameters.AddWithValue("$newPhoneNumber", newPhoneNumber);
            commandForUpdate.Parameters.AddWithValue("$ownerId", ownerId);

            commandForUpdate.ExecuteNonQuery();
        }
    }
    public string? SearchOwnersPhoneNumberByPet(string petname)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var commandForSearch = connection.CreateCommand();
            commandForSearch.CommandText = @"
                SELECT Owners.phoneNumber
                FROM Owners
                JOIN Pets ON Owners.id = Pets.ownerId
                WHERE Pets.name = $petname;";
            commandForSearch.Parameters.AddWithValue("$petname", petname);

            using (var reader = commandForSearch.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetString(0);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
