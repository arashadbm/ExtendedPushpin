using System.Collections.Generic;
using Windows.Devices.Geolocation;
using Sample.WP81.Models;

namespace Sample.WP81.DataSource
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
                    new Marker(new Geopoint(new BasicGeoposition {Latitude = 52.530744,Longitude = 13.383039}), "Mitte" ),
                    new Marker(new Geopoint(new BasicGeoposition {Latitude =52.5276842, Longitude = 13.3714342}),"Medical Museum" ),
                    new Marker(new Geopoint(new BasicGeoposition {Latitude =52.5259775, Longitude =13.3734353}),"Charité" ),
                    new Marker(new Geopoint(new BasicGeoposition {Latitude =52.525873, Longitude =13.3725341}),"Universitätsmedizin" ),
                };
            }
        }
    }
}
