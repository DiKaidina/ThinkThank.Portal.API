using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using ThinkTank.Portal.WebApi.Data;
using ThinkTank.Portal.WebApi.Models;

namespace ThinkTank.Portal.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] // для доступа к контроллеру необходимо авторизироваться
    public class CommentController : ControllerBase
    {
        private CommentContext _db;

        public CommentController(CommentContext db)
        {
            _db = db;
        }

        //получить все сущности
        [HttpGet]
        public IEnumerable<Comment> Get()
        {
            var data = _db.Comments;

            return data;
        }

        [HttpGet("{Id}")]
        public Comment Get(int Id)
        {
            var data = _db.Comments.FirstOrDefault(f => f.Id == Id);

            return data;
        }

        [HttpPost]
        public Comment Post(Comment comment)
        {
            _db.Comments.Add(comment);
            _db.SaveChanges();
            return comment; 

        }

        [HttpPut]
        public StatusCodeResult Put([FromForm] Comment comment)
        {
            var data = _db.Comments.FirstOrDefault(f=>f.Id.Equals(comment.Id));
            if (data != null)
            {

                data.CommentText = comment.CommentText; 
                data.ReviewerName = comment.ReviewerName;
                data.ReviewerAvatar = comment.ReviewerAvatar; 

                _db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }

           
        }

        [HttpDelete]
        public void Delete(int id)
        {
            var data = _db.Comments.FirstOrDefault(f=>f.Id.Equals(id));
            _db.Remove(data);
            _db.SaveChanges();  
        }
    }
}
