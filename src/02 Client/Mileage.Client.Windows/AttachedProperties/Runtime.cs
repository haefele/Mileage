using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;

namespace Mileage.Client.Windows.AttachedProperties
{
    public static class Runtime
    {
        public static readonly DependencyProperty VisibilityProperty = DependencyProperty.RegisterAttached(
            "Visibility", typeof (Visibility), typeof (Runtime), 
            new PropertyMetadata(default(Visibility), OnVisibilityChanged));

        public static void SetVisibility(DependencyObject element, Visibility value)
        {
            element.SetValue(VisibilityProperty, value);
        }

        public static Visibility GetVisibility(DependencyObject element)
        {
            return (Visibility)element.GetValue(VisibilityProperty);
        }
        
        private static void OnVisibilityChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var control = (FrameworkElement)dependencyObject;

            if (Execute.InDesignMode == false)
                control.Visibility = (Visibility)e.NewValue;
        }
    }
}
