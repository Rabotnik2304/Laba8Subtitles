using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;

namespace Laba_8
{
    public class SubtitleData
    {   
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public string Position { get; set; }
        public string Color { get; set;}
        public string Text { get; set; }
    }

    class Program
    {
        
        static int Timer = 0;
        static List<SubtitleData> ListOfSubtitles = new List<SubtitleData>();
        static int Length = 60;
        static int Height = 20;

        static void Main()
        {
            //Insert(); - Для удобства проверки создаёт нужный файлик
            SubtitlesDataRecording();
            MakeScreen();

            Timer aTimer = CreateTimer(); 
            while (true)
            {
                aTimer.Start();
            }
        }

        private static Timer CreateTimer()
        {
            var timer = new Timer(1000);
            timer.Elapsed += SubtitleReturn;

            return timer;
        }

        private static void WriteSubtitle(SubtitleData subtitle)
        {
            SetCursorPosition(subtitle);
            SetColor(subtitle);
            Console.Write(subtitle.Text);
        }

        private static void DeleteSubtitle(SubtitleData subtitle)
        {
            SetCursorPosition(subtitle);
            for(int i = 0; i < subtitle.Text.Length; i++)
            {
                Console.Write(" ");
            }
        }

        

        static void SubtitleReturn(Object source, ElapsedEventArgs e)
        {
            foreach (var subtitle in ListOfSubtitles)
            {
                if (subtitle.StartTime == Timer)
                    WriteSubtitle(subtitle);
                else if (subtitle.EndTime == Timer)
                    DeleteSubtitle(subtitle);
            }
            Timer++;
        }

        private static void SetColor(SubtitleData subtitle)
        {
            switch (subtitle.Color)
            {
                case "Red":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "Green":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "Blue":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case "White":
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                default:
                    break;
            }
        }

        private static void SetCursorPosition(SubtitleData subtitle)
        {
            switch (subtitle.Position)
            {
                case "Top":
                    Console.SetCursorPosition((Length - subtitle.Text.Length) / 2, 1);
                    break;
                case "Bottom":
                    Console.SetCursorPosition((Length - subtitle.Text.Length) / 2, Height - 1);
                    break;
                case "Right":
                    Console.SetCursorPosition(Length - 1 - subtitle.Text.Length, Height / 2);
                    break;
                case "Left":
                    Console.SetCursorPosition(1, Height / 2);
                    break;
                default:
                    break;
            }
        }

        private static void MakeScreen()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write("+");

            for (int i = 1; i < Length - 1; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("-");
            }

            Console.SetCursorPosition(Length - 1, 0);
            Console.Write("+");

            for (int i = 1; i < Height; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("|");

                Console.SetCursorPosition(Length - 1, i);
                Console.Write("|");
            }

            Console.SetCursorPosition(0, Height);
            Console.Write("+");

            for (int i = 1; i < Length - 1; i++)
            {
                Console.SetCursorPosition(i, Height);
                Console.Write("-");
            }

            Console.SetCursorPosition(Length - 1, Height);
            Console.Write("+");
        }

        static void SubtitlesDataRecording()
        {
            string[] strings = File.ReadAllLines(".Subtitles.txt");

            foreach (string s in strings)
            {
                SubtitleData subtitle = new SubtitleData();
                string[] subtitlesData = s.Split(new char[] { '[', ']' });

                if (subtitlesData.Length > 1) // в субтитрах есть указание места и цвета
                {
                    string StartTimeMinAndSec = subtitlesData[0].Split('-')[0].Trim(' ');
                    subtitle.StartTime = 60*int.Parse(StartTimeMinAndSec.Split(':')[0])+ int.Parse(StartTimeMinAndSec.Split(':')[1]);

                    string EndTimeMinAndSec = subtitlesData[0].Split('-')[1].Trim(' ');
                    subtitle.EndTime = 60 * int.Parse(EndTimeMinAndSec.Split(':')[0]) + int.Parse(EndTimeMinAndSec.Split(':')[1]);

                    subtitle.Position = subtitlesData[1].Split(new char[] { ' ', ',' })[0].Trim(' ');
                    subtitle.Color = subtitlesData[1].Split(new char[] { ' ', ',' })[2].Trim(' ');
                    subtitle.Text = subtitlesData[2].Trim(' ');
                }
                else
                {
                    string StartTimeMinAndSec = subtitlesData[0].Substring(0, 13).Split('-')[0].Trim(' ');
                    subtitle.StartTime = 60 * int.Parse(StartTimeMinAndSec.Split(':')[0]) + int.Parse(StartTimeMinAndSec.Split(':')[1]);

                    string EndTimeMinAndSec = subtitlesData[0].Substring(0, 13).Split('-')[1].Trim(' ');
                    subtitle.EndTime = 60 * int.Parse(EndTimeMinAndSec.Split(':')[0]) + int.Parse(EndTimeMinAndSec.Split(':')[1]);

                    subtitle.Position = "Bottom";
                    subtitle.Color = "White";
                    subtitle.Text = subtitlesData[0].Substring(14).Trim(' ');
                }

                ListOfSubtitles.Add(subtitle);
            }
        }

        static void Insert()
        {
            string[] array0 = new string[] { 
            "00:01 - 00:03 [Top, Red] Hello",
            "00:01 - 00:03 [Bottom, Red] World",
            "00:02 - 00:06 [Right, Green] Yes",
            "00:04 - 00:07 [Left, Blue] No",
            "00:07 - 00:15 Bill is a very motivated young man"};
            File.WriteAllLines(".Subtitles.txt", array0);
        }
    }
}