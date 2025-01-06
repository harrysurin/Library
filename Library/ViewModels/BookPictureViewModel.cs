namespace Library.ViewModels
{
    public class BookPictureViewModel
    {
        public Guid Id { get; set; }

        public Guid BookId { get; set; }

        public IFormFile Picture { get; set; }
    }
}