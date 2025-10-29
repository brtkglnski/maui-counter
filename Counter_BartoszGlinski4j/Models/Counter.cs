using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Counter_BartoszGlinski4j.Models
{
    internal class Counter
    {
        public string Filename { get; set; }
        public string Name { get; set; }

        public int Amount { get; set; }

        public int InitialAmount { get; set; }

        public DateTime Date { get; set; }

        public Counter()
        {
            Filename = $"{Path.GetRandomFileName()}.counter.txt";
            Date = DateTime.Now;
            Name = string.Empty;
            InitialAmount = InitialAmount;
            Amount = InitialAmount;
        }

        public Counter(string name, int initialAmount, string color = "#FFFFFF")
        {
            Filename = $"{Path.GetRandomFileName()}.counter.txt";
            Date = DateTime.Now;
            Name = name;
            InitialAmount = initialAmount;
            Amount = InitialAmount;
        }

        public void Save()
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, Filename);
            string json = JsonSerializer.Serialize(this);
            File.WriteAllText(path, json);
        }

        public static Counter Load(string filename)
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, filename);

            if (!File.Exists(path))
            {
                return new Counter();
            }

            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Counter>(json);
        }

        public static IEnumerable<Counter> LoadAll()
        {
            string appDataPath = FileSystem.AppDataDirectory;
            return Directory
                .EnumerateFiles(appDataPath, "*.counter.txt")
                .Select(filename => Counter.Load(Path.GetFileName(filename)))
                .OrderByDescending(counter => counter.Date);
        }
        public void Reset()
        {
            Amount = InitialAmount;
            Save();
        }

        public void Remove()
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, Filename);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
