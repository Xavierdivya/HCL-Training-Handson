using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace FileHandling;

    internal class FileExercise
    {
    public void Run() { 

        string? path = "Demo1.txt";
            string? content = "Hello World! , Iam divya";
            File.WriteAllText(path,content);
        //    File.AppendAllText(path, "\nIm blessy");

        //    string? fileContent = File.ReadAllText(path);
        //    Console.WriteLine(fileContent);
        //    Console.WriteLine("File Opration implemented successfully");
       string? path2 = "Demo2.txt";
        string? path3 = "Demo3.txt";
    FileInfo f1 = new FileInfo(path);
        Console.WriteLine((f1.Exists)); 
        Console.WriteLine(f1.CreationTime.ToString());
        Console.WriteLine(f1.IsReadOnly);
        Console.WriteLine(f1.Name);
        Console.WriteLine(f1.FullName);
        ///Console.WriteLine(f1.Length);
        //Console.WriteLine(f1.LastWriteTime);
        //Console.WriteLine(f1.Extension);
        //Console.WriteLine(f1.DirectoryName);
        //Console.WriteLine(f1.Directory);
       // f1.CopyTo(path2);
        //FileInfo f2 = new FileInfo(path2);
        //f2.Delete();
        //f1.CopyTo(path2, true);
        //FileInfo f3 = new FileInfo(path3);
        ////f1.MoveTo(path3);
        //f3.Create(path);

    }
    }

