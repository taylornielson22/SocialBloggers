using System.Collections.Generic;

namespace SocialBloggers.Models
{
    public class HomeSummary
    {
        public IEnumerable<Post> AllPosts { get; set; }

        public CurrentUser CurrentUser { get; set; }

        public CurrentUser CurrentProfile { get; set; }
    }
}