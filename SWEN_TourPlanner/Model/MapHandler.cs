using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Net.Http.Headers;
using System.Reflection;
using static SWEN_TourPlanner.Model.RouteResponse;
using static System.Net.WebRequestMethods;
using SixLabors.ImageSharp; //using this because it is compatible with windows and mac

public class MapHandler
{
    HttpClient client = new HttpClient();
    string key;
    int imageWidth;
    int imageHeight;

    public MapHandler()
    {
        InitializeMapHandler();
        client.BaseAddress = new Uri("https://www.mapquestapi.com");
        
        GetRoute("pöchlarn", "stpolten");
    }

    void GetRoute(string fromLocation, string toLocation)
    {
        //make request to https://www.mapquestapi.com/directions/v2/route?key=KEY&from=FROM&to=TO&outFormat=json&ambiguities=ignore&routeType=fastest&doReverseGeocode=false&enhancedNarrative=false&avoidTimedConditions=false
        Root root = GetRouteAsync(fromLocation, toLocation).Result;
        //get map image from https://www.mapquestapi.com/staticmap/v5/map?key=KEY&session=SESSION
        string sessionId = root.route.sessionId;
        Image<Rgba32> mapImage = new(imageWidth, imageHeight);
        mapImage = GetRouteImageAsync(sessionId).Result;
        //save image to local file system
        string workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
        Trace.WriteLine("working dir: " + workingDirectory);
        string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.Parent.Parent.Parent.FullName + "\\Images\\";
        Trace.WriteLine("project dir: " + projectDirectory);
        var uniqueFilename = string.Format(@"{0}.jpg", Guid.NewGuid());
        Trace.WriteLine("full path: " + projectDirectory + uniqueFilename);
        mapImage.SaveAsJpeg(projectDirectory + uniqueFilename);
        //create tour object from route and route image
        //save tour object to database
    }

    async Task<Root> GetRouteAsync(string fromLocation, string toLocation)
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

    string BuildRouteEndpoint(string fromLocation, string toLocation)
    {
        var endpoint = client.BaseAddress.ToString() + 
            "directions/v2/route?key=" + key +
            "&from=" + fromLocation +
            "&to=" + toLocation +
            "&outFormat=json&ambiguities=ignore&routeType=fastest&doReverseGeocode=false&enhancedNarrative=false&avoidTimedConditions=false";
        return endpoint;
    }

    async Task<Image<Rgba32>> GetRouteImageAsync(string sessionId)
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

    string BuildRouteImageEndpoint(string sessionId)
    {
        var endpoint = client.BaseAddress.ToString() + "staticmap/v5/map?key=" + key + "&session=" + sessionId;
        Trace.WriteLine(endpoint);
        return endpoint;
    }

    void InitializeMapHandler()
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

    static string GetKey(IConfigurationRoot config)
    {
        return config.GetSection("MapHandlerSettings")["key"];
    }

    static string GetWidth(IConfigurationRoot config)
    {
        return config.GetSection("MapHandlerSettings")["width"];
    }

    static string GetHeight(IConfigurationRoot config)
    {
        return config.GetSection("MapHandlerSettings")["height"];
    }
}
