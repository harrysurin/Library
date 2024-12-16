using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LibraryRepository.Models;

public class User: IdentityUser<Guid>
{
    public RentHistory? History {get; set;}

}