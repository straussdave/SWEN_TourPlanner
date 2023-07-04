using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWEN_TourPlanner.Model
{
    public class RouteResponse //created with the help of https://json2csharp.com
    {
        //Root deserializedRoute = JsonConvert.DeserializeObject<Root>(myJsonResponse);

        public RouteResponse() { }

        public class Root
        {
            public Route route { get; set; }
        }

        public class Route
        {
            public string sessionId { get; set; }
            public int realTime { get; set; }
            public double distance { get; set; }
            public int time { get; set; }
            public string formattedTime { get; set; }
            public bool hasHighway { get; set; }
            public bool hasTollRoad { get; set; }
            public bool hasBridge { get; set; }
            public bool hasSeasonalClosure { get; set; }
            public bool hasTunnel { get; set; }
            public bool hasFerry { get; set; }
            public bool hasUnpaved { get; set; }
            public bool hasTimedRestriction { get; set; }
            public bool hasCountryCross { get; set; }
            public List<Leg> legs { get; set; }
            public Options options { get; set; }
            public BoundingBox boundingBox { get; set; }
            public string name { get; set; }
            public string maxRoutes { get; set; }
            public List<Location> locations { get; set; }
            public List<int> locationSequence { get; set; }
        }

        public class BoundingBox
        {
            public Ul ul { get; set; }
            public Lr lr { get; set; }
        }

        public class DisplayLatLng
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class LatLng
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Leg
        {
            public int index { get; set; }
            public bool hasTollRoad { get; set; }
            public bool hasHighway { get; set; }
            public bool hasBridge { get; set; }
            public bool hasUnpaved { get; set; }
            public bool hasTunnel { get; set; }
            public bool hasSeasonalClosure { get; set; }
            public bool hasFerry { get; set; }
            public bool hasCountryCross { get; set; }
            public bool hasTimedRestriction { get; set; }
            public double distance { get; set; }
            public int time { get; set; }
            public string formattedTime { get; set; }
            public int origIndex { get; set; }
            public string origNarrative { get; set; }
            public int destIndex { get; set; }
            public string destNarrative { get; set; }
            public List<Maneuver> maneuvers { get; set; }
        }

        public class Location
        {
            public string street { get; set; }
            public string adminArea6 { get; set; }
            public string adminArea6Type { get; set; }
            public string adminArea5 { get; set; }
            public string adminArea5Type { get; set; }
            public string adminArea4 { get; set; }
            public string adminArea4Type { get; set; }
            public string adminArea3 { get; set; }
            public string adminArea3Type { get; set; }
            public string adminArea1 { get; set; }
            public string adminArea1Type { get; set; }
            public string postalCode { get; set; }
            public string geocodeQualityCode { get; set; }
            public string geocodeQuality { get; set; }
            public bool dragPoint { get; set; }
            public string sideOfStreet { get; set; }
            public string linkId { get; set; }
            public string unknownInput { get; set; }
            public string type { get; set; }
            public LatLng latLng { get; set; }
            public DisplayLatLng displayLatLng { get; set; }
            public string mapUrl { get; set; }
        }

        public class Lr
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Maneuver
        {
            public int index { get; set; }
            public double distance { get; set; }
            public string narrative { get; set; }
            public int time { get; set; }
            public int direction { get; set; }
            public string directionName { get; set; }
            public List<object> signs { get; set; }
            public List<object> maneuverNotes { get; set; }
            public string formattedTime { get; set; }
            public string transportMode { get; set; }
            public StartPoint startPoint { get; set; }
            public int turnType { get; set; }
            public int attributes { get; set; }
            public string iconUrl { get; set; }
            public List<string> streets { get; set; }
            public string mapUrl { get; set; }
        }

        public class Options
        {
            public string routeType { get; set; }
            public bool enhancedNarrative { get; set; }
            public bool doReverseGeocode { get; set; }
            public string narrativeType { get; set; }
            public int walkingSpeed { get; set; }
            public int highwayEfficiency { get; set; }
            public bool avoids { get; set; }
            public int generalize { get; set; }
            public string shapeFormat { get; set; }
            public string unit { get; set; }
            public string locale { get; set; }
            public bool useTraffic { get; set; }
            public int timeType { get; set; }
            public int dateType { get; set; }
            public bool manMaps { get; set; }
            public bool sideOfStreetDisplay { get; set; }
        }

        public class StartPoint
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Ul
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }
    }
}
