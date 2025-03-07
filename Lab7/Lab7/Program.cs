using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Lab7
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("You did not give a path!");
                return;
            }

            
            string path = args[0];

            if(!Directory.Exists(path))
            {
                Console.WriteLine("Path is not exist!");
                return;
            }
            printFromPath(path);


            SortedDictionary<string, double> dictionary = CreateDirectory(path);
            foreach(var item in dictionary)
            {
                Console.WriteLine(item.Key + " -> " + item.Value);
            }
            Console.WriteLine("Oldest file: " + GetOldestDate(path));


            FileStream fileStream = new FileStream("collection.bin", FileMode.Create);
            Serialization(dictionary, fileStream);

            fileStream.Position = 0;

            SortedDictionary<string, double> newDictionary = Deserialization(fileStream);
            foreach(var item in newDictionary)
            {
                Console.WriteLine(item.Key + " -> " + item.Value);
            }
            fileStream.Close();
        }

        public static SortedDictionary<string, double> CreateDirectory(string path)
        {
            SortedDictionary<string, double> dic = new SortedDictionary<string, double>(new Comparator());

            addToDictionary(dic, path);

            return dic;
        }

        public static void addToDictionary(SortedDictionary<string, double> dic, string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            foreach (var file in directoryInfo.GetFiles())
            {
                double size = Math.Round(file.Length/1.0, 2);
                dic.Add(file.Name, size);
            }
            foreach (var directory in directoryInfo.GetDirectories())
            {
                addToDictionary(dic, directory.FullName);
                dic.Add(directory.Name, countSubdirectories(directory.FullName));
            }
        }
        public static void printFiles(FileInfo[] files, string section)
        {
            foreach(var file in files)
            {
                Console.WriteLine(section + file.Name + " " + file.LastWriteTime + " " + $"{(file.Length):F2}" + " b " + GetDosAttributes(file));
            }
        }

        public static void printDirectories(DirectoryInfo[] directories, string section)
        {
            foreach (var dir in directories)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(dir.FullName);
                Console.WriteLine(section + dir.Name + " " + dir.LastWriteTime + " (" + countSubdirectories(dir.FullName) + ") " + GetDosAttributes(dir));
                printFiles(directoryInfo.GetFiles(), section + "   ");
                printDirectories(directoryInfo.GetDirectories(), section + "   ");
            }
        }

        public static void printFromPath(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            printFiles(directoryInfo.GetFiles(), "");
            printDirectories(directoryInfo.GetDirectories(), "");
        }

        public static DateTime GetOldestDate(string path)
        {
            DateTime date = DateTime.MaxValue;
            
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            

            foreach (var file in directoryInfo.GetFiles())
            {
                DateTime currentDate = file.LastWriteTime;
                if(currentDate < date)
                {
                    date = currentDate;
                }
            }

            foreach (var dir in directoryInfo.GetDirectories())
            {
                DateTime currentDate = dir.LastWriteTime;
                if(currentDate < date) {
                    date = currentDate;
                }
                DateTime subdirectories = GetOldestDate(dir.FullName);
                if(date > subdirectories)
                {
                    date = subdirectories;
                }
            }
            return date;
        }

        public static string GetDosAttributes(FileSystemInfo fileSystemInfo)
        {
            string attributes = "";
            if ((fileSystemInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                attributes += "r";
            }
            else
            {
                attributes += "-";
            }
            if ((fileSystemInfo.Attributes & FileAttributes.Archive) == FileAttributes.Archive)
            {
                attributes += "a";
            }
            else
            {
                attributes += "-";
            }
            if ((fileSystemInfo.Attributes & FileAttributes.System) == FileAttributes.System)
            {
                attributes += "s";
            }
            else
            {
                attributes += "-";
            }
            if ((fileSystemInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                attributes += "h";
            }
            else
            {
                attributes += "-";
            }
            

            return attributes;
        }
        
        public static int countSubdirectories(string path)
        {
            int count = 0;
            
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            count = directoryInfo.GetFiles().Length + directoryInfo.GetDirectories().Length;

            foreach(var dir in directoryInfo.GetDirectories())
            {
                count += countSubdirectories(dir.FullName);
            }

            return count;
        }

        public static void Serialization(SortedDictionary<string,double> dictionary, FileStream fileStream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fileStream, dictionary);
            Console.WriteLine("Dictionary serialized!");
        }

        public static SortedDictionary<string, double> Deserialization(FileStream fileStream)
        {
            SortedDictionary<string, double> dic = new SortedDictionary<string, double>(new Comparator());
            BinaryFormatter formatter = new BinaryFormatter();
            dic = (SortedDictionary<string,double>)formatter.Deserialize(fileStream);
            Console.WriteLine("Dictionary deserialized!");
            return dic;
        }
    }

}
