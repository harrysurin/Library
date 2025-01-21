using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LibraryRepository.Models;

public class BookPictures
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid BookId { get; set; }

    public required string Path { get; set; }

    [NotMapped]
    public byte[]? PictureBytes { get; set; }

    [NotMapped]
    public string? FileExtension { get; set; }
}