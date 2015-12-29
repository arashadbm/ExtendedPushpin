using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ExtendedPushbin.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps;
using Microsoft.Phone.Maps.Controls;
using Sample.WPSL80.DataSource;
using Sample.WPSL80.Models;


namespace Sample.WPSL80
{
    public partial class MainPage : PhoneApplicationPage
    {
        private readonly GeoCoordinate _center = new GeoCoordinate(52.526212, 13.378565);//Mitte, Berlin, Germany
        private const int InitialZoomLevel = 14;
        /// <summary>
        /// This is the resource key of PushPin Tooltib template inside Page.Resources
        /// </summary>
        private const string PushPinTemplateName = "PushExpansionTemplate";

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode != NavigationMode.Back)
            {
                AddMarkers();
            }
        }

        /// <summary>
        /// Zoom to initial location in map with Animation, the initial location is Mitte, Berlin.
        /// </summary>
        private  void ZoomToInitialLocation()
        {
            Dispatcher.BeginInvoke(() => MyMap.SetView(_center, InitialZoomLevel, MapAnimationKind.Parabolic));
        }

        /// <summary>
        /// This method will add a list of sample Markers.
        /// </summary>
        /// <returns></returns>
        private void AddMarkers()
        {
            //Clear previous Markers if any
            MyMap.Layers.Clear();

            //Get the sample markers
            var markers = MarkersSource.Markers;

            //Add markers to map
            foreach (var marker in markers)
            {
                AddPushPin(marker);
            }
        }

        /// <summary>
        /// Renders pushbin using Mapoverlay and the Extended pushbin.
        /// </summary>
        /// <param name="marker"></param>
        public void AddPushPin(Marker marker)
        {

            var pushpin = new Pushpin
            {
                //set data template and data context of pushpin expansion template
                ExpansionTemplate = (DataTemplate)Resources[PushPinTemplateName],
                DataContext = marker,
            };
            MapOverlay overlay = new MapOverlay
            {
                GeoCoordinate = marker.GeoCoordinate,
                Content = pushpin,
                PositionOrigin = new Point(0.5, 1)
            };
            MapLayer layer = new MapLayer { overlay };
            MyMap.Layers.Add(layer);
        }



        private void MapLoaded(object sender, RoutedEventArgs e)
        {
            ZoomToInitialLocation();
            //Store Tokens to Authenticate map.
            //To be able to use Map control in windows phone 8.x Sliver light
            //You need to Get Map tokens from dev.windows.com for your application
            MapsSettings.ApplicationContext.ApplicationId = Constants.MapApplicationId;
            MapsSettings.ApplicationContext.AuthenticationToken = Constants.MapAuthenticationToken;
        }

        private void MapTapped(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //Hide Expansion on Tapping on empty parts of map
            if (Pushpin.LastAnimatedPushpin != null) Pushpin.LastAnimatedPushpin.HideExpansion();
        }
    }
}