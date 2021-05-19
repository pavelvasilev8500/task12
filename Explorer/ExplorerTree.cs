using System;
using System.IO;

namespace Explorer
{
    class ExplorerTree
    {
        private static int currentLevel = 0;
        private static int Index = 0;
        private static int selectedIndex = 0;
        private static DriveInfo[] drives;
        private static string[] files;
        private static string prefix;

        private static void SetPrefix(int i, int Index)
        {
            if (i == Index)
            {
                prefix = ">";
            }
            else
            {
                prefix = " ";
            }
        }

        private static int GetGrivies()
        {
            ConsoleKey keyPressed;
            do
            {
                Console.Clear();
                drives = DriveInfo.GetDrives();
                for (int i = 0; i < drives.Length; i++)
                {
                    SetPrefix(i, Index);
                    Console.WriteLine(prefix + drives[i]);
                }
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;
                if (keyPressed == ConsoleKey.UpArrow)
                {
                    Index--;
                    if (Index == -1)
                    {
                        Index = 0;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    Index++;
                    if (Index == drives.Length)
                    {
                        Index = drives.Length - 1;
                    }
                }
            }
            while (keyPressed != ConsoleKey.Enter);
            return Index;
        }

        private static void GetDirectory(string path, int maxLevel, int currentLevel)
        {
            if (currentLevel >= maxLevel)
            {
                return;
            }
            string indent = new string(' ', currentLevel);
            try
            {
                string[] directory = Directory.GetDirectories(path);
                selectedIndex = GetFiles(path);
                if (selectedIndex == Index)
                {
                    try
                    {

                        using (FileStream fstream = File.OpenRead(files[selectedIndex].ToString()))
                        {
                            Console.Clear();
                            byte[] array = new byte[fstream.Length];
                            fstream.Read(array, 0, array.Length);
                            string textFromFile = System.Text.Encoding.Default.GetString(array);
                            Console.WriteLine($"Текст из файла: {textFromFile}");
                            Console.ReadKey();
                        }
                    }
                    catch
                    {
                        Console.WriteLine(indent + "Can’t access");
                        Console.ReadKey();
                    }
                }
                for (int i = 0; i < directory.Length; i++)
                {
                    Index = 0;
                    ConsoleKey keyPressed;
                    do
                    {
                        Console.Clear();
                        for (i = 0; i < directory.Length; i++)
                        {
                            SetPrefix(i, Index);
                            Console.WriteLine(prefix + directory[i]);
                        }
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        keyPressed = keyInfo.Key;
                        if (keyPressed == ConsoleKey.UpArrow)
                        {
                            Index--;
                            if (Index == -1)
                            {
                                Index = 0;
                            }
                        }
                        else if (keyPressed == ConsoleKey.DownArrow)
                        {
                            Index++;
                            if (Index == directory.Length)
                            {
                                Index = 0;
                            }
                        }
                    }
                    while (keyPressed != ConsoleKey.Enter);
                    GetDirectory(directory[Index].ToString(), maxLevel, currentLevel + 1);
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine(indent + "Can’t access");
                return;
            }

            catch (DirectoryNotFoundException)
            {
                Console.WriteLine(indent + "Can’t find");
                return;
            }
        }

        private static int GetFiles(string path)
        {
            files = Directory.GetFiles(path);
            Index = 0;
            ConsoleKey keyPressed;
            do
            {
                Console.Clear();
                for (int j = 0; j < files.Length; j++)
                {
                    SetPrefix(j, Index);
                    Console.WriteLine(prefix + files[j]);
                }
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;
                if (keyPressed == ConsoleKey.UpArrow)
                {
                    Index--;
                    if (Index == -1)
                    {
                        Index = 0;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    Index++;
                    if (Index == files.Length)
                    {
                        Index = 0;
                    }
                }
            }
            while (keyPressed != ConsoleKey.Enter);
            return Index;
        }

        public void Start()
        {
            selectedIndex = GetGrivies();
            GetDirectory(drives[selectedIndex].ToString(), 5, currentLevel);
        }
    }
}
