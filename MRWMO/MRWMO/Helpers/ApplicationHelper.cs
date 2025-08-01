using MRWMO.Enums;
using MRWMO.Models;
using System.Reflection;
using System.Xml.Linq;

namespace MRWMO.Helpers
{
    public class ApplicationHelper
    {
        public static string GetSinhalaContent(int bookId, int chapterId)
        {
            var xmalFileName = string.Format("content_of_book{0}.xml", bookId);
            XDocument xdoc = GetXDocument(xmalFileName).Result;

            var contents = xdoc.Descendants("Content").ToList();
            var content = contents.Where(c => c.Attribute("name").Value == string.Format("book{0}chapter{1}", bookId, chapterId)).FirstOrDefault().Value;
           
            return content;
        }

        public static string GetEnglishContent(int bookId, int chapterId)
        {
            var content = GetChaptersByEnglishBookId(bookId).Where(c => c.Id == chapterId).FirstOrDefault().Content;
            return content;
        }

        public static IList<Book> GetAllSinhalaBooks()
        {
            var books = new List<Book>();

            var book1 = new Book { Id = 1, Name = "අතහැරීම ලිපි 1", Image = "ic_book1.jpg", LanguageId = (int)LanguageEnum.Sinhala };
            books.Add(book1);

            var book2 = new Book { Id = 2, Name = "අතහැරීම ලිපි 2", Image = "ic_book2.jpg", LanguageId = (int)LanguageEnum.Sinhala };
            books.Add(book2);

            var book3 = new Book { Id = 3, Name = "අතහැරීම ලිපි 3", Image = "ic_book3.jpg", LanguageId = (int)LanguageEnum.Sinhala };
            books.Add(book3);

            var book4 = new Book { Id = 4, Name = "අතහැරීම ලිපි 4", Image = "ic_book4.jpg", LanguageId = (int)LanguageEnum.Sinhala };
            books.Add(book4);

            var book5 = new Book { Id = 5, Name = "අතහැරීම ලිපි 5", Image = "ic_book5.jpg", LanguageId = (int)LanguageEnum.Sinhala };
            books.Add(book5);

            var book6 = new Book { Id = 6, Name = "අතහැරීම ලිපි 6", Image = "ic_book6.jpg", LanguageId = (int)LanguageEnum.Sinhala };
            books.Add(book6);

            var book7 = new Book { Id = 7, Name = "අතහැරීම ලිපි 7", Image = "ic_book7.jpg", LanguageId = (int)LanguageEnum.Sinhala };
            books.Add(book7);

            var book8 = new Book { Id = 8, Name = "අතහැරීම ලිපි 8", Image = "ic_book8.jpg", LanguageId = (int)LanguageEnum.Sinhala };
            books.Add(book8);

            var book9 = new Book { Id = 9, Name = "අතහැරීම ලිපි 9", Image = "ic_book9.jpg", LanguageId = (int)LanguageEnum.Sinhala };
            books.Add(book9);

            var book10 = new Book { Id = 10, Name = "අතහැරීම ලිපි 10", Image = "ic_book10.jpg", LanguageId = (int)LanguageEnum.Sinhala };
            books.Add(book10);

            var book11 = new Book { Id = 11, Name = "අතහැරීම ලිපි 11", Image = "ic_book11.jpg", LanguageId = (int)LanguageEnum.Sinhala };
            books.Add(book11);

            var book12 = new Book { Id = 12, Name = "අතහැරීම ලිපි 12", Image = "ic_book12.jpg", LanguageId = (int)LanguageEnum.Sinhala };
            books.Add(book12);

            var book13 = new Book { Id = 13, Name = "අතහැරීම ලිපි 13", Image = "ic_book13.jpg", LanguageId = (int)LanguageEnum.Sinhala };
            books.Add(book13);

            var book14 = new Book { Id = 14, Name = "අතහැරීම ලිපි 14", Image = "ic_book14.jpg", LanguageId = (int)LanguageEnum.Sinhala };
            books.Add(book14);

            var book15 = new Book { Id = 15, Name = "අතහැරීම ලිපි 15", Image = "ic_book15.jpg", LanguageId = (int)LanguageEnum.Sinhala };
            books.Add(book15);

            var book16 = new Book { Id = 16, Name = "අතහැරීම ලිපි 16", Image = "ic_book16.jpg", LanguageId = (int)LanguageEnum.Sinhala };
            books.Add(book16);

            return books;
        }

        public static IList<Book> GetAllEnglishBooks()
        {
            var books = new List<Book>();

            var book1 = new Book { Id = 1, Name = "Giving Up 1", Image = "e01.png",LanguageId = (int)LanguageEnum.English };
            books.Add(book1);

            var book4 = new Book { Id = 4, Name = "Giving Up 4", Image = "e04.png", LanguageId = (int)LanguageEnum.English };
            books.Add(book4);

            var book5 = new Book { Id = 5, Name = "Giving Up 5", Image = "e05.png", LanguageId = (int)LanguageEnum.English };
            books.Add(book5);

            return books;
        }

        public static Book GetSinhalaBookByBookId(int bookId)
        {
            return GetAllSinhalaBooks().Where(c => c.Id == bookId).FirstOrDefault();
        }

        public static Book GetEnglishBookByBookId(int bookId)
        {
            return GetAllEnglishBooks().Where(c => c.Id == bookId).FirstOrDefault();
        }

        public static List<Chapter> GetChaptersBySinhalaBookId(int bookId)
        {
            var xmalFileName = "Books.xml";
            XDocument xdoc = GetXDocument(xmalFileName).Result;

            var book = xdoc.Descendants("book").ToList().Where(c => c.Attribute("name").Value == string.Format("book{0}", bookId)).FirstOrDefault();

            var chapters = new List<Chapter>();

            foreach (var selectedChapter in book.Descendants("Chapter"))
            {
                var chapter = new Chapter();
                chapter.BookId = bookId;
                chapter.Id = Convert.ToInt32(selectedChapter.Element("Id").Value);
                chapter.Name = selectedChapter.Element("Name").Value;
                chapter.Book = GetSinhalaBookByBookId(bookId);
                chapters.Add(chapter);
            }

            return chapters;
        }

        public static List<Chapter> GetChaptersByEnglishBookId(int bookId)
        {
            var xmalFileName = string.Format("content_of_english_book{0}.xml", bookId);
            XDocument xdoc = GetXDocument(xmalFileName).Result;

            var book = xdoc.Descendants("Content");

            var chapters = new List<Chapter>();

            int chapterId = 1;
            foreach (var selectedChapter in book)
            {
                var chapter = new Chapter();
                chapter.BookId = bookId;
                chapter.Id = chapterId;
                chapter.Name = selectedChapter.Element("Name").Value;
                chapter.Content = selectedChapter.Element("Description").Value;
                chapter.Book = GetEnglishBookByBookId(bookId);
                chapters.Add(chapter);
                chapterId++;
            }

            return chapters;
        }

        public static async  Task<List<PansilMaluwa>> GetVideoLinks()
        {
            var xmalFileName = "pansilmaluwa.xml";
            XDocument xdoc = await GetXDocument(xmalFileName);

            var contents = xdoc.Descendants("Content").ToList();
            var vls = new List<PansilMaluwa>();
            foreach (var item in contents)
            {
                var vl = new PansilMaluwa
                {
                    Name = item.Element("Name").Value,
                    Url = item.Element("Url").Value,
                };
                vls.Add(vl);
            }
            return vls;
        }

        public static async Task<List<Deshana>> GetDhammaDeshana()
        {
            var xmalFileName = "DhammaDeshana.xml";
            XDocument xdoc = await GetXDocument(xmalFileName);

            var contents = xdoc.Descendants("Content").ToList();
            var vls = new List<Deshana>();
            foreach (var item in contents)
            {
                var vl = new Deshana
                {
                    Time = item.Element("Time").Value,
                    Date = item.Element("Date").Value,
                    Location = item.Element("Location").Value,
                    Url = item.Element("Url").Value,
                };
                vls.Add(vl);
            }
            return vls;
        }

        private static async Task<XDocument> GetXDocument(string xmalFileName)
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(xmalFileName);
            return XDocument.Load(stream);
        }

        public static async Task<List<Deshana>> GetDhammaDeshana(int pageNumber, int pageSize)
        {
            // This is an example assuming you have a complete list of items.
            // If you are fetching from a database or API, you would modify your query
            // to only return the requested page (e.g., using LIMIT/OFFSET in SQL).
            List<Deshana> allItems = await GetDhammaDeshana(); 

            var pagedItems = allItems.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            // Simulate network delay
            await Task.Delay(500);

            return pagedItems;
        }
    }
}
