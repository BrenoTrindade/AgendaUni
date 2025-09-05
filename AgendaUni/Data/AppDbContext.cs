using Microsoft.Data.Sqlite;

public class AppDbContext
{
    private readonly string _connectionString;

    public AppDbContext(string dbPath)
    {
        _connectionString = $"Data Source={dbPath}";
    }

    public SqliteConnection GetConnection()
    {
        return new SqliteConnection(_connectionString);
    }

    // Método para criar uma tabela de exemplo
    public void InitializeDatabase()
    {
        using var connection = GetConnection();
        connection.Open();

        // Criação da tabela Class
        var createClassTable = connection.CreateCommand();
        createClassTable.CommandText = @"
                CREATE TABLE IF NOT EXISTS Class (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ClassName TEXT NOT NULL,
                    MaximumAbsences INTEGER NOT NULL
                )";
        createClassTable.ExecuteNonQuery();

        var createClassScheduleTable = connection.CreateCommand();
        createClassScheduleTable.CommandText = @"
                CREATE TABLE IF NOT EXISTS ClassSchedule (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    DayOfWeek INTEGER NOT NULL,
                    ClassTime TEXT NOT NULL,
                    ClassId INTEGER NOT NULL,
                    FOREIGN KEY (ClassId) REFERENCES Class(Id)
                )";
        createClassScheduleTable.ExecuteNonQuery();

        // Criação da tabela Absence
        var createAbsenceTable = connection.CreateCommand();
        createAbsenceTable.CommandText = @"
                CREATE TABLE IF NOT EXISTS Absence (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    AbsenceDate TEXT NOT NULL,
                    AbsenceReason TEXT NOT NULL,
                    ClassId INTEGER NOT NULL,
                    FOREIGN KEY (ClassId) REFERENCES Class(Id)
                )";

        createAbsenceTable.ExecuteNonQuery();

        // Criação da tabela Event
        var createEventTable = connection.CreateCommand();
        createEventTable.CommandText = @"
                CREATE TABLE IF NOT EXISTS Event (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    EventDate TEXT NOT NULL,
                    Description TEXT NOT NULL,
                    ClassId INTEGER NOT NULL,
                    FOREIGN KEY (ClassId) REFERENCES Class(Id)
                )";

        createEventTable.ExecuteNonQuery();
    }
}