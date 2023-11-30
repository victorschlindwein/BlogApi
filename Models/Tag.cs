using System.Text.Json.Serialization;

namespace Blog.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        [JsonIgnore]
        public List<Post> Posts { get; set; }
    }
}