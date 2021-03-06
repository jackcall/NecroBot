﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using GMap.NET.WindowsPresentation;
using System.Diagnostics;
using PoGo.Necrobot.Window.Model;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.Tasks;
using PoGo.NecroBot.Logic.State;

namespace PoGo.Necrobot.Window.Controls.MapMarkers
{
    /// <summary>
    /// Interaction logic for CustomMarkerDemo.xaml
    /// </summary>
    public partial class MapPokemonMarker
    {
        GMapMarker Marker;
        MainClientWindow MainWindow;
        MapPokemonViewModel nearbyModel;
        private ISession session;

        public bool IsMarkerOf(string encounterId)
        {
            return this.nearbyModel.EncounterId .ToString() == encounterId;

        }
        public MapPokemonMarker(MainClientWindow window, GMapMarker marker, ISession session)
        {
            this.InitializeComponent();

            this.MainWindow = window;
            this.Marker = marker;

            this.Unloaded += new RoutedEventHandler(CustomMarkerDemo_Unloaded);
            this.Loaded += new RoutedEventHandler(CustomMarkerDemo_Loaded);
            this.SizeChanged += new SizeChangedEventHandler(CustomMarkerDemo_SizeChanged);
            this.MouseEnter += new MouseEventHandler(MarkerControl_MouseEnter);
            this.MouseLeave += new MouseEventHandler(MarkerControl_MouseLeave);
            this.MouseMove += new MouseEventHandler(CustomMarkerDemo_MouseMove);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonUp);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonDown);
            this.session = session;
           // Popup.Placement = PlacementMode.Mouse;
        }

        public MapPokemonMarker(MainClientWindow window, GMapMarker marker, ISession session, MapPokemonViewModel nearbyModel) : this(window, marker, session)
        {
            this.session = session;
            this.nearbyModel = nearbyModel;
            this.DataContext = nearbyModel;
        }

        void CustomMarkerDemo_Loaded(object sender, RoutedEventArgs e)
        {
            if (icon.Source.CanFreeze)
            {
                icon.Source.Freeze();
            }
        }

        void CustomMarkerDemo_Unloaded(object sender, RoutedEventArgs e)
        {
            //this.Unloaded -= new RoutedEventHandler(CustomMarkerDemo_Unloaded);
            //this.Loaded -= new RoutedEventHandler(CustomMarkerDemo_Loaded);
            //this.SizeChanged -= new SizeChangedEventHandler(CustomMarkerDemo_SizeChanged);
            //this.MouseEnter -= new MouseEventHandler(MarkerControl_MouseEnter);
            //this.MouseLeave -= new MouseEventHandler(MarkerControl_MouseLeave);
            //this.MouseMove -= new MouseEventHandler(CustomMarkerDemo_MouseMove);
            //this.MouseLeftButtonUp -= new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonUp);
            //this.MouseLeftButtonDown -= new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonDown);

            //Marker.Shape = null;
            //icon.Source = null;
            //icon = null;
        }

        void CustomMarkerDemo_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Marker.Offset = new Point(-e.NewSize.Width / 2, -e.NewSize.Height);
        }

        void CustomMarkerDemo_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && IsMouseCaptured)
            {
                //Point p = e.GetPosition(MainWindow.MainMap);
                // Marker.Position = MainWindow.MainMap.FromLocalToLatLng((int) (p.X), (int) (p.Y));
            }
        }

        void CustomMarkerDemo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsMouseCaptured)
            {
                Mouse.Capture(this);
            }
            //Popup.IsOpen = true;
        }

        void CustomMarkerDemo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsMouseCaptured)
            {
                Mouse.Capture(null);
            }
        }

        void MarkerControl_MouseLeave(object sender, MouseEventArgs e)
        {
            Marker.ZIndex -= 10000;
            //Popup.IsOpen = false;
        }

        void MarkerControl_MouseEnter(object sender, MouseEventArgs e)
        {
            Marker.ZIndex += 10000;
            //Popup.IsOpen = true;
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
        }

        private void icon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            popInfo.IsOpen = true;

        }

        private void UserControl_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void btnCatchHim_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(async() =>
            {
                await SetMoveToTargetTask.Execute(nearbyModel.Latitude, nearbyModel.Longitude, nearbyModel.FortId);
            });
            this.Dispatcher.Invoke(() =>
            {
                popInfo.IsOpen = false;
            });
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                popInfo.IsOpen = false;
            });
        }
    }
}