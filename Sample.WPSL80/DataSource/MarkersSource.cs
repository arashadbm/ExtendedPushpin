using System.Collections.Generic;
using System.Device.Location;
using Sample.WPSL80.Models;

namespace Sample.WPSL80.DataSource
{
    public class MarkersSource
    {
        public static List<Marker> Markers
        {
            get
            {
                return new List<Marker>()
                {
                    //Sample locations from Mitte the middle of Berlin
                    new Marker(new GeoCoordinate(52.530744, 13.383039),"Mitte" ),
                    new Marker(new GeoCoordinate(52.5276842, 13.3714342),"Medical Museum" ),
                    new Marker(new GeoCoordinate(52.5259775,13.3734353),"Charité" ),
                    new Marker(new GeoCoordinate(52.525873, 13.3725341),"Universitätsmedizin" ),
                };
            }
        }
    }
}
