using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace LibraryRepository.Models;

public class RentHistory
{
    [Key]
    public Guid Id {get; set;} = Guid.NewGuid();
    public Guid UserId {get; set;}
    public Guid BookId {get; set;}
    public DateTime DateOfRent {get; set;}
    public DateTime? DateOfReturn{get; set;}

}