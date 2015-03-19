using System.Windows;

namespace Mileage.Client.Windows.Layout
{
    public class LayoutSettings
    {
        public static readonly DependencyProperty ControlNameProperty = DependencyProperty.RegisterAttached(
            "ControlName", typeof (string), typeof (LayoutSettings), new PropertyMetadata(default(string)));

        public static void SetControlName(DependencyObject element, string value)
        {
            element.SetValue(ControlNameProperty, value);
        }

        public static string GetControlName(DependencyObject element)
        {
            return (string)element.GetValue(ControlNameProperty);
        }

        public static readonly DependencyProperty StopLayoutAnalysisProperty = DependencyProperty.RegisterAttached(
            "StopLayoutAnalysis", typeof (bool), typeof (LayoutSettings), new PropertyMetadata(default(bool)));

        public static void SetStopLayoutAnalysis(DependencyObject element, bool value)
        {
            element.SetValue(StopLayoutAnalysisProperty, value);
        }

        public static bool GetStopLayoutAnalysis(DependencyObject element)
        {
            return (bool)element.GetValue(StopLayoutAnalysisProperty);
        }
    }
}