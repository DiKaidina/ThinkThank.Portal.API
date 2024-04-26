using System;
using System.Collections.Generic;

namespace ThinkTank.Portal.WebApi.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public string CommentText { get; set; } = null!;
        public string ReviewerName { get; set; } = null!;
        public string ReviewerAvatar { get; set; } = null!;
    }
}
