using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Old
{
    public class Season
    {
        [BsonId]
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
    }
}
