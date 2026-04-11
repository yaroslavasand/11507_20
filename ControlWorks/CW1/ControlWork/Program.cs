using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ControlWork8
{
    // ЗАДАНИЕ 1
    public static class ObjectFactory
    {
        public static T Create<T>() where T : new()
        {
            return new T();
        }

        public static List<T> CreateList<T>(int count) where T : new()
        {
            if (count < 0)
                return new List<T>();

            var list = new List<T>();
            for (int i = 0; i < count; i++)
            {
                list.Add(new T());
            }

            return list;
        }

        public static T CreateAndAction<T>(Action<T> initializer) where T : new()
        {
            T obj = new T();
            initializer?.Invoke(obj);
            return obj;
        }
    }

    // Класс для демонстрации
    public class Tool
    {
        public string Name { get; set; }

        public Tool()
        {
            Name = "Default Tool";
        }

        public Tool(string name)
        {
            Name = name;
        }
    }

    public class Worker
    {
        public string Name { get; set; }

        public Worker()
        {
            Name = "Default Worker";
        }

        public Worker(string name)
        {
            Name = name;
        }
    }

    // ЗАДАНИЕ 2
    public class PrinterEventArgs : EventArgs
    {
        public int PagesRequested { get; set; }
        public int PagesAvailable { get; set; }
    }

    public class Printer
    {
        private int _paperAmount = 10;
        public event EventHandler<PrinterEventArgs> PaperOut;

        public void Print(int pages)
        {
            if (pages > _paperAmount)
            {
                OnPaperOut(new PrinterEventArgs
                {
                    PagesRequested = pages,
                    PagesAvailable = _paperAmount
                });
                return;
            }

            _paperAmount -= pages;
            Console.WriteLine($"Печать {pages} страниц выполнена. Осталось бумаги: {_paperAmount}");
        }

        protected virtual void OnPaperOut(PrinterEventArgs e)
        {
            PaperOut?.Invoke(this, e);
        }

        public void Refill(int amount)
        {
            _paperAmount += amount;
            Console.WriteLine($"Добавлено {amount} листов. Теперь бумаги: {_paperAmount}");
        }

        public int GetPaperAmount() => _paperAmount;
    }

    //ЗАДАНИЕ 3
    public class FileAnalysisResult
    {
        public string Extension { get; set; }
        public string FileName { get; set; }
        public double SizeMB { get; set; }
        public long SizeBytes { get; set; }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("ЗАДАНИЕ 1");
            
            // 1. Создание одного объекта Tool
            Tool tool = ObjectFactory.Create<Tool>();
            Console.WriteLine($"Создан объект типа: {tool.GetType().Name}, Name = {tool.Name}");
            
            // 2. Создание списка из 3 объектов Worker
            List<Worker> workers = ObjectFactory.CreateList<Worker>(3);
            Console.WriteLine($"Создан список из {workers.Count} объектов типа Worker");
            for (int i = 0; i < workers.Count; i++)
            {
                Console.WriteLine($"  Worker {i + 1}: {workers[i].Name}");
            }
            
            // 3. Создание с инициализацией
            Tool customTool = ObjectFactory.CreateAndAction<Tool>(t => t.Name = "Hammer");
            Console.WriteLine($"Создан Tool с инициализацией: Name = {customTool.Name}");
            
            Console.WriteLine("\nЗАДАНИЕ 2");
            
            Printer printer = new Printer();
            
            printer.PaperOut += (sender, e) =>
            {
                Console.WriteLine($"ОШИБКА ПЕЧАТИ! Запрошено: {e.PagesRequested} стр., доступно: {e.PagesAvailable} стр.");
            };
            
            Console.WriteLine($"Начальное количество бумаги: {printer.GetPaperAmount()}");
            
            Console.WriteLine("\nПопытка напечатать 15 страниц:");
            printer.Print(15);
            
            Console.WriteLine("\nЗаправка принтера:");
            printer.Refill(10);
            
            Console.WriteLine("\nПопытка напечатать 12 страниц:");
            printer.Print(12);
            
            Console.WriteLine($"\nОсталось бумаги: {printer.GetPaperAmount()}");
             Console.WriteLine("\nЗАДАНИЕ 3");
             
            string directoryPath = @"C:\Windows";
            
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    var top5Files = AnalyzeDirectory(directoryPath);
                    
                    Console.WriteLine($"Топ-5 самых больших файлов в директории: {directoryPath}\n");
                    
                    foreach (var file in top5Files)
                    {
                        Console.WriteLine($"{file.Extension}: {file.FileName} - [{file.SizeMB:F2}] MB");
                    }
                }
                else
                {
                    Console.WriteLine($"Директория не существует: {directoryPath}");
                    Console.WriteLine("Попробуйте использовать другую директорию, например C:\\Downloads");
                    
                    string altPath = Directory.GetCurrentDirectory();
                    Console.WriteLine($"\nИспользуем текущую директорию: {altPath}");
                    var top5Files = AnalyzeDirectory(altPath);
                    
                    foreach (var file in top5Files)
                    {
                        Console.WriteLine($"{file.Extension}: {file.FileName} - [{file.SizeMB:F2}] MB");
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Нет доступа к этой директории. Попробуйте другую папку.");
                Console.WriteLine($"Например: {Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}");
                
                string docsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var top5Files = AnalyzeDirectory(docsPath);
                
                foreach (var file in top5Files)
                {
                    Console.WriteLine($"{file.Extension}: {file.FileName} - [{file.SizeMB:F2}] MB");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            
        }
        
        static List<FileAnalysisResult> AnalyzeDirectory(string path)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                FileInfo[] files = dirInfo.GetFiles();
                
                if (files.Length == 0)
                {
                    Console.WriteLine("В директории нет файлов.");
                    return new List<FileAnalysisResult>();
                }
                
                var result = files
                    .Where(f => !string.IsNullOrEmpty(f.Extension))
                    .GroupBy(f => f.Extension.ToLower())
                    .Select(group => new FileAnalysisResult
                    {
                        Extension = group.Key,
                        FileName = group.OrderByDescending(f => f.Length).First().Name,
                        SizeMB = group.OrderByDescending(f => f.Length).First().Length / (1024.0 * 1024.0),
                        SizeBytes = group.OrderByDescending(f => f.Length).First().Length
                    })
                    .OrderByDescending(r => r.SizeBytes)
                    .Take(5)
                    .ToList();
                
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при анализе директории: {ex.Message}");
                return new List<FileAnalysisResult>();
            }
        }
    }
}
        
        
    