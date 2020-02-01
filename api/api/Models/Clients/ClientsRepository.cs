using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Models
{
    public class ClientsRepository
    {
        // Database settings
        private static string url = ("mongodb+srv://thales:iambatman@teste-tngy3.mongodb.net/test?retryWrites=true&w=majority");
        private static MongoClient client = new MongoClient(url);
        private static IMongoDatabase database = client.GetDatabase("OtanerBank");
        private static IMongoCollection<Client> collection = database.GetCollection<Client>("Clients");


        // Clients stuffs

        public string CountClientsAccounts()
        {
            try
            {
                return (collection.CountDocuments(new BsonDocument())).ToString();
            }
            catch (MongoException e)
            {
                return e.GetBaseException().ToString();
            }
        }

        private List<Client> SearchClients()
        {
            try
            {
                return collection.Find(x => true).ToList();
            }
            catch (MongoException)
            {
                return null;
            }
        }
        
        public Client SearchClient(String CPF)
        {
            try
            {
                return SearchClients().Where(x => x.CPF == CPF).FirstOrDefault();
            }
            catch (MongoException)
            {
                return null;
            }
        }
        
        public Client SearchAccount(String ACCOUNT)
        {
            try
            {
                return SearchClients().Where(x => x.ACCOUNT == ACCOUNT).FirstOrDefault();
            }
            catch (MongoException)
            {
                return null;
            }
        }

        public string RegisterClient(Client client)
        {
            try
            {
                if (SearchClient(client.CPF) == null)
                {
                    collection.InsertOne(client);
                    return "200";
                }
                else
                {
                    return "400";
                }
            }
            catch (MongoException e)
            {
                return e.GetBaseException().ToString();
            }
        }

        public string UpdateClient(String CPF, Client client)
        {
            try
            {
                if (SearchClient(CPF) == null)
                {
                    return "404";
                }
                else
                {
                    var update = Builders<Client>.Update
                    .Set("ACCOUNT", client.ACCOUNT)
                        .Set("CPF", client.CPF).Set("NAME", client.NAME).Set("EMAIL", client.EMAIL)
                        .Set("LAST_ACCESS", client.LAST_ACCESS).Set("BALANCE_EARNED", client.BALANCE_EARNED)
                        .Set("PASSWORD", client.PASSWORD).Set("BALANCE", client.BALANCE).Set("MANAGER_NAME", client.MANAGER_NAME)
                        .Set("MANAGER_EMAIL", client.MANAGER_EMAIL);

                    collection.UpdateOne(x => x.CPF == CPF, update, null);
                    return "200";
                }
            }
            catch (MongoException e)
            {
                return e.GetBaseException().ToString();
            }
        }
    }
}
