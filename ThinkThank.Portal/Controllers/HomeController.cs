using System.Diagnostics;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ThinkThank.Portal.Models;

namespace ThinkThank.Portal.Controllers;

public class HomeController : Controller
{
    private CommentContext _db; 
    private readonly ILogger<HomeController> _logger;
    private readonly IWebHostEnvironment _env;
    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env, CommentContext db)
    {
        _logger = logger;
        _env = env;
        _db = db;
    }

    //[IEFilter] пока не использую
    public IActionResult Index(string culture)
    {
        GetCulture(culture);
        List<Comment> comments = _db.Comments.ToList();
        return View();
    }

   public async Task<IActionResult> ApiJwt(int page)
    {
        List<Comment> listComments = new List<Comment>();
        //на стороне клиента генерим токен и передаем в заголовке в апи
        var jwt = GenerateJWT();
        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", jwt);

            using (var responce = await httpClient
                .GetAsync("http://localhost:1375/api/comment"))
            {
                var apiResponce = await responce
                    .Content.ReadAsStringAsync();

                listComments = JsonConvert
                    .DeserializeObject<List<Comment>>(apiResponce);
            }
        }

        return View(listComments);
    }
   

    public IActionResult About()
    {
        return View();
    }
    
    public IActionResult NotFound(int? statusCode)
    {
        return View(statusCode is 404 ? "_NotFound" : "_Error");
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile data, UploadFormRequest request)
    {
        var path = Path.Combine(_env.WebRootPath + "\\downloads", data.FileName);
        await using (var stream = new FileStream(path, FileMode.Create))
        {
             await data.CopyToAsync(stream);
        }

        return View("Index");
    }

    public IActionResult Contact()
    {
        return View();
    }
    public string GetCulture(string code = "")
    {
        if (string.IsNullOrWhiteSpace(code)) return "";
        CultureInfo.CurrentCulture = new CultureInfo(code);
        CultureInfo.CurrentUICulture = new CultureInfo(code);
        ViewBag.Culture =
            $"CurrentCulture: {CultureInfo.CurrentCulture}, CurrentUICulture: {CultureInfo.CurrentUICulture}";
        return "";
    }

    private string GenerateJWT()
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("4c53ce9de0ab7c9ce2f72f2b1447aa73"));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: "",
            audience: "",
            expires: DateTime.Now.AddDays(20),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
}