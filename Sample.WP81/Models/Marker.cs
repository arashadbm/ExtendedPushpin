
using Windows.Devices.Geolocation;

namespace Sample.WP81.Models
{
    public class Marker
    {
        public Geopoint Geopoint { get; set; }

        public string Name { get; set; }

        public Marker()
        {
            
        }

        public Marker(Geopoint geopoint, string name)
        {
            Geopoint = geopoint;
            Name = name;
        }
    }
}
