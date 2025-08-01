namespace MRWMO.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public ICollection<Chapter> Chapters { get; set; }
        public int LanguageId { get; set; }
        public int Progress { get; set; }
    }
}
