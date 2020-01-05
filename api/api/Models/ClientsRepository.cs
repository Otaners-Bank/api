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

        public string CountClients()
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

        public List<Client> ListClients()
        {
            try
            {
                List<Client> clients = collection.Find(x => true).ToList();
                foreach (Client client in clients)
                {
                    client.senha = null;
                    client.email = null;
                }
                return clients;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public Client ListClient(String CPF)
        {
            try
            {
                return ListClients().Where(x => x.CPF == CPF).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string InsertClient(Client client)
        {
            try
            {
                if (ListClient(client.CPF) == null)
                {
                    collection.InsertOne(client);
                    return "Client sucessfully inserted";
                }
                else
                {
                    return "Client already exists";
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
                if (ListClient(CPF) == null)
                {
                    return "Client doens't exists";
                }
                else
                {
                    var update = Builders<Client>.Update
                    .Set("conta", client.conta)
                        .Set("CPF", client.CPF).Set("nome", client.nome).Set("email", client.email)
                        .Set("ultimoAcesso", client.ultimoAcesso).Set("rendaGerada", client.rendaGerada)
                        .Set("senha", client.senha).Set("saldo", client.saldo).Set("nomeGerenteResponsavel", client.nomeGerenteResponsavel)
                        .Set("emailGerenteResponsavel", client.emailGerenteResponsavel);

                    collection.UpdateOne(x => x.CPF == CPF, update, null);
                    return "Client sucessfully updated";
                }
            }
            catch (MongoException e)
            {
                return e.GetBaseException().ToString();
            }
        }

        public string DeleteClient(String CPF)
        {
            try
            {
                if (ListClient(CPF) == null)
                {
                    return "Client doesn't exists";
                }
                else
                {
                    collection.DeleteOne(x => x.CPF == CPF, null);
                    return "Client sucessfully removed";
                }
            }
            catch (MongoException e)
            {
                return e.GetBaseException().ToString();
            }
        }
    }
}
