using System.Data.SQLite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MareSounds.Core.Models;

namespace MareSounds.Infrastructure.Services
{
    public class SqliteService
    {
        static string _dbLocation = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\MareSounds\\sqlite.db";

        public static void InitDatabase()
        {
            if (!File.Exists($"{_dbLocation}"))
            {
                SQLiteConnection.CreateFile($"{_dbLocation}");
            }

            SQLiteConnection connection = new($"Data Source={_dbLocation}");

            SQLiteCommand command = connection.CreateCommand();

            command.CommandText = "CREATE TABLE IF NOT EXISTS Mares (Id INTEGER PRIMARY KEY, Name VARCHAR(50), Picture TEXT);" + 
                "CREATE TABLE IF NOT EXISTS VoiceClips (Id INTEGER PRIMARY KEY AUTOINCREMENT, Location TEXT, MareId INTEGER, FOREIGN KEY (MareId) REFERENCES Mares (id));";

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

        }
    }
}
