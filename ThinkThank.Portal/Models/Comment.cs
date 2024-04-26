using System.ComponentModel.DataAnnotations;

namespace ThinkThank.Portal.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string CommentText { get; set; }

        public string ReviewerName { get; set; }

        public string ReviewerAvatar { get; set; }
    }
}
