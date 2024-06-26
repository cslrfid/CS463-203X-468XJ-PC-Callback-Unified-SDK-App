﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CSLibrary.Windows
{
    /// <summary>
    /// Helper to show or close given splash window
    /// </summary>
    public static class Splasher
    {
        /// <summary>
        /// 
        /// </summary>
        private static Window mSplash;

        /// <summary>
        /// Get or set the splash screen window
        /// </summary>
        public static Window Splash
        {
            get
            {
                return mSplash;
            }
            set
            {
                mSplash = value;
            }
        }

        /// <summary>
        /// Show splash screen
        /// </summary>
        public static void ShowSplash()
        {
            if (mSplash != null)
            {
                mSplash.Dispatcher.BeginInvoke(new Action(() =>
                {
                    mSplash.Close();
                }));
            }

            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
            {
                Splash = new SplashDemo.SplashScreen();
                Splash.ShowDialog();
            }));
            thread.SetApartmentState(System.Threading.ApartmentState.STA);
            thread.Start();
        }
        /// <summary>
        /// Close splash screen
        /// </summary>
        public static void CloseSplash()
        {
            if (mSplash != null)
            {
                mSplash.Dispatcher.BeginInvoke(new Action(() =>
                {
                    mSplash.Close();
                }));

                if (mSplash is IDisposable)
                    (mSplash as IDisposable).Dispose();
            }
        }
    }
}
