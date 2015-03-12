﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;

namespace Mileage.Client.Windows.Windows
{
    public class MileageWindowManager : WindowManager
    {
        #region Overrides of WindowManager
        /// <summary>
        /// Makes sure the view is a window is is wrapped by one.
        /// </summary>
        /// <param name="model">The view model.</param>
        /// <param name="view">The view.</param>
        /// <param name="isDialog">Whethor or not the window is being shown as a dialog.</param>
        protected override Window EnsureWindow(object model, object view, bool isDialog)
        {
            var window = view as Window;

            if (window == null)
            {
                window = this.CreateWindow(model, view, isDialog);
                window.Content = view;

                window.SetValue(View.IsGeneratedProperty, true);

                var owner = InferOwnerOf(window);
                if (owner != null)
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    window.Owner = owner;
                }
                else
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }
            }
            else
            {
                var owner = InferOwnerOf(window);
                if (owner != null && isDialog)
                {
                    window.Owner = owner;
                }
            }

            this.ConfigureWindow(window);

            return window;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates the window.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="view">The view.</param>
        /// <param name="isDialog">if set to <c>true</c> [is dialog].</param>
        protected virtual Window CreateWindow(object model, object view, bool isDialog)
        {
            return new ExtendedDXRibbonWindow();
        }
        /// <summary>
        /// Configures the window.
        /// </summary>
        /// <param name="window">The window.</param>
        protected virtual void ConfigureWindow(Window window)
        {
            window.UseLayoutRounding = true;
        }
        #endregion
    }
}