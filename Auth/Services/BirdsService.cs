using Auth.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Services
{
    public class BirdsService
    {
        public BsonDocument MapBirdToDbBird(Bird bird)
        {            
            var document = new BsonDocument
                {
                    //{ "_id", bird._id },
                    { "Name", bird.Name },
                    { "Order", bird.Order },
                    { "Status", bird.Status },
                    { "Habitat", bird.Habitat },
                    //{ "Number", bird.Number },
                };
            return document;
        }
    }
}
