using System.Windows;
using System.Windows.Media;

namespace ExtendedPushbin.Controls
{

    public static class ControlFinder
    {
        public static T FindParent<T> ( DependencyObject control ) where T : DependencyObject
        {
            DependencyObject p = VisualTreeHelper.GetParent(control) as DependencyObject;
            if(p != null)
            {
                if(p is T)
                    return p as T;
                else
                    return ControlFinder.FindParent<T>(p);
            }
            return null;
        }
    }
}
