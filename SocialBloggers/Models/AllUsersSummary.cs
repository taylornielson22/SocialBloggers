using System.Collections.Generic;

namespace SocialBloggers.Models
{
    public class AllUserSummary
    {
        public IEnumerable<User> Users { get; set; }

        public CurrentUser CurrentUser { get; set; }
    }
}