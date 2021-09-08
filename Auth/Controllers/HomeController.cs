using Auth.Models;
using Auth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

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

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Glossary()
        {
            return View();
        }


        public IActionResult QuizStart()
        {
            return View();
        }

        public IActionResult QuizOptions()
        {
            return View();
        }

        //public IActionResult Admin()
        //{
        //    return View();
        //}

        public IActionResult Quiz()
        {
            return View();
        }



        #region AUTHENTICATION AND AUTHORIZATION

        [HttpGet("denied")]
        public IActionResult Denied()
        {
            return View();
        }

        [Authorize(/*Roles = "Admin"*/)]
        public async Task<IActionResult> Admin()
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

        #endregion AUTHENTICATION AND AUTHORIZATION

        //MARIYA'S CODE FOR ADMIN PAGE
        [HttpPost]
        public ActionResult Save(Bird bird)
        {
            var birdService = new BirdsService();
            //gets MongoCollection instance representing a collection on this database
            var collection = _database.GetCollection<BsonDocument>("nzbirdspecies");
            //add filter to check duplicate records on basis of bird name
            var filter = Builders<BsonDocument>.Filter.Eq("_id", bird._id);
            //will return count if same document exists else will return 0
            BsonDocument dbBird = collection.Find(filter).FirstOrDefault();
            //if it is 0, then we are going to insert document
            if (dbBird == null)
            {         
                // if null, then create a new bird item
                dbBird = birdService.MapBirdToDbBird(bird);
                //insert into dataabse
                collection.InsertOne(dbBird);
                bird._id = dbBird.GetValue("_id").AsString;
            }
            else
            {
                //update existing bird (document)
                var update = Builders<BsonDocument>.Update 
                    .Set("Name", bird.Name)
                    .Set("Order", bird.Order)
                    .Set("Status", bird.Status)
                    .Set("Habitat", bird.Habitat)
                    .Set("Number", bird.Number);
                collection.UpdateOne(filter, update);
            }
            return Ok(bird);
            //return View();
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
        [HttpGet("birdquiz")]
        public ActionResult GetData()
        {
            var birdsMongoCollection = _database.GetCollection<Bird>("nzbirdspecies");
            //convert to Linq Queryable
            var birdsQueryable = birdsMongoCollection.AsQueryable();

            //convert to IList
            var birdsList = birdsQueryable.ToList();

            //OK converts to JSON 
            return Ok(birdsList);
        }


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

            //Return audio file instead of newQuestion
            return Ok(newQuestion);
        }


        [HttpGet("getSound/{birdId}")]
        public ActionResult GetBirdSound(string birdId)
        {
            // TODO: find bird sound in DB using birdId
            // I.e.
            var collection = _database.GetCollection<Bird>("nzbirdspecies");
            ////add filter to check duplicate records on basis of bird name
            //var filter = Builders<BsonDocument>.Filter.Eq("_id", birdId);
            ////will return count if same document exists else will return 0
            //BsonDocument dbBird = collection.Find(filter).FirstOrDefault();

            var dbBird = collection
                .Find(Builders<Bird>.Filter.Eq("_id", birdId))
                .FirstOrDefault();

            // Get sound from file.
            // Eventually we will get the wav file from the database so this will be no longer needed
            var fileBytes = System.IO.File.ReadAllBytes("Controllers/birdcall.wav");

            // Eventually we should have the sound stored in the db
            //var fileBytes = dbBird.GetValue("sound").AsByteArray;

            return File(fileBytes, "text/plain", Path.GetFileName("birdsound"));
         }

        [HttpGet("getQuestion")]
        public ActionResult GetQuestion()
        {
            // TODO: Get random whole bird from database
            var collection = _database.GetCollection<Bird>("nzbirdspecies");

            //convert to Linq Queryable
            var birdsQueryable = collection.AsQueryable();
            var result = birdsQueryable.ToArray();

            // Get random bird using linq
            var rand = new Random();
            var randomBird = result.ElementAt(rand.Next(result.Count()));
            

            // Get sound from file.
            // Eventually we will get the wav file from the database so this will be no longer needed
            //var fileBytes = System.IO.File.ReadAllBytes("Controllers/birdcall.wav");
            //randomBird.Sound = fileBytes;

            return Ok(randomBird);
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
