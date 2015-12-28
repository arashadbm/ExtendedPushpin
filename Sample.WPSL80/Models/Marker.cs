using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.WPSL80.Models
{
    public class Marker
    {
        public GeoCoordinate GeoCoordinate { get; set; }

        public string Name { get; set; }

        public Marker()
        {
            
        }

        public Marker(GeoCoordinate geoCoordinate, string name)
        {
            GeoCoordinate = geoCoordinate;
            Name = name;
        }
    }
}
