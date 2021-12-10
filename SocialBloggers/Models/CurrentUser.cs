using System.Collections.Generic;

namespace SocialBloggers.Models
{
    public class CurrentUser
    {
        public string Username { get; set; }

        public List<Following> Followers { get; set; }

        public List<Following> Following { get; set; }
    }
}