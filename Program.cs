using System;
using System.IO;
using System.Globalization;

namespace MeterReadingsParser
{
    public class MeterReading
    {
        private readonly string _resourceType;
        private readonly DateTime _date;
        private readonly double _value;

        public MeterReading(string resourceType, DateTime date, double value)
        {
            if (string.IsNullOrWhiteSpace(resourceType))
                throw new ArgumentException("Тип ресурса не может быть пустым");

            if (value < 0)
                throw new ArgumentException("Значение не может быть отрицательным");

            _resourceType = resourceType;
            _date = date;
            _value = value;
        }

        public string ResourceType => _resourceType;
        public DateTime Date => _date;
        public double Value => _value;

        public override string ToString()
        {
            return $"Тип ресурса: {ResourceType}, Дата: {Date:yyyy.MM.dd}, Значение: {Value}";
        }
    }

    public class MeterReadingParser
    {
        public static MeterReading Parse(string input)
        {
            var parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 3)
                throw new FormatException("Некорректный формат входной строки");

            string resourceType = parts[0].Trim('"');

            if (!DateTime.TryParseExact(parts[1], "yyyy.MM.dd", null,
                DateTimeStyles.None, out DateTime date))
                throw new FormatException("Некорректный формат даты");

            if (!double.TryParse(parts[2], NumberStyles.Any,
                CultureInfo.InvariantCulture, out double value))
                throw new FormatException("Некорректный формат значения");

            return new MeterReading(resourceType, date, value);
        }
    }

    public class Program
    {
        private const string FilePath = @"D:\Storage\8 семестр\тимп\ЛР1\lr1.txt";

        public static void Main()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    Console.WriteLine($"Файл не найден: {FilePath}");
                    return;
                }

                string[] lines = File.ReadAllLines(FilePath);

                foreach (string line in lines)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        MeterReading reading = MeterReadingParser.Parse(line);
                        Console.WriteLine(reading);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при обработке строки '{line}': {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критическая ошибка: {ex.Message}");
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}