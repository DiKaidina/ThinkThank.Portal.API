using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ThinkThank.Portal.Models;

namespace ThinkThank.Portal
{
    public class CommentContext :DbContext
    {
       public CommentContext(DbContextOptions<CommentContext> options) : base(options)
        {
        
        
        }

        public DbSet<Comment>Comments { get; set; }
    }
}
