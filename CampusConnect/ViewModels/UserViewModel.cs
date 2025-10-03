using CampusConnect.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CampusConnect.ViewModels
{
    public class UserViewModel
    {
        public ApplicationUser User { get; set; }
        public IList<string> Roles { get; set; }
    }
}
