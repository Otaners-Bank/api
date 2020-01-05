using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace api.Models
{
    public class ClientsRepository
    {
        // Database settings
        private static string url = ("mongodb+srv://thales:iambatman@teste-tngy3.mongodb.net/test?retryWrites=true&w=majority");
        private static MongoClient client = new MongoClient(url);
        private static IMongoDatabase database = client.GetDatabase("APS");
        private static IMongoCollection<Client> collection = database.GetCollection<Client>("Otaner Bank");



        public long countClients()
        {
            return (long)collection.CountDocuments(new BsonDocument());
        }


        public List<Client> listClients()
        {
            return collection.Find(_ => true).ToList();
        }
        public Client listClient(String CPF)
        {
            return listClients().Where(x => x.CPF == CPF).FirstOrDefault();
        }

        public Boolean insertClient(Client client)
        {
            return true;
        }

        public Boolean updateClient(String CPF, Client client)
        {
            return true;
        }

        public Boolean removeClient(String CPF)
        {
            return true;
        }
    }
}
