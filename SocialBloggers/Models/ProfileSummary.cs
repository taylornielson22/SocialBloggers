using System.Collections.Generic;

namespace SocialBloggers.Models
{
    public class ProfileSummary
    {
        public string Username { get; set; }

        public IEnumerable<Post> AllPosts { get; set; }

        public int Followers { get; set; }

        public int Following { get; set; }

        public string CurrentUser { get; set; }
    }
}