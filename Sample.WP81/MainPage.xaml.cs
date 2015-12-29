using System;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Navigation;
using ExtendedPushpin.Controls;
using Sample.DataSource;
using Sample.Models;

namespace Sample
{

    public sealed partial class MainPage : Page
    {
        private readonly Geopoint _center = new Geopoint(new BasicGeoposition { Latitude = 52.530744, Longitude = 13.383039 });//Mitte, Berlin, Germany
        private const int InitialZoomLevel = 14;
        /// <summary>
        /// This is the resource key of PushPin Tooltib template inside Page.Resources
        /// </summary>
        private const string PushPinTemplateName = "PushExpansionTemplate";

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            AddMarkers();
        }

        /// <summary>
        /// Zoom to initial location in map with Animation, the initial location is Mitte, Berlin.
        /// </summary>
        private async void ZoomToInitialLocation()
        {
            await MyMap.TrySetViewAsync(_center, InitialZoomLevel, null, null, MapAnimationKind.Bow);
        }

        /// <summary>
        /// This method will add a list of sample Markers.
        /// </summary>
        /// <returns></returns>
        private void AddMarkers()
        {
            //Clear previous Markers if any
            MyMap.Children.Clear();

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
            MyMap.Children.Add(pushpin);
            var point = new Point(0.5, 1);
            MapControl.SetNormalizedAnchorPoint(pushpin, point);
            MapControl.SetLocation(pushpin, marker.Geopoint);
        }

        private void MapLoaded(object sender, RoutedEventArgs e)
        {
            ZoomToInitialLocation();
            MyMap.MapServiceToken = Constants.MapServiceToken;
        }

        private void MapTapped(MapControl sender, MapInputEventArgs args)
        {
            //Hide Expansion on Tapping on empty parts of map
            if (Pushpin.LastAnimatedPushpin != null) Pushpin.LastAnimatedPushpin.HideExpansion();
        }
    }
}
