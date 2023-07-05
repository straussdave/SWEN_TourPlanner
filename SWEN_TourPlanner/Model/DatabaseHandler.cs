
using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SWEN_TourPlanner;
using SWEN_TourPlanner.Model;

public class DatabaseHandler
{
    private readonly TourPlannerDbContext _context;

    MapHandler maphandler = new MapHandler();

    public DatabaseHandler()
    {
        _context = new TourPlannerDbContext();

        if (!_context.Tours.Any())
        {
            _context.Tours.AddRange(
                new Tour
                {
                    Name = "Tour 1",
                    Description = "Description of Tour 1",
                    FromLocation = "Location A",
                    ToLocation = "Location B",
                    TransportType = "Car",
                    TourDistance = 100,
                    EstimatedTime = 120,
                    RouteImage = "route_1.jpg"
                },
                new Tour
                {
                    Name = "Tour 2",
                    Description = "Description of Tour 2",
                    FromLocation = "Location X",
                    ToLocation = "Location Y",
                    TransportType = "Bike",
                    TourDistance = 80,
                    EstimatedTime = 90,
                    RouteImage = "route_2.jpg"
                }
            // Add more sample data as needed
            );
            _context.SaveChanges();
        }
        //AddTour("pöchlarn", "melk", "A carride from pöchlarn to melk", "Pöchlarn-Melk");
        //var date = new DateTime(2023, 7, 4);
        //AddLog(5, date, "war eine angenehme fahrt", 1, 780, 3);
    }

    public Tour AddTour(string fromLocation, string toLocation, string description, string name)
    {
        Tour tour = maphandler.GetRoute(fromLocation, toLocation, description, name);
        _context.Add(tour);
        _context.SaveChanges();
        return tour;
    }

    public Log AddLog(int TourId, DateTime TourDate, string commment, int difficulty, int TotalTime, int Rating) 
    {
        Log log = new Log();
        log.TourId = TourId;
        log.TourDate = TourDate;
        log.Comment = commment;
        log.Difficulty = difficulty;
        log.TotalTime = TotalTime;
        log.Rating = Rating;
        _context.Add(log);
        _context.SaveChanges();
        return log;
    }

    /*
    /// <summary>
    /// ORM Tour Table
    /// </summary>
    public DbSet<Tour> Tours { get; set; }

    /// <summary>
    /// ORM Log Table 
    /// </summary>
    public DbSet<Log> Logs { get; set; }

    /// <summary>
    /// Reads the connection string from config file
    /// </summary>
    /// <returns>returns connection string as string</returns>
    static public string GetConnectionString() 
    {
        return MauiProgram.Services.GetService<IConfiguration>().GetConnectionString("cs");
    }

    /// <summary>
    /// This Constructor creates and populates the Tour and Log tables
    /// </summary>
    public DatabaseHandler()
    {
        CreateTables();
        PopulateTables();
    }

    /// <summary>
    /// Creates Tour and Log tables
    /// </summary>
    private void CreateTables()
    {
        CreateTourTable();
        CreateLogTable();
    }

    /// <summary>
    /// Populates the Tour and Log tables
    /// </summary>
    private void PopulateTables()
    {
        PopulateTourTable();
        PopulateLogTable();
    }

    /// <summary>
    /// Creates the Tour table if it doesn't already exist
    /// </summary>
    private void CreateTourTable()
    {
        if (CheckIfExists("tour"))
        {
            return;
        }
        else
        {
            connection.Open();

            string sql = "CREATE TABLE tour (" +
                "\r\n  id INTEGER PRIMARY KEY," +
                "\r\n  name VARCHAR(50)," +
                "\r\n  description VARCHAR(500)," +
                "\r\n  from_location VARCHAR(50)," +
                "\r\n  to_location VARCHAR(50)," +
                "\r\n  transport_type VARCHAR(50)," +
                "\r\n  tour_distance INTEGER," +
                "\r\n  estimated_time INTEGER," +
                "\r\n  route_image VARCHAR(100)" +
                "\r\n);";

            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }

            connection.Close();
        }

    }

    /// <summary>
    /// Creates the Log Table, if this table does not already exists AND the tour table does exist
    /// </summary>
    private void CreateLogTable()
    {
        if (CheckIfExists("log"))
        {
            return;
        }
        else if (!CheckIfExists("tour"))
        {
            return;
        }
        else
        {
            connection.Open();

            string sql = "CREATE TABLE log (" +
                "\r\n  id INTEGER PRIMARY KEY," +
                "\r\n  tour_id INTEGER," +
                "\r\n  tour_date TIMESTAMP," +
                "\r\n  comment VARCHAR(500)," +
                "\r\n  difficulty INTEGER," +
                "\r\n  total_time INTEGER," +
                "\r\n  rating INTEGER," +
                "\r\n  FOREIGN KEY (tour_id) REFERENCES tour (id)" +
                "\r\n);";

            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    /// <summary>
    /// Populates the Tour Table with Dummy Data
    /// </summary>
    private void PopulateTourTable()
    {
        if (CheckIfExists("tour"))
        {
            if (CheckIfAlreadyPopulated("tour"))
            {
                return;
            }
            connection.Open();

            string sql = "INSERT INTO tour " +
                "(id, name, description, from_location, to_location, transport_type, tour_distance, estimated_time, route_image)" +
                "\r\nVALUES" +
                "\r\n  (1, 'Exploring Nature', 'Embark on a journey to explore the wonders of nature', 'City A', 'City B', 'Hiking', 10, 120, 'route_image_1.jpg')," +
                "\r\n  (2, 'Historical Tour', 'Discover the rich history of the region with this guided tour', 'City C', 'City D', 'Bus', 50, 180, 'route_image_2.jpg')," +
                "\r\n  (3, 'Coastal Adventure', 'Experience the thrill of water sports along the beautiful coastline', 'City E', 'City F', 'Boat', 25, 90, 'route_image_3.jpg');";

            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    /// <summary>
    /// Populates the Log Table with Dummy Data
    /// </summary>
    private void PopulateLogTable()
    {
        if (CheckIfExists("log"))
        {
            if (CheckIfAlreadyPopulated("log"))
            {
                return;
            }
            connection.Open();

            string sql = "INSERT INTO log" +
                "(id, tour_id, tour_date, comment, difficulty, total_time, rating)" +
                "\r\nVALUES\r\n  (1, 1, '2023-06-01 09:00:00', 'The hike was breathtaking. Amazing views!', 3, 120, 4)," +
                "\r\n  (2, 1, '2023-06-05 14:30:00', 'The trail was challenging but totally worth it.', 4, 150, 5)," +
                "\r\n  (3, 2, '2023-06-10 10:15:00', 'Fascinating insights into the historical landmarks.', 2, 90, 4)," +
                "\r\n  (4, 2, '2023-06-12 11:30:00', 'The tour guide was extremely knowledgeable.', 3, 120, 4)," +
                "\r\n  (5, 3, '2023-06-15 14:00:00', 'Had a great time kayaking along the coastline!', 2, 75, 5);";

            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    /// <summary>
    /// Checks if a given table exists
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns>true if the table exists, false if the table does not exist</returns>
    private bool CheckIfExists(string tableName)
    {
        connection.Open();

        string sql = "SELECT *\r\nFROM pg_catalog.pg_tables\r\nWHERE tablename = @tableName;";

        using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("tableName", tableName);
            using (NpgsqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    connection.Close();
                    return true;
                }
            }
        }

        connection.Close();
        return false;
    }

    /// <summary>
    /// Checks if a given table is populated
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns>true if the table is populated with any data, false if it isn't</returns>
    private bool CheckIfAlreadyPopulated(string tableName)
    {
        tableName = SanitizeString(tableName);
        connection.Open();

        string sql = "SELECT COUNT(*) FROM " + tableName + ";"; //I am aware that it's bad practice to include the parameter as string literal, but I'm quite sure that it is save in this case because the values don't come from user input, but to at least have one safety net I used the CleanString function

        using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("tableName", tableName);
            int rowCount = Convert.ToInt32(command.ExecuteScalar());

            if (rowCount > 0)
            {
                connection.Close();
                return true;
            }
            else
            {
                connection.Close();
                return false;
            }
        }

    }

    /// <summary>
    /// sanitizes a string using Regex (by MSDN http://msdn.microsoft.com/en-us/library/844skk0h(v=vs.71).aspx)
    /// </summary>
    /// <param name="strIn"></param>
    /// <returns>sanitized string</returns>
    public static string SanitizeString(string strIn)
    {
        // Replace invalid characters with empty strings.
        return Regex.Replace(strIn, @"[^\w\.@-]", "");
    } 
    */
}