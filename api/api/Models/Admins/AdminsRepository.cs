using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Models.Admins
{
    public class AdminsRepository
    {
        // Database settings
        private static string url = ("mongodb+srv://thales:iambatman@teste-tngy3.mongodb.net/test?retryWrites=true&w=majority");
        private static MongoClient client = new MongoClient(url);
        private static IMongoDatabase database = client.GetDatabase("OtanerBank");
        private static IMongoCollection<Admin> AdminCollection = database.GetCollection<Admin>("Admins");
        private static IMongoCollection<Client> ClientCollection = database.GetCollection<Client>("Clients");


        // Admin stuffs

        public List<Admin> SearchAdmins()
        {
            try
            {
                return AdminCollection.Find(x => true).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Admin SearchAdmin(String CPF)
        {
            try
            {
                return SearchAdmins().Where(x => x.CPF == CPF).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string UpdateAdmin(String CPF, Admin admin)
        {
            try
            {
                if (SearchAdmin(CPF) == null)
                {
                    return "404";
                }
                else
                {
                    var update = Builders<Admin>.Update
                    .Set("NAME", admin.NAME).Set("EMAIL", admin.EMAIL).Set("PASSWORD", admin.PASSWORD);

                    AdminCollection.UpdateOne(x => x.CPF == CPF, update, null);
                    return "200";
                }
            }
            catch (MongoException e)
            {
                return e.GetBaseException().ToString();
            }
        }



        //Client stuffs
        public List<Client> Admin_SearchClients()
        {
            try
            {
                return ClientCollection.Find(x => true).ToList();
            }
            catch (MongoException)
            {
                return null;
            }
        }

        public Client Admin_SearchClient(String CPF)
        {
            try
            {
                return Admin_SearchClients().Where(x => x.CPF == CPF).FirstOrDefault();
            }
            catch (MongoException)
            {
                return null;
            }
        }

        public string Admin_RegisterClient(Client client)
        {
            try
            {
                if (Admin_SearchClient(client.CPF) == null)
                {
                    ClientCollection.InsertOne(client);
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

        public string Admin_UpdateClient(String CPF, Client client)
        {
            try
            {
                if (Admin_SearchClient(CPF) == null)
                {
                    return "404";
                }
                else if (Admin_SearchClient(CPF).STATUS == "0")
                {
                    return "403";
                }
                else
                {
                    var update = Builders<Client>.Update
                    .Set("ACCOUNT", client.ACCOUNT)
                        .Set("CPF", client.CPF).Set("NAME", client.NAME).Set("EMAIL", client.EMAIL)
                        .Set("LAST_ACCESS", client.LAST_ACCESS).Set("BALANCE_EARNED", client.BALANCE_EARNED)
                        .Set("PASSWORD", client.PASSWORD).Set("BALANCE", client.BALANCE).Set("MANAGER_NAME", client.MANAGER_NAME)
                        .Set("MANAGER_EMAIL", client.MANAGER_EMAIL).Set("STATUS", "1");

                    ClientCollection.UpdateOne(x => x.CPF == CPF, update, null);
                    return "200";
                }
            }
            catch (MongoException e)
            {
                return e.GetBaseException().ToString();
            }
        }

        // Active Clients stuffs
        public string Admin_CountInactiveClientsAccounts()
        {
            try
            {
                return Admin_SearchInactiveClients().Count().ToString();
            }
            catch (MongoException e)
            {
                return e.GetBaseException().ToString();
            }
        }

        public List<Client> Admin_SearchInactiveClients()
        {
            try
            {
                return ClientCollection.Find(x => x.STATUS == "0").ToList();
            }
            catch (MongoException)
            {
                return null;
            }
        }

        public string Admin_ActiveClient(String CPF)
        {
            try
            {
                if (Admin_SearchClient(CPF) == null)
                {
                    return "404";
                }
                else
                {
                    Client client = Admin_SearchClient(CPF);
                    var update = Builders<Client>.Update
                    .Set("ACCOUNT", client.ACCOUNT)
                        .Set("CPF", client.CPF).Set("NAME", client.NAME).Set("EMAIL", client.EMAIL)
                        .Set("LAST_ACCESS", client.LAST_ACCESS).Set("BALANCE_EARNED", client.BALANCE_EARNED)
                        .Set("PASSWORD", client.PASSWORD).Set("BALANCE", client.BALANCE).Set("MANAGER_NAME", client.MANAGER_NAME)
                        .Set("MANAGER_EMAIL", client.MANAGER_EMAIL).Set("STATUS", "1");

                    ClientCollection.UpdateOne(x => x.CPF == CPF, update, null);
                    return "200";
                }
            }
            catch (MongoException e)
            {
                return e.GetBaseException().ToString();
            }
        }

        // Inactive Clients stuffs
        public string Admin_CountActiveClientsAccounts()
        {
            try
            {
                return Admin_SearchActiveClients().Count().ToString();
            }
            catch (MongoException e)
            {
                return e.GetBaseException().ToString();
            }
        }

        public List<Client> Admin_SearchActiveClients()
        {
            try
            {
                return ClientCollection.Find(x => x.STATUS == "1").ToList();
            }
            catch (MongoException)
            {
                return null;
            }
        }

        public string Admin_InactiveClient(String CPF)
        {
            try
            {
                if (Admin_SearchClient(CPF) == null)
                {
                    return "404";
                }                
                else
                {
                    Client client = Admin_SearchClient(CPF);
                    var update = Builders<Client>.Update
                    .Set("ACCOUNT", client.ACCOUNT)
                        .Set("CPF", client.CPF).Set("NAME", client.NAME).Set("EMAIL", client.EMAIL)
                        .Set("LAST_ACCESS", client.LAST_ACCESS).Set("BALANCE_EARNED", client.BALANCE_EARNED)
                        .Set("PASSWORD", client.PASSWORD).Set("BALANCE", client.BALANCE).Set("MANAGER_NAME", client.MANAGER_NAME)
                        .Set("MANAGER_EMAIL", client.MANAGER_EMAIL).Set("STATUS", "0");

                    ClientCollection.UpdateOne(x => x.CPF == CPF, update, null);
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
