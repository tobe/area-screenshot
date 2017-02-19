#define DEBUG
using System;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace LoLScoreboard {
    public class Worker{
        System.Windows.Controls.Image screenshotControl;
        Bitmap screenshot;
        Graphics graphics;
        
        // Coordinates
        int[] topLeft       = { 310, 243 };
        int[] bottomRight   = { 1131, 570 };

        [DllImport("user32.dll")]
        public static extern bool GetAsyncKeyState(int vKey);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public Worker(System.Windows.Controls.Image screenshotControl) {
            this.screenshotControl = screenshotControl;
        }

        /* http://stackoverflow.com/questions/26260654/wpf-converting-bitmap-to-imagesource */
        public ImageSource ImageSourceForBitmap(Bitmap bmp) {
            var handle = bmp.GetHbitmap();
            try {
                return Imaging.CreateBitmapSourceFromHBitmap(
                    handle, IntPtr.Zero, Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions()
                );
            }
            finally { DeleteObject(handle); }
        }

        public void TakeScreenshot() {
            while (true) {
                if (!GetAsyncKeyState(0x09)) {
                    Thread.Sleep(100);
                    continue;
                }

                // If F1 has been hit, log the cursor coordinates.
#if DEBUG
                System.Drawing.Point Point = System.Windows.Forms.Cursor.Position;
                Debug.WriteLine(String.Format(
                    "X: {0}, Y: {1}",
                    Point.X, Point.Y
                ));
#endif

                // TAB has been hit, take a screenshot and update the control, figure out the dimensions
                int width = bottomRight[0] - topLeft[0];
                int height = bottomRight[1] - topLeft[1];

                // Spawn a new Bitmap object
                try {
                    screenshot = null;
                    screenshot = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                }catch(ArgumentException) {
                    MessageBox.Show("Failed to spawn the Bitmap object. Verify the coordinates are correct.");
                }

                // Create the image
                try {
                    graphics = null;
                    graphics = Graphics.FromImage(screenshot);
                    graphics.CopyFromScreen(
                        topLeft[0],
                        topLeft[1],
                        0,
                        0,
                        new System.Drawing.Size(width, height), CopyPixelOperation.SourceCopy
                    );
                }catch(Exception) { // Since the docs don't even have a Return section for Graphics...
                    MessageBox.Show("Failed to create the image.");
                }

                // Convert to Bitmap and set the source
                if(this != null) {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {
                        screenshotControl.Source = ImageSourceForBitmap(screenshot);
                    }));
                }

                // Just to give it some room
                Thread.Sleep(10);
            }
        }
    }
}
