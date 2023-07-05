using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Net.Http.Headers;
using System.Reflection;
using static SWEN_TourPlanner.Model.RouteResponse;
using static System.Net.WebRequestMethods;
using SixLabors.ImageSharp; //using this because it is compatible with windows and mac
using SWEN_TourPlanner.Model;

public class MapHandler
{
    HttpClient client = new HttpClient();
    string key;
    int imageWidth;
    int imageHeight;

    /// <summary>
    /// constuctor initializes variables and sets HttpClient base address
    /// </summary>
    public MapHandler()
    {
        InitializeMapHandler();
        client.BaseAddress = new Uri("https://www.mapquestapi.com");
    }

    /// <summary>
    /// makes two requests to mapquest api to get the route and its image, stores the info in tour object and returns it
    /// </summary>
    /// <param name="fromLocation"></param>
    /// <param name="toLocation"></param>
    /// <returns>tour object</returns>
    public Tour GetRoute(string fromLocation, string toLocation, string description, string name)
    {
        //make request to https://www.mapquestapi.com/directions/v2/route?key=KEY&from=FROM&to=TO&outFormat=json&ambiguities=ignore&routeType=fastest&doReverseGeocode=false&enhancedNarrative=false&avoidTimedConditions=false
        Root root = GetRouteAsync(fromLocation, toLocation).Result;

        //get map image from https://www.mapquestapi.com/staticmap/v5/map?key=KEY&session=SESSION
        string sessionId = root.route.sessionId;
        Image<Rgba32> mapImage = new(imageWidth, imageHeight);
        mapImage = GetRouteImageAsync(sessionId).Result;

        //save image to local file system
        string workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.Parent.Parent.Parent.FullName + "\\Images\\";
        var uniqueFilename = string.Format(@"{0}.jpg", Guid.NewGuid());
        mapImage.SaveAsJpeg(projectDirectory + uniqueFilename);

        //build tour object from response data
        Tour tour = new Tour();
        tour.FromLocation = fromLocation;
        tour.ToLocation = toLocation;
        tour.TransportType = root.route.legs[0].maneuvers[0].transportMode;
        tour.TourDistance = (int) Math.Round(root.route.distance * 1.609);
        tour.EstimatedTime = root.route.realTime;
        tour.RouteImage = uniqueFilename;
        tour.Description = description;
        tour.Name = name;

        return tour;
    }

    /// <summary>
    /// get route
    /// </summary>
    /// <param name="fromLocation"></param>
    /// <param name="toLocation"></param>
    /// <returns>route task</returns>
    private async Task<Root> GetRouteAsync(string fromLocation, string toLocation)
    {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        Root root = null;
        HttpResponseMessage response = client.GetAsync(BuildRouteEndpoint(fromLocation, toLocation)).Result;
        if (response.IsSuccessStatusCode)
        {
            root = await response.Content.ReadAsAsync<Root>();
        }
        return root;
    }

    /// <summary>
    /// concatinates the base address, key, fromlocation and to locatin to build the endpoint
    /// </summary>
    /// <param name="fromLocation"></param>
    /// <param name="toLocation"></param>
    /// <returns>string with endpoint</returns>
    private string BuildRouteEndpoint(string fromLocation, string toLocation)
    {
        string endpoint = client.BaseAddress.ToString() + 
            "directions/v2/route?key=" + key +
            "&from=" + fromLocation +
            "&to=" + toLocation +
            "&outFormat=json&ambiguities=ignore&routeType=fastest&doReverseGeocode=false&enhancedNarrative=false&avoidTimedConditions=false";
        return endpoint;
    }

    /// <summary>
    /// uses sessionId from previous call to get the image from the api
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns>image</returns>
    private async Task<Image<Rgba32>> GetRouteImageAsync(string sessionId)
    {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/jpeg"));
        HttpResponseMessage response = client.GetAsync(BuildRouteImageEndpoint(sessionId)).Result;
        response.EnsureSuccessStatusCode(); // Ensure the response is successful

        byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

        using (MemoryStream memoryStream = new MemoryStream(imageBytes))
        {
            Image<Rgba32> mapImage = SixLabors.ImageSharp.Image.Load<Rgba32>(memoryStream);
            return mapImage;
        }
    }

    /// <summary>
    /// build the endpoint for image retrieval
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    private string BuildRouteImageEndpoint(string sessionId)
    {
        string endpoint = client.BaseAddress.ToString() + "staticmap/v5/map?key=" + key + "&session=" + sessionId;
        return endpoint;
    }

    /// <summary>
    /// sets key, imagewidth and imageheight with values from config file
    /// </summary>
    private void InitializeMapHandler()
    {
        var a = Assembly.GetExecutingAssembly();
        using var stream = a.GetManifestResourceStream("SWEN_TourPlanner.appsettings.json");

        var config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();
        key = GetKey(config);
        imageWidth = Int32.Parse(GetWidth(config));
        imageHeight = Int32.Parse(GetHeight(config));
    }

    /// <summary>
    /// gets key from config file
    /// </summary>
    /// <param name="config"></param>
    /// <returns>key string</returns>
    private static string GetKey(IConfigurationRoot config)
    {
        return config.GetSection("MapHandlerSettings")["key"];
    }

    /// <summary>
    /// gets width from config file
    /// </summary>
    /// <param name="config"></param>
    /// <returns>width string</returns>
    private static string GetWidth(IConfigurationRoot config)
    {
        return config.GetSection("MapHandlerSettings")["width"];
    }

    /// <summary>
    /// gets height from config file
    /// </summary>
    /// <param name="config"></param>
    /// <returns>height string</returns>
    private static string GetHeight(IConfigurationRoot config)
    {
        return config.GetSection("MapHandlerSettings")["height"];
    }
}
