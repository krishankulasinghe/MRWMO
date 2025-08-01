namespace MRWMO.Models
{
    public class Chapter
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }

    }
}
