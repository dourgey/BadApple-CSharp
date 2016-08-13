using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using Un4seen.Bass;

namespace BmpTest
{
    class Program
    {
        static string[] fileName = GetFileNameArray("BadApple\\", 5477, ".bmp");  //    取得文件名Array
        static void Main(string[] args)
        {
            //控制台样式初始化
            Console.Title="BadApple Version-CSharp";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Magenta;
            /*Bass库初始化*/
            if (!Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_CPSPEAKERS, new IntPtr(0)))
            {
                Console.WriteLine("Bass初始化失败！" + Bass.BASS_ErrorGetCode().ToString());
                System.Environment.Exit(0);
            }
            /*创建音频流并播放*/
            int stream = Bass.BASS_StreamCreateFile("BadApple.mp3", 0L, 0L, BASSFlag.BASS_SAMPLE_FLOAT);
            Bass.BASS_ChannelPlay(stream, true);
            int oldTime = Environment.TickCount;     //获取初始播放点时间戳
            int newTime=0; //当前播放点时间戳
            while ((newTime - oldTime) / 40<5478)
            {
                newTime = Environment.TickCount;  //获取当前时间戳
                if ((newTime - oldTime)/40 > 5476)
                    break;
                GetBmpArray(fileName[(newTime-oldTime)/40]);  //每隔40秒输出一帧图像
                Console.SetCursorPosition(0, 0);  //将光标移动到(0,0)的位置
            }
            Console.SetCursorPosition(20, 20);
            Console.WriteLine("播放完毕，谢谢观赏！");
            Console.SetCursorPosition(21, 25);
            Console.WriteLine("按任意键退出");
            Console.ReadKey();
        }
        static string[] GetFileNameArray(string filePath,int num, string extension)
        {
            string[] fileNameArray = new string[num];
            /*文件名--拼接字符串返回字符串数组*/
            for (int i = 0; i < num; i++)
            {
                fileNameArray[i] = filePath + (i + 1).ToString() + extension;
            }
            return fileNameArray;
        }

        /*Bmp文件读取到二维数组，取像素点红色分量为基准值并一次输出一帧*/
        static void GetBmpArray(string filePath)
        {
            Bitmap bmp = new Bitmap(filePath);
            double[,] bmpArray = new double[bmp.Width, bmp.Height];
            /*将像素点的红色分量读取到数组*/
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    bmpArray[i, j] = bmp.GetPixel(i, j).R;
                }
            }
            /*输出一帧*/
            for (int j = 0; j < bmp.Height; j +=2)
            {
                for (int i = 0; i < bmp.Width; i+=2)
                {
                    if (bmpArray[i, j] == 0)
                        Console.Write("█");
                    else
                        Console.Write("□");
                }
                Console.WriteLine();
            }
        }
    }
}
