using System.Text.Json.Serialization;

namespace Blog.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        
        [JsonIgnore]
        public IList<Post> Posts { get; set; }
    }
}