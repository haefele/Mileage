using System.Windows;

namespace Mileage.Client.Windows.Layout
{
    public class LayoutSettings
    {
        public static readonly DependencyProperty LayoutControlNameProperty = DependencyProperty.RegisterAttached(
            "LayoutControlName", typeof (string), typeof (LayoutSettings), new PropertyMetadata(default(string)));

        public static void SetLayoutControlName(DependencyObject element, string value)
        {
            element.SetValue(LayoutControlNameProperty, value);
        }

        public static string GetLayoutControlName(DependencyObject element)
        {
            return (string)element.GetValue(LayoutControlNameProperty);
        }

        public static readonly DependencyProperty IgnoreChildControlsProperty = DependencyProperty.RegisterAttached(
            "IgnoreChildControls", typeof (bool), typeof (LayoutSettings), new PropertyMetadata(default(bool)));

        public static void SetIgnoreChildControls(DependencyObject element, bool value)
        {
            element.SetValue(IgnoreChildControlsProperty, value);
        }

        public static bool GetIgnoreChildControls(DependencyObject element)
        {
            return (bool)element.GetValue(IgnoreChildControlsProperty);
        }
    }
}