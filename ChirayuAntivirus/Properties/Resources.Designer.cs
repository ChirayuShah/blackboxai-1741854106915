using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace ChirayuAntivirus.Properties
{
    internal static class Resources
    {
        private static Icon appIcon;

        public static Icon AppIcon
        {
            get
            {
                if (appIcon == null)
                {
                    // Create a simple shield icon programmatically
                    using (Bitmap bmp = new Bitmap(32, 32))
                    {
                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            g.Clear(Color.FromArgb(0, 122, 204)); // Blue background
                            
                            // Draw shield outline
                            Point[] shieldPoints = {
                                new Point(16, 4),   // Top
                                new Point(28, 8),   // Top right
                                new Point(28, 20),  // Bottom right
                                new Point(16, 28),  // Bottom
                                new Point(4, 20),   // Bottom left
                                new Point(4, 8)     // Top left
                            };
                            g.FillPolygon(Brushes.White, shieldPoints);

                            // Draw checkmark
                            Pen checkPen = new Pen(Color.FromArgb(0, 122, 204), 2);
                            g.DrawLines(checkPen, new Point[] {
                                new Point(10, 16),
                                new Point(14, 20),
                                new Point(22, 12)
                            });
                        }

                        // Convert bitmap to icon
                        IntPtr hIcon = bmp.GetHicon();
                        appIcon = Icon.FromHandle(hIcon);
                    }
                }
                return appIcon;
            }
        }
    }
}
