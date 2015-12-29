﻿#if WINDOWS_PHONE
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
#else
using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;
#endif
namespace ExtendedPushbin.Controls
{

    /// <summary>
    /// Represents a pushpin on the map.
    /// </summary>
#if WINDOWS_PHONE
    [ContentProperty("Content")]
#else
    [ContentProperty(Name = "Content")]
#endif
    [TemplatePart(Name = ExpandedPanelTemplateName, Type = typeof(Panel))]
    [TemplatePart(Name = IconPresenterTemplateName, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = ExpandingPopupName, Type = typeof(Popup))]
    [TemplatePart(Name = ExpandingPresenterName, Type = typeof(ContentPresenter))]

    public sealed class Pushpin : ContentControl
    {
        #region Fields
        //Template parts names
        private const string ExpandedPanelTemplateName = "PART_ExpandedPanel";
        private const string IconPresenterTemplateName = "PART_IconPresenter";
        private const string ExpandingPopupName = "PART_ExpandingPopup";
        private const string ExpandingPresenterName = "PART_ExpandingPresenter";
        //State Names
        private const string HiddenStateName = "Hidden";
        private const string ExpandedStateName = "Expanded";
        //Template Parts
        private ContentPresenter _iconPresenter;
        private ContentPresenter _expandingPresenter;
        public static Pushpin LastAnimatedPushpin;
        private Popup _expandingPopup;
        private Panel _expandedPanel;


        #endregion

        #region Is Expanded dependency property
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsExpanded.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(Pushpin), new PropertyMetadata(false));



        #endregion

        #region Expansion Template property

        public DataTemplate ExpansionTemplate
        {
            get { return (DataTemplate)GetValue(ExpansionTemplateProperty); }
            set { SetValue(ExpansionTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ExpansionTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ExpansionTemplateProperty =
            DependencyProperty.Register("ExpansionTemplate", typeof(DataTemplate), typeof(Pushpin), new PropertyMetadata(null));

        #endregion

        #region Public Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Pushpin"/> class.
        /// </summary>
        public Pushpin()
        {
            this.DefaultStyleKey = typeof(Pushpin);
        }
        #endregion

        #region Methods
#if WINDOWS_PHONE
        public override void OnApplyTemplate()
#else
        protected override void OnApplyTemplate()
#endif
        {
            base.OnApplyTemplate();

            _expandedPanel = (StackPanel)GetTemplateChild(ExpandedPanelTemplateName);
            _iconPresenter = (ContentPresenter)GetTemplateChild(IconPresenterTemplateName);
            _expandingPopup = (Popup)GetTemplateChild(ExpandingPopupName);
            _expandingPresenter = (ContentPresenter)GetTemplateChild(ExpandingPresenterName);


            _expandingPopup.VerticalOffset = 0;
            _expandingPopup.HorizontalOffset = 0;
            if (_iconPresenter != null)
            {
                //Listen for Ellipse Tap to expand pushpin tooltip
#if WINDOWS_PHONE
        _iconPresenter.Tap += IconPresenterTap;
#else
                _iconPresenter.Tapped += IconPresenterTap;
#endif
            }
            //Set initial state to Hidden
            VisualStateManager.GoToState(this, HiddenStateName, false);
        }

        /// <summary>
        /// Listen to Ellipse tap to expand the pushPin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
#if WINDOWS_PHONE
private void IconPresenterTap(object sender, System.Windows.Input.GestureEventArgs e)
#else
        private void IconPresenterTap(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
#endif
        {
            //if(!IsExpanded)
            {
                AnimateExpansion(e.GetPosition(null));
            }
            //Don't propgate event to Map
            e.Handled = true;
        }

        /// <summary>
        /// Hide tooltip expansion of pushin.
        /// </summary>
        public void HideExpansion()
        {
            if (_expandedPanel != null)
            {

                VisualStateManager.GoToState(this, HiddenStateName, false);
            }
            IsExpanded = false;
        }

        /// <summary>
        /// Shows the tooltip expansion of pushpin.
        /// </summary>
        private void AnimateExpansion(Point point)
        {
            //Hide previous Pushbin if any
            if (LastAnimatedPushpin != null) LastAnimatedPushpin.HideExpansion();

            OnExpanding();

            LastAnimatedPushpin = this;

            if (_expandedPanel != null)
            {
                Canvas.SetLeft(_expandingPopup, point.X);
                Canvas.SetTop(_expandingPopup, point.Y);
                Canvas.SetZIndex(_expandingPopup, 0);
                //Unsubscribe to prevent memory leaks due to multiple subscribes
                _expandedPanel.LayoutUpdated -= ExpandedPanelLayoutUpdated;

                //Subscribe to LayoutUpdated event to update margin of _PART_ExpandedPanel when it's rendered
                _expandedPanel.LayoutUpdated += ExpandedPanelLayoutUpdated;

                VisualStateManager.GoToState(this, ExpandedStateName, true);
                IsExpanded = true;

                OnExpanded();
            }
        }

#if WINDOWS_PHONE
private void ExpandedPanelLayoutUpdated(object sender, EventArgs e)
#else
        private void ExpandedPanelLayoutUpdated(object sender, System.Object e)
#endif
        {
            //Debug.WriteLine("AW:" + _expandedPanel.ActualWidth + "_ AC:" + _expandedPanel.ActualHeight);

            //small offset to make the arrow in the center of ellipse
            //We need to set negative Margin to shift the tooltip to top and left sides
            double smallOffset = 10;
            _expandedPanel.Margin = new Thickness((_iconPresenter.ActualWidth - _expandedPanel.ActualWidth) / 2 + smallOffset, (_iconPresenter.ActualHeight / 2 - _expandedPanel.ActualHeight), 0, 0);
            //_expandingPopup.HorizontalOffset = (_iconPresenter.ActualWidth - _expandedPanel.ActualWidth) /2;
            //_expandingPopup.VerticalOffset = (_iconPresenter.ActualHeight/2 - _expandedPanel.ActualHeight);

            //if width is less than 80 supress, 80 = ContentPresenter's( min width + Margin)= 70 +8
            //If it's less to 80 then content presenter didn't render yet
            //TODO:Investigate further
            if (Math.Abs(_expandedPanel.ActualWidth) < 80 || Math.Abs(_expandedPanel.ActualHeight) < 1e-9) return;
            //Unsubscribe or Layout cycle will happen
            _expandedPanel.LayoutUpdated -= ExpandedPanelLayoutUpdated;

        }

        #endregion

        #region Custom Events
        public event EventHandler Expanding;

        private void OnExpanding()
        {
            var handler = Expanding;
            if (handler != null) handler(this, EventArgs.Empty);
        }


        public event EventHandler Expanded;

        private void OnExpanded()
        {
            var handler = Expanded;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        #endregion
    }
}
