
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
        _context.Remove(tour);
        _context.SaveChanges();
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
}