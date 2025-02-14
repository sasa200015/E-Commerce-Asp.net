using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Models
{
    public class User: IdentityUser
    {
        public List<Cart>? cart { get; set; }
    }
}
