
using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Xml.Linq;
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
            PopulateDatabase();
        }
    }

    /// <summary>
    /// Gets all tours from the Database
    /// </summary>
    /// <returns>List of all tours</returns>
    public List<Tour> ReadTours()
    {
        return _context.Tours.ToList();
    }

    /// <summary>
    /// Gets a single tour entry from the Database
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Tour object</returns>
    public Tour ReadTour(int id)
    {
        return _context.Tours.Where(x => x.Id == id).FirstOrDefault();
    }

    /// <summary>
    /// Creates new Tour in the Database
    /// </summary>
    /// <param name="fromLocation"></param>
    /// <param name="toLocation"></param>
    /// <param name="description"></param>
    /// <param name="name"></param>
    /// <returns>Newly created tour</returns>
    public Tour CreateTour(string fromLocation, string toLocation, string description, string name)
    {
        Tour tour = maphandler.GetRoute(fromLocation, toLocation, description, name);
        _context.Add(tour);
        _context.SaveChanges();
        return tour;
    }

    /// <summary>
    /// Updates a single Tour entry in the Database
    /// </summary>
    /// <param name="id"></param>
    /// <param name="fromLocation"></param>
    /// <param name="toLocation"></param>
    /// <param name="description"></param>
    /// <param name="name"></param>
    /// <returns>Updated Tour or null if Id was not found</returns>
    public Tour UpdateTour(int id, string fromLocation, string toLocation, string description, string name)
    {
        if (_context.Tours.Where(x => x.Id == id).FirstOrDefault() != default)
        {
            var tour = _context.Update(_context.Tours.Where(x => x.Id == id).FirstOrDefault());
            Tour newTour = maphandler.GetRoute(fromLocation, toLocation, description, name);
            tour.Entity.FromLocation = fromLocation;
            tour.Entity.ToLocation = toLocation;
            tour.Entity.Description = description;
            tour.Entity.Name = name;
            tour.Entity.TourDistance = newTour.TourDistance;
            tour.Entity.TransportType = newTour.TransportType;
            tour.Entity.EstimatedTime = newTour.EstimatedTime;
            tour.Entity.RouteImage = newTour.RouteImage;
            _context.SaveChanges();
            return newTour;
        }
        return null;
    }

    /// <summary>
    ///  Updates a single Tour entry in the Database
    /// </summary>
    /// <param name="oldTour"></param>
    /// <param name="fromLocation"></param>
    /// <param name="toLocation"></param>
    /// <param name="description"></param>
    /// <param name="name"></param>
    /// <returns>Updated Tour</returns>
    public Tour UpdateTour(Tour oldTour, string fromLocation, string toLocation, string description, string name)
    {
        var newTour = _context.Update(oldTour);
        Tour tour = maphandler.GetRoute(fromLocation, toLocation, description, name);
        newTour.Entity.FromLocation = fromLocation;
        newTour.Entity.ToLocation = toLocation;
        newTour.Entity.Description = description;
        newTour.Entity.Name = name;
        newTour.Entity.TourDistance = tour.TourDistance;
        newTour.Entity.TransportType = tour.TransportType;
        newTour.Entity.EstimatedTime = tour.EstimatedTime;
        newTour.Entity.RouteImage = tour.RouteImage;
        _context.SaveChanges();
        return tour;
    }

    /// <summary>
    /// Deletes a single entry in Tour table
    /// </summary>
    /// <param name="id"></param>
    public void DeleteTour(int id)
    {
        if (_context.Tours.Where(x => x.Id == id).FirstOrDefault() != default)
        {
            Tour tour = ReadTour(id);
            DeleteImage(tour.RouteImage);
            _context.Remove(_context.Tours.Where(x => x.Id == id).FirstOrDefault());
            _context.SaveChanges();
        }
    }

    /// <summary>
    /// Deletes a single entry in Tour table
    /// </summary>
    /// <param name="tour"></param>
    public void DeleteTour(Tour tour)
    {
        DeleteImage(tour.RouteImage);
        _context.Remove(tour);
        _context.SaveChanges();
    }

    /// <summary>
    /// Deletes the image belonging to a tour
    /// </summary>
    /// <param name="fileName"></param>
    private void DeleteImage(string fileName)
    {
        string workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string path = Directory.GetParent(workingDirectory).Parent.Parent.Parent.Parent.Parent.FullName + "\\Images\\" + fileName;
        File.Delete(path);
    }

    /// <summary>
    /// Gets a list of all Logs in the Database
    /// </summary>
    /// <returns>List of Logs</returns>
    public List<Log> ReadLogs()
    {
        return _context.Logs.ToList();
    }

    /// <summary>
    /// Gets a single Log entry from the Database
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Log ReadLog(int id)
    {
        return _context.Logs.Where(x => x.Id == id).FirstOrDefault();
    }

    /// <summary>
    /// Creates new Tour Log, if Tour with param tourId exists
    /// </summary>
    /// <param name="tourId"></param>
    /// <param name="tourDate"></param>
    /// <param name="comment"></param>
    /// <param name="difficulty"></param>
    /// <param name="TotalTime"></param>
    /// <param name="Rating"></param>
    /// <returns>Newly created Log, null if tourId did not exist</returns>
    public Log CreateLog(int tourId, DateTime tourDate, string comment, int difficulty, int TotalTime, int Rating)
    {
        if (_context.Tours.Where(x => x.Id == tourId).FirstOrDefault() != default)
        {
            Log log = new Log();
            log.TourId = tourId;
            log.TourDate = tourDate;
            log.Comment = comment;
            log.Difficulty = difficulty;
            log.TotalTime = TotalTime;
            log.Rating = Rating;
            _context.Add(log);
            _context.SaveChanges();
            return log;
        }
        return null;
    }

    /// <summary>
    /// Updates single Tour Log entry
    /// </summary>
    /// <param name="id"></param>
    /// <param name="tourDate"></param>
    /// <param name="comment"></param>
    /// <param name="difficulty"></param>
    /// <param name="totalTime"></param>
    /// <param name="rating"></param>
    public void UpdateLog(int id, DateTime tourDate, string comment, int difficulty, int totalTime, int rating)
    {
        if (_context.Logs.Where(x => x.Id == id).FirstOrDefault() != default)
        {
            var log = _context.Update(_context.Logs.Where(x => x.Id == id).FirstOrDefault());
            log.Entity.TourDate = tourDate;
            log.Entity.Comment = comment;
            log.Entity.Difficulty = difficulty;
            log.Entity.TotalTime = totalTime;
            log.Entity.Rating = rating;
            _context.SaveChanges();
        }
    }

    /// <summary>
    /// Updates single Tour Log entry
    /// </summary>
    /// <param name="oldLog"></param>
    /// <param name="tourDate"></param>
    /// <param name="comment"></param>
    /// <param name="difficulty"></param>
    /// <param name="totalTime"></param>
    /// <param name="rating"></param>
    public void UpdateLog(Log oldLog, DateTime tourDate, string comment, int difficulty, int totalTime, int rating)
    {
        var log = _context.Update(oldLog);
        log.Entity.TourDate = tourDate;
        log.Entity.Comment = comment;
        log.Entity.Difficulty = difficulty;
        log.Entity.TotalTime = totalTime;
        log.Entity.Rating = rating;
        _context.SaveChanges();
    }

    /// <summary>
    /// Deletes a Log from the Database
    /// </summary>
    /// <param name="id"></param>
    public void DeleteLog(int id)
    {
        if (_context.Logs.Where(x => x.Id == id).FirstOrDefault() != default)
        {
            _context.Remove(_context.Logs.Where(x => x.Id == id).FirstOrDefault());
            _context.SaveChanges();
        }
    }

    /// <summary>
    /// Deletes a Log from the Database
    /// </summary>
    /// <param name="log"></param>
    public void DeleteLog(Log log)
    {
        _context.Remove(log);
        _context.SaveChanges();
    }

    /// <summary>
    /// Populates the Databse with realistic dummy data
    /// </summary>
    private void PopulateDatabase()
    {
        Tour tour1 = CreateTour("Pöchlarn", "Vienna", "A carride from Pöchlarn to Vienna", "Pöchlarn-Vienna");
        Tour tour2 = CreateTour("New York", "Miami", "A carride from New York to Miami", "New York-Miami");
        Tour tour3 = CreateTour("Hamburg", "Berlin", "A carride from Hamburg to Berlin", "Hamburg-Berlin");

        CreateLog(
            tourId: tour1.Id,
            tourDate: new DateTime(2023, 7, 4),
            comment: "The weather was perfect for a scenic drive from Pöchlarn to Vienna. We enjoyed beautiful landscapes and historic landmarks along the way.",
            difficulty: 2,
            TotalTime: 3600,
            Rating: 4
            );
        CreateLog(
            tourId: tour1.Id,
            tourDate: new DateTime(2023, 8, 12),
            comment: "We decided to take a detour and explore the Wachau Valley. The vineyards and the Danube River were simply breathtaking.",
            difficulty: 2,
            TotalTime: 5000,
            Rating: 5
            );

        CreateLog(
            tourId: tour2.Id,
            tourDate: new DateTime(2023, 6, 21),
            comment: "Our road trip from New York to Miami was filled with excitement and adventure. We encountered diverse landscapes, vibrant cities, and stunning coastal views.",
            difficulty: 3,
            TotalTime: 43200,
            Rating: 5
            );
        CreateLog(
            tourId: tour2.Id,
            tourDate: new DateTime(2023, 7, 2),
            comment: "We made a pit stop at the Kennedy Space Center and witnessed a rocket launch. It was an unforgettable experience!",
            difficulty: 3,
            TotalTime: 50000,
            Rating: 5
            );


        CreateLog(
            tourId: tour3.Id,
            tourDate: new DateTime(2023, 9, 10),
            comment: "The journey from Hamburg to Berlin was smooth and enjoyable. We explored picturesque countryside and experienced the vibrant atmosphere of Berlin.",
            difficulty: 1,
            TotalTime: 14400,
            Rating: 4
            );
        CreateLog(
            tourId: tour3.Id,
            tourDate: new DateTime(2023, 10, 5),
            comment: "We took a scenic route along the Baltic Sea coastline and enjoyed stunning views of the beaches and charming seaside towns.",
            difficulty: 1,
            TotalTime: 16000,
            Rating: 4
            );
    }
}