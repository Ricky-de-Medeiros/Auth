using Auth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Auth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;

        //}

        private IMongoDatabase _database;
        //Constructor
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            //Connect to MongoDB Database
            var settings = MongoClientSettings.FromConnectionString("mongodb://nzbirds:nzbirds@cluster0-shard-00-00.amr9m.mongodb.net:27017,cluster0-shard-00-01.amr9m.mongodb.net:27017,cluster0-shard-00-02.amr9m.mongodb.net:27017/nzbirds1?ssl=true&replicaSet=atlas-10rqz7-shard-0&authSource=admin&retryWrites=true&w=majority");
            var client = new MongoClient(settings);
            _database = client.GetDatabase("nzbirds1");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult QuizStart()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("denied")]
        public IActionResult Denied()
        {
            return View();
        }

        [Authorize(/*Roles = "Admin"*/)]
        public async Task<IActionResult> Secured()
        {
            var idToken = await HttpContext.GetTokenAsync("id_token");
            return View();
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Validate(string username, string password, string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (username == "phil" && password == "bird")
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("username", username));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, username));
                claims.Add(new Claim(ClaimTypes.Name, "Dr. Phil Battley"));

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                return Redirect(returnUrl);
            }
            TempData["Error"]="Error. Username or password is Invalid";
            return View("login");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect(@"https://www.google.com/accounts/Logout?continue=https://appengine.google.com/_ah/logout?continue=https://localhost:44358/");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        //MARIYA'S CODE BELOW:

        //private IMongoDatabase _database;
        ////Constructor
        //public HomeController()
        //{
        //    //Connect to MongoDB Database
        //    var settings = MongoClientSettings.FromConnectionString("mongodb://nzbirds:nzbirds@cluster0-shard-00-00.amr9m.mongodb.net:27017,cluster0-shard-00-01.amr9m.mongodb.net:27017,cluster0-shard-00-02.amr9m.mongodb.net:27017/nzbirds1?ssl=true&replicaSet=atlas-10rqz7-shard-0&authSource=admin&retryWrites=true&w=majority");
        //    var client = new MongoClient(settings);
        //    _database = client.GetDatabase("nzbirds1");
        //}


        // GET: HomeController
        //[HttpGet("birdquiz")]
        ////public ActionResult Index()
        //{
        //    var birdsMongoCollection = _database.GetCollection<Bird>("nzbirdspecies");
        //    //convert to Linq Queryable
        //    var birdsQueryable = birdsMongoCollection.AsQueryable();

        //    //convert to IList
        //    var birdsList = birdsQueryable.ToList();

        //    //OK converts to JSON 
        //    return Ok(birdsList);
        //}


        [HttpGet("habitats")]
        public ActionResult GetHabitats()
        {
            var birdsMongoCollection = _database.GetCollection<Bird>("nzbirdspecies");
            //convert to Linq Queryable
            var birdsQueryable = birdsMongoCollection.AsQueryable();

            //convert to IList
            var birdsList = birdsQueryable.ToList();

            // get unique habitats and split by commas
            var habitatsList = birdsList.SelectMany(bird => bird.Habitat.Split(',').ToList().Select(habitat => habitat.Trim())).Distinct();
            //var habitatsObjects = habitatsList.Select((habitat,i) => new { key = i + 1, value = habitat });

            //OK converts to JSON 
            return Ok(habitatsList);
        }

        [HttpGet("order")]
        public ActionResult GetOrder()
        {
            var birdsMongoCollection = _database.GetCollection<Bird>("nzbirdspecies");
            //convert to Linq Queryable
            var birdsQueryable = birdsMongoCollection.AsQueryable();

            //convert to IList
            var birdsList = birdsQueryable.ToList();

            // get unique habitats and split by commas
            var orderList = birdsList.Select(bird => bird.Order).Distinct();

            //OK converts to JSON 
            return Ok(orderList);
        }

        [HttpGet("status")]
        public ActionResult GetStatus()
        {
            //OK converts to JSON 
            return Ok(getStatusesList());
        }


        [HttpPost("statusQuestion")]
        public ActionResult GetStatusQuestion([FromBody] Bird bird)
        {
            // Get correct answer
            var correctStatus = bird.Status;

            // get two incorrect answers for multiple choice
            //var incorrectStatus = getStatusesList().Select(status => status != correctStatus);

            // TODO need to find code to mix up the position of the answer

            var newQuestion = new Question();

            newQuestion.QuestionText = "What status is " + bird.Name + "?";
            newQuestion.Answers = getStatusesList().Select(status => new Answer
            {
                AnswerText = status,
                IsCorrect = correctStatus == status
            }).ToList();

            return Ok(newQuestion);
        }

        private IEnumerable<string> getStatusesList()
        {
            var birdsMongoCollection = _database.GetCollection<Bird>("nzbirdspecies");

            //convert to Linq Queryable
            var birdsQueryable = birdsMongoCollection.AsQueryable();

            //convert to IList
            var birdsList = birdsQueryable.ToList();

            // get unique habitats and split by commas
            var statusList = birdsList.Select(bird => bird.Status).Distinct();

            return statusList;
        }
    }
}
