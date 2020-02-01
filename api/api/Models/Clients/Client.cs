using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Client
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string ACCOUNT { get; set; }
        public string CPF { get; set; }
        public string NAME { get; set; }
        public string EMAIL { get; set; }
        public string PASSWORD { get; set; }
        public string BALANCE { get; set; }

        public string LAST_ACCESS { get; set; }
        public string BALANCE_EARNED { get; set; }

        public string MANAGER_NAME { get; set; }
        public string MANAGER_EMAIL { get; set; }
    }
}
