using Blog.Models;
using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Posts
{
    public class CreatePostViewModel
    {
        
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public string CategoryName { get; set; }

        public List<string> Tags { get; set; }
    }
}
