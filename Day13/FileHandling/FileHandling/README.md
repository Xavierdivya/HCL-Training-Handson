# FileHandling – C# File Operations

## 📌 Overview

This project is a C# console application designed to practice file handling operations using the `System.IO` namespace. It demonstrates how to create and write text files, retrieve file information, and explore operations such as reading, appending, copying, moving, and deleting files.

## 🎯 Objectives

- Understand file handling in C#.
- Create and write content to text files.
- Read and append content to files.
- Retrieve file properties using the `FileInfo` class.
- Practice copying, moving, and deleting files.
- Check whether a file exists and whether it is read-only.

## 🛠️ Technologies Used

- **Language:** C#
- **Framework:** .NET
- **Namespace:** `System.IO`
- **Application Type:** Console Application
- **IDE:** Visual Studio

## 📂 Project Structure

```text
FileHandling/
├── Program.cs
├── FileExercise.cs
├── FileHandling.csproj
└── README.md
```

*Note: The source files may be organized differently in your project.*

## 📚 Concepts Practiced

### 1. Writing to a File

Uses `File.WriteAllText()` to create a text file and write content to it.

```csharp
string path = "Demo1.txt";
string content = "Hello World! , Iam divya";

File.WriteAllText(path, content);
```

If the file already exists, its previous content is overwritten.

### 2. Appending Content

Uses `File.AppendAllText()` to add new content to the end of an existing file without replacing its current content.

```csharp
File.AppendAllText(path, "\nIm blessy");
```

### 3. Reading a File

Uses `File.ReadAllText()` to read the complete contents of a text file.

```csharp
string fileContent = File.ReadAllText(path);
Console.WriteLine(fileContent);
```

### 4. Retrieving File Information

Uses the `FileInfo` class to obtain information about `Demo1.txt`.

Properties demonstrated:

- `Exists` – Checks whether the file exists.
- `CreationTime` – Gets the file creation time.
- `IsReadOnly` – Checks whether the file is read-only.
- `Name` – Gets the file name.
- `FullName` – Gets the full path of the file.
- `Length` – Gets the file size in bytes.
- `LastWriteTime` – Gets the last modification time.
- `Extension` – Gets the file extension.
- `DirectoryName` – Gets the parent directory path.

### 5. Copying a File

Uses `FileInfo.CopyTo()` to copy a file to another location or filename.

```csharp
f1.CopyTo("Demo2.txt");
```

To allow overwriting an existing destination file:

```csharp
f1.CopyTo("Demo2.txt", true);
```

### 6. Moving and Deleting Files

The practice also includes examples of:

- `FileInfo.MoveTo()` – Moves or renames a file.
- `FileInfo.Delete()` – Deletes a file.
- `FileInfo.Create()` – Creates a file.

These operations are currently commented out in the source code and can be uncommented for additional practice.

## ▶️ How to Run the Project

1. Open the `FileHandling` project in Visual Studio.
2. Ensure the project builds successfully.
3. Run the application using **Ctrl + F5**.
4. Check the console output for file information.
5. Locate `Demo1.txt` in the application's working directory to inspect the generated file.

## 🖥️ Expected Output

The program displays information about `Demo1.txt`, including:

- Whether the file exists.
- Its creation date and time.
- Whether it is read-only.
- Its file name.
- Its full path.

The exact output depends on the computer and the file's location.

## 💡 Key Learning Outcomes

After completing this practice, you will understand:

- Basic file input and output operations.
- The difference between writing, appending, and reading files.
- How to use `File` and `FileInfo`.
- How to inspect file properties.
- How to copy, move, and delete files.
- How to work with text files in .NET applications.

## 🚀 Conclusion

This project provides practical experience with C# file handling and file metadata. These concepts are useful when developing applications that store, retrieve, manage, and process data using files.
