using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Models
{
    [BsonIgnoreExtraElements]
    public class AdminDatabase
    {

        [BsonId]
        public Object Id { get; set; }

        [BsonElement]
        public string BirdName { get; set; }

        [BsonElement]
        public string BirdOrder { get; set; }

        [BsonElement]
        public string BirdHabitat { get; set; }

        [BsonElement]
        public string BirdStatus { get; set; }
    }
}
