using CampusConnect.Data;
using CampusConnect.Models;
using CampusConnect.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

    

namespace CampusConnect.ViewModels
{
    public class DashboardViewModel
    {
        public List<ApplicationUser> SuggestedUsers { get; set; } = new();
        public List<Question> TrendingQuestions { get; set; } = new();
        public List<Announcement> Announcements { get; set; } = new();
        public ApplicationUser CurrentUser { get; set; }
    }
}
