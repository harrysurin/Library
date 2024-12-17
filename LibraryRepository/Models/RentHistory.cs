using System.ComponentModel.DataAnnotations;

namespace LibraryRepository.Models;

public class RentHistory
{
    public Guid UserId {get; set;}
    public Guid BookId {get; set;}
    public DateTime DateOfRent {get; set;}
    public DateTime? DateOfReturn{get; set;}

}