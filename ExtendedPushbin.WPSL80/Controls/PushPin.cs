using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace ExtendedPushbin.Controls
{

    /// <summary>
    /// Represents a pushpin on the map.
    /// </summary>
    [ContentProperty("Content")]
    [TemplatePart(Name = ExpandedPanelTemplateName, Type = typeof(Panel))]
    [TemplatePart(Name = IconTemplateName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = ExpandingPopupName, Type = typeof(Popup))]
    [TemplatePart(Name = PresenterName, Type = typeof(ContentPresenter))]

    public sealed class Pushpin : ContentControl
    {
        #region Fields
        //Template parts names
        private const string ExpandedPanelTemplateName = "ExpandedPanel";
        private const string IconTemplateName = "Icon";
        private const string ExpandingPopupName = "ExpandingPopup";
        private const string PresenterName = "Presenter";
        //State Names
        private const string HiddenStateName = "Hidden";
        private const string ExpandedStateName = "Expanded";
        //Template Parts
        private Panel _expandedPanel;
        private FrameworkElement _icon;
        private ContentPresenter _presenter;
        public static Pushpin LastAnimatedPushpin;
        private Popup _expandingPopup;

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
        public Pushpin ()
        {
            this.DefaultStyleKey = typeof(Pushpin);
        }
        #endregion

        #region Methods
        public override void OnApplyTemplate ()
        {
            base.OnApplyTemplate();

            _expandedPanel = (StackPanel)GetTemplateChild(ExpandedPanelTemplateName);
            _icon = (FrameworkElement)GetTemplateChild(IconTemplateName);
            _expandingPopup = (Popup)GetTemplateChild(ExpandingPopupName);
            _presenter = (ContentPresenter)GetTemplateChild(PresenterName);


            _expandingPopup.VerticalOffset = 0;
            _expandingPopup.HorizontalOffset = 0;
            if(_icon != null)
            {
                //Listen for Ellipse Tap to expand pushpin tooltip
                _icon.Tap += _icon_Tap;
            }
            //Set initial state to Hidden
            VisualStateManager.GoToState(this, HiddenStateName, false);
        }

        /// <summary>
        /// Listen to Ellipse tap to expand the pushPin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _icon_Tap ( object sender, System.Windows.Input.GestureEventArgs e )
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
        public void HideExpansion ()
        {
            if(_expandedPanel != null)
            {

                VisualStateManager.GoToState(this, HiddenStateName, false);
            }
            IsExpanded = false;
        }

        /// <summary>
        /// Shows the tooltip expansion of pushpin.
        /// </summary>
        private void AnimateExpansion ( Point point )
        {
            //Hide previous Pushbin if any
            if(LastAnimatedPushpin != null) LastAnimatedPushpin.HideExpansion();

            OnExpanding();

            LastAnimatedPushpin = this;

            if(_expandedPanel != null)
            {
                Canvas.SetLeft(_expandingPopup, point.X);
                Canvas.SetTop(_expandingPopup, point.Y);

                //Unsubscribe to prevent memory leaks due to multiple subscribes
                _expandedPanel.LayoutUpdated -= _expandedPanel_LayoutUpdated;

                //Subscribe to LayoutUpdated event to update margin of _expandedPanel when it's rendered
                _expandedPanel.LayoutUpdated += _expandedPanel_LayoutUpdated;

                VisualStateManager.GoToState(this, ExpandedStateName, true);
                IsExpanded = true;

                OnExpanded();
            }
        }

        private void _expandedPanel_LayoutUpdated ( object sender, EventArgs e )
        {
            Debug.WriteLine("AW:" + _expandedPanel.ActualWidth + "_ AC:" + _expandedPanel.ActualHeight);
            double smallOffset = 10;//small offset to make the arrow in the center of ellipse
           //We need to set negative Margin to shift the tooltip to top and left sides
            _expandedPanel.Margin = new Thickness((_icon.ActualWidth - _expandedPanel.ActualWidth) / 2 + smallOffset, (_icon.ActualHeight / 2 - _expandedPanel.ActualHeight), 0, 0);

            //if width is less than 80 supress, 80 = ContentPresenter's( min width + Margin)= 70 +8
            //If it's less to 80 then content presenter didn't render yet
            //TODO:Investigate further
            if(Math.Abs(_expandedPanel.ActualWidth) < 80 || Math.Abs(_expandedPanel.ActualHeight) < 1e-9) return;
            //Unsubscribe or Layout cycle will happen
            _expandedPanel.LayoutUpdated -= _expandedPanel_LayoutUpdated;

        }

        public event EventHandler Expanding;

        private void OnExpanding ()
        {
            var handler = Expanding;
            if(handler != null) handler(this, EventArgs.Empty);
        }


        public event EventHandler Expanded;

        private void OnExpanded ()
        {
            var handler = Expanded;
            if(handler != null) handler(this, EventArgs.Empty);
        }
        #endregion
    }


}
