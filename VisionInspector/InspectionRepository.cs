using System.Data;
using Microsoft.Data.Sqlite;

namespace VisionInspector;

public class InspectionRepository
{
    private readonly string _connStr;

    public InspectionRepository(string dbFileName = "inspections.db")
    {
        var path = Path.Combine(AppContext.BaseDirectory, dbFileName);
        _connStr = $"Data Source={path}";
        Init();
    }

    private void Init()
    {
        using var conn = new SqliteConnection(_connStr);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Inspections (
                Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                InspectedAt TEXT    NOT NULL,
                FileName    TEXT    NOT NULL,
                Result      TEXT    NOT NULL,
                DefectCount INTEGER NOT NULL,
                ElapsedMs   INTEGER NOT NULL
            );";
        cmd.ExecuteNonQuery();
    }

    public void Insert(string fileName, bool ok, int count, long ms)
    {
        using var conn = new SqliteConnection(_connStr);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Inspections (InspectedAt, FileName, Result, DefectCount, ElapsedMs)
            VALUES ($at, $file, $result, $count, $ms);";
        cmd.Parameters.AddWithValue("$at", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        cmd.Parameters.AddWithValue("$file", fileName);
        cmd.Parameters.AddWithValue("$result", ok ? "OK" : "NG");
        cmd.Parameters.AddWithValue("$count", count);
        cmd.Parameters.AddWithValue("$ms", ms);
        cmd.ExecuteNonQuery();
    }

    public DataTable GetAll()
    {
        using var conn = new SqliteConnection(_connStr);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT Id, InspectedAt, FileName, Result, DefectCount, ElapsedMs
            FROM Inspections ORDER BY Id DESC LIMIT 200;";
        using var reader = cmd.ExecuteReader();
        var table = new DataTable();
        table.Load(reader);
        return table;
    }
}