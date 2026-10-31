using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace JsonHomework
{

    public class PublishingHouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
    }

    public class Book
    {
        public int PublishingHouseId { get; set; }
        public string Title { get; set; }
        public PublishingHouse PublishingHouse { get; set; }
    }


    public class BookOut
    {
        [JsonIgnore]
        public int PublishingHouseId { get; set; }

        [JsonPropertyName("Name")]
        public string Title { get; set; }

        public PublishingHouse PublishingHouse { get; set; }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            string jsonString = File.ReadAllText("data.json", Encoding.UTF8);
            List<Book> books = JsonSerializer.Deserialize<List<Book>>(jsonString);

            Console.WriteLine("=== Пункт 1: дані після десеріалізації ===");
            foreach (Book book in books)
            {
                Console.WriteLine(
                    $"PublishingHouseId: {book.PublishingHouseId} | " +
                    $"Title: {book.Title} | " +
                    $"Видавництво: {book.PublishingHouse.Name} ({book.PublishingHouse.Adress})");
            }

            List<BookOut> booksOut = books.Select(b => new BookOut
            {
                PublishingHouseId = b.PublishingHouseId,
                Title = b.Title,
                PublishingHouse = b.PublishingHouse
            }).ToList();

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };

            string newJson = JsonSerializer.Serialize(booksOut, options);

            Console.WriteLine("\n=== Пункти 2 і 3: JSON без PublishingHouseId, Title -> Name ===");
            Console.WriteLine(newJson);
        }
    }
}