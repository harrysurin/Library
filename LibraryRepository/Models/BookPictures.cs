namespace LibraryRepository.Models;

public class BookPictures
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid BookId { get; set; }

    public string Path { get; set; }

    public byte[]? PictureBytes { get; set; }

    public string? FileExtension { get; set; }
}