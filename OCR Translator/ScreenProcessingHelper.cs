using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OCR_Translator
{
    class ScreenProcessingHelper
    {
        [DllImport("user32.dll")]
        static extern IntPtr WindowFromPoint(Point p);

        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);


        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int x1;        // x position of upper-left corner
            public int y1;         // y position of upper-left corner
            public int x2;       // x position of lower-right corner
            public int y2;      // y position of lower-right corner
        }



        private static PixelFormat format = PixelFormat.Format32bppRgb;

        public static Bitmap getPrimaryScreen()
        {
            Bitmap image = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, format);
            var gfx = Graphics.FromImage(image);
            gfx.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            return image;
        }

        public static Bitmap getVirtualScreen()
        {
            Bitmap image = new Bitmap(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height, format);
            var gfx = Graphics.FromImage(image);
            gfx.CopyFromScreen(0, 0, 0, 0, SystemInformation.VirtualScreen.Size, CopyPixelOperation.SourceCopy);
            return image;
        }

        public static Bitmap getScreenRectangle(Point startPos, Size size)
        {
            Bitmap image = new Bitmap(size.Width, size.Height, format);
            var gfx = Graphics.FromImage(image);
            gfx.CopyFromScreen(startPos.X, startPos.Y, 0, 0, size, CopyPixelOperation.SourceCopy);
            return image;
        }

        public static Color getModalColour(Bitmap bitmap)
        {
            Dictionary<Color, uint> colorCounts = new Dictionary<Color, uint>();
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color c = bitmap.GetPixel(x, y);
                    
                    if(colorCounts.ContainsKey(c))
                    {
                        colorCounts[c] += 1;
                    }
                    else
                    {
                        colorCounts[c] = 1;
                    }
                }
            }
            return colorCounts.OrderByDescending(x => x.Value).First().Key;
        }

        public static IEnumerable<Bitmap> getAllScreens()
        {
            foreach(var scr in Screen.AllScreens)
            {
                Bitmap image = new Bitmap(scr.Bounds.Width, scr.Bounds.Height, format);
                var gfx = Graphics.FromImage(image);
                gfx.CopyFromScreen(scr.Bounds.X, scr.Bounds.Y, 0, 0, scr.Bounds.Size, CopyPixelOperation.SourceCopy);

                yield return image;
            }
        }

        public static int[][] getPixelArray(Bitmap bitmap)
        {
            var result = new int[bitmap.Height][];
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly,
                format);

            for (int y = 0; y < bitmap.Height; ++y)
            {
                result[y] = new int[bitmap.Width];
                Marshal.Copy(bitmapData.Scan0 + y * bitmapData.Stride, result[y], 0, result[y].Length);
            }

            bitmap.UnlockBits(bitmapData);

            return result;
        }

        public static Point? Find(Bitmap haystack, Bitmap needle)
        {
            if (null == haystack || null == needle)
            {
                return null;
            }
            if (haystack.Width < needle.Width || haystack.Height < needle.Height)
            {
                return null;
            }

            var haystackArray = getPixelArray(haystack);
            var needleArray = getPixelArray(needle);

            foreach (var firstLineMatchPoint in FindMatch(haystackArray.Take(haystack.Height - needle.Height), needleArray[0]))
            {
                if (IsNeedlePresentAtLocation(haystackArray, needleArray, firstLineMatchPoint, 1))
                {
                    return firstLineMatchPoint;
                }
            }

            return null;
        }

        private static IEnumerable<Point> FindMatch(IEnumerable<int[]> haystackLines, int[] needleLine)
        {
            var y = 0;
            foreach (var haystackLine in haystackLines)
            {
                for (int x = 0, n = haystackLine.Length - needleLine.Length; x < n; ++x)
                {
                    if (ContainSameElements(haystackLine, x, needleLine, 0, needleLine.Length))
                    {
                        yield return new Point(x, y);
                    }
                }
                y += 1;
            }
        }

        private static bool ContainSameElements(int[] first, int firstStart, int[] second, int secondStart, int length)
        {
            for (int i = 0; i < length; ++i)
            {
                if (first[i + firstStart] != second[i + secondStart])
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsNeedlePresentAtLocation(int[][] haystack, int[][] needle, Point point, int alreadyVerified)
        {
            //we already know that "alreadyVerified" lines already match, so skip them
            for (int y = alreadyVerified; y < needle.Length; ++y)
            {
                if (!ContainSameElements(haystack[y + point.Y], point.X, needle[y], 0, needle.Length))
                {
                    return false;
                }
            }
            return true;
        }
        public static IntPtr getWindowFromPoint(Point p)
        {
            return WindowFromPoint(p);
        }

        public static Point screenPointToWindowPoint(IntPtr windowHandle, Point startPos)
        {
            RECT rect = getWindowRect(windowHandle);
            return new Point(startPos.X - rect.x1, startPos.Y - rect.y1);
        }

        public static RECT getWindowRect(IntPtr windowHandle)
        {
            RECT rect = new RECT();
            GetWindowRect(windowHandle, out rect);
            return rect;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        static uint PW_RENDERFULLCONTENT = 0x00000002;

        public static Bitmap getWindowScreenshot(IntPtr windowHandle, Point startPos, Size size)
        {
            RECT rect = new RECT();
            GetWindowRect(windowHandle, out rect);
            int width = rect.x2 - rect.x1;
            int height = rect.y2 - rect.y1;

            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            IntPtr dc = g.GetHdc();

            PrintWindow(windowHandle, dc, PW_RENDERFULLCONTENT);

            g.ReleaseHdc();
            g.Dispose();

            return cropBitmap(bmp, startPos, size);
        }

        public static Bitmap cropBitmap(Bitmap bitmap, Point startPos, Size size)
        {
            Rectangle rect = new Rectangle(startPos.X, startPos.Y, size.Width, size.Height);
            Bitmap cropped = bitmap.Clone(rect, format);
            return cropped;
        }

        public static Bitmap filterColour(Bitmap bitmap, Color returnColor)
        {
            Bitmap newBitmap = (Bitmap)bitmap.Clone();
            Color replaceColor = Color.FromArgb(returnColor.ToArgb() ^ 0xffffff);
            for (var x = 0; x < newBitmap.Width; x++)
            {
                for (var y = 0; y < newBitmap.Height; y++)
                {
                    if (newBitmap.GetPixel(x, y) != returnColor)
                    {
                        newBitmap.SetPixel(x, y, replaceColor);
                    }
                }
            }
            return newBitmap;
        }
    }
}
