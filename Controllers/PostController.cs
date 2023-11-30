using Blog.Data;
using Blog.Data.Mappings;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    public class PostController : ControllerBase
    {
        [HttpGet("v1/posts")]
        public async Task<IActionResult> GetAsync(
            [FromServices] BlogDataContext context,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25)
        {
            try
            {
                var count = await context.Posts.AsNoTracking().CountAsync();
                var posts = await context
                    .Posts
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .Include(x => x.Author)
                    .Select(x => new ListPostsViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Slug = x.Slug,
                        LastUpdateDate = x.LastUpdateDate,
                        Category = x.Category.Name,
                        Author = $"{x.Author.Name} - ({x.Author.Email})"
                    })
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(x => x.LastUpdateDate)
                    .ToListAsync();

                return Ok(new ResultViewModel<dynamic>(new
                {
                    total = count,
                    page,
                    pageSize,
                    posts
                }));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Post>>("05X04 - Falha interna do servidor"));
            }
        }

        [HttpGet("v1/posts/{id:int}")]
        public async Task<IActionResult> DetailsAsync(
            [FromServices] BlogDataContext context,
            [FromRoute] int id)
        {
            try
            {
                var count = await context.Posts.AsNoTracking().CountAsync();
                var post = await context
                    .Posts
                    .AsNoTracking()
                    .Include(x => x.Author)
                    .ThenInclude(x => x.Roles)
                    .Include(x => x.Category)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (post == null)
                    return NotFound(new ResultViewModel<Post>("Conteúdo não encontrado"));

                return Ok(new ResultViewModel<Post>(post));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Post>>("05X04 - Falha interna do servidor"));
            }
        }

        [Authorize]
        [HttpPost("v1/posts/create")]
        public async Task<IActionResult> CreateNewPost(
        [FromBody] CreatePostViewModel model,
        [FromServices] BlogDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Post>(ModelState.GetErrors()));

            var email = User.Identity.Name;
            
            try
            {
                var authorInfo = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
                var categoryInfo = await context.Categories.AsNoTracking().Where(x => x.Name == model.CategoryName).FirstOrDefaultAsync();
                var tags = await GetOrCreateTagsAsync(context, model.Tags);

                var newPost = new Post
                {
                    Title = model.Title,
                    Summary = model.Summary,
                    Body = model.Body,
                    Slug = model.Title.Replace(" ", "-"),
                    CreateDate = DateTime.UtcNow.Date,
                    LastUpdateDate = DateTime.UtcNow.Date,
                    Author = authorInfo
                };

                newPost.Tags = tags;

                await context.Posts.AddAsync(newPost);
                await context.SaveChangesAsync();

                return Ok(newPost);
            }
            catch (Exception ex)
            { 
                return BadRequest(ex.Message);
            }
        }

        private async Task<List<Tag>> GetOrCreateTagsAsync(BlogDataContext context, List<string> tagNames)
        {
            var tags = new List<Tag>();
            foreach (var tagName in tagNames)
            {
                var tag = await context.Set<Tag>().FirstOrDefaultAsync(x => x.Name == tagName);
                if (tag == null)
                {
                    tag = new Tag
                    {
                        Name = tagName,
                        Slug = tagName.ToLower()
                    };

                    context.Set<Tag>().Add(tag);
                }
                tags.Add(tag);
            }
            return tags;
        }

        [HttpGet("v1/posts/category/{category}")]
        public async Task<IActionResult> GetByCategoryAsync(
            [FromServices] BlogDataContext context,
            [FromRoute] string category,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25)
        {
            try
            {
                var count = await context.Posts.AsNoTracking().CountAsync();
                var posts = await context
                    .Posts
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .Include(x => x.Author)
                    .Where(x => x.Category.Slug == category)
                    .Select(x => new ListPostsViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Slug = x.Slug,
                        LastUpdateDate = x.LastUpdateDate,
                        Category = x.Category.Name,
                        Author = $"{x.Author.Name} - ({x.Author.Email})"
                    })
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(x => x.LastUpdateDate)
                    .ToListAsync();

                return Ok(new ResultViewModel<dynamic>(new
                {
                    total = count,
                    page,
                    pageSize,
                    posts
                }));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Post>>("05X04 - Falha interna do servidor"));
            }
        }
    }
}
