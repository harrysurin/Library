using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace LibraryRepository.Models;

public class BookPictures
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid BookId { get; set; }

    public string Path { get; set; }

    [NotMapped]
    public IFormFile Picture { get; set; }
}