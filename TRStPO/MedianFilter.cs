using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace TRStPO.Lab1
{
    public class MedianFilter
    {
        public static Bitmap newImage;
        public static Mutex mutexObj = new Mutex();

        public void Filter1()
        {
            Bitmap image = (Bitmap)Image.FromFile("C:/Users/Anuitex/source/repos/TRStPO/TRStPO/1.jpg");
            newImage = new Bitmap(image.Width, image.Height);
            int areaSize = 9;
            int medianSize = (areaSize * areaSize) / 2;

            for (int x = medianSize; x < (image.Width - medianSize); x++)
            {
                for (int y = medianSize; y < (image.Height - medianSize); y++)
                {
                    var colorArray = new List<Color>();

                    for (int i = x; i < areaSize + x; i++)
                    {
                        for (int j = y; j < areaSize + y; j++)
                        {
                            Color color = image.GetPixel(i, j);
                            colorArray.Add(color);
                        }
                    }
                    SetMedianFilter(colorArray, newImage, x, y);
                }
            }

            newImage.Save("C:/Users/Anuitex/source/repos/TRStPO/TRStPO/2.jpg");
        }

        public static void SetMedianFilter(List<Color> colors, Bitmap image, int x, int y)
        {
            var red = new List<byte>();
            var green = new List<byte>();
            var blue = new List<byte>();

            foreach (var color in colors)
            {
                red.Add(color.R);
                green.Add(color.G);
                blue.Add(color.B);
            }

            red.Sort();
            green.Sort();
            blue.Sort();

            var medianColor = Color.FromArgb(red[40], green[40], blue[40]);
            mutexObj.WaitOne();
            image.SetPixel(x, y, medianColor);
            mutexObj.ReleaseMutex();
        }

        public void Filter()
        {
            var threads = new List<Thread>();
            int threadAmount = 3;

            Bitmap image = (Bitmap)Image.FromFile("C:/Users/Anuitex/source/repos/TRStPO/TRStPO/1.png");
            newImage = new Bitmap(image.Width, image.Height);
            int areaSize = 9;
            int borderSize = areaSize / 2;
            int medianSize = (areaSize * areaSize) / 2;

            //Thread t1 = new Thread(
            //    () => UseMedianFilter(0, (image.Height / 2) + borderSize, image, areaSize / 2, areaSize));
            //t1.Start();

            //Thread t2 = new Thread(
            //    () => UseMedianFilter((image.Height / 2) - borderSize, image.Height, image, areaSize / 2, areaSize));


            // (0 * (image.Height / 3))                      (1 * (image.Height / 3)) + borderSize
            // (1 *(image.Height / 3)) - borderSize          (2 * (image.Height / 3)) + borderSize
            // (2 *(image.Height / 3)) - borderSize          (3 * (image.Height / 3))

            for (int i = 0; i < threadAmount; i++)
            {
                var start = (i * (image.Height / threadAmount)) - borderSize;
                if (i == 0)
                    start = 0;


                var end = ((i + 1) * (image.Height / threadAmount)) + borderSize;
                if (i == (threadAmount -1))
                    end = image.Height;
                var a = "";
                //Thread thread = new Thread
                //    (() => UseMedianFilter(start, end, image, borderSize, areaSize));
            }


            //t2.Start();
            //t1.Join();
            //t2.Join();




            newImage.Save("C:/Users/Anuitex/source/repos/TRStPO/TRStPO/2.jpg");
        }

        public static void UseMedianFilter(int start, int end, Bitmap image, int borderSize, int areaSize)
        {
            for (int x = borderSize; x < image.Width - borderSize; x++)
            {
                for (int y = start + borderSize; y < (end - borderSize); y++)
                {
                    var colorArray = new List<Color>();

                    for (int i = x - borderSize; i < areaSize + x - borderSize; i++)
                    {
                        for (int j = y - borderSize; j < areaSize + y - borderSize; j++)
                        {
                            mutexObj.WaitOne();
                            Color color = image.GetPixel(i, j);
                            mutexObj.ReleaseMutex();
                            colorArray.Add(color);
                        }
                    }
                    mutexObj.WaitOne();
                    newImage.SetPixel(x, y, GetMedianPixel(colorArray));
                    mutexObj.ReleaseMutex();
                }
            }
        }

        public static Color GetMedianPixel(List<Color> colors)
        {
            var red = new List<byte>();
            var green = new List<byte>();
            var blue = new List<byte>();

            foreach (var color in colors)
            {
                red.Add(color.R);
                green.Add(color.G);
                blue.Add(color.B);
            }

            red.Sort();
            green.Sort();
            blue.Sort();

            var medianColor = Color.FromArgb(red[40], green[40], blue[40]);

            return medianColor;
        }
    }
}
