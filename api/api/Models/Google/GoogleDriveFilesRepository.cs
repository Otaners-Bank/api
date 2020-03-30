using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using File = Google.Apis.Drive.v3.Data.File;
using System.Linq;
using System.Collections.Generic;
using Google.Apis.Drive.v2.Data;

namespace api.Models.Google
{
    public class GoogleDriveFilesRepository
    {
        //defined scope.
        public static string[] Scopes = { DriveService.Scope.Drive };

        //create Drive API service.
        public static DriveService GetService()
        {
            //get Credentials from client_secret.json file 
            UserCredential credential;
            using (var stream = new FileStream(Directory.GetCurrentDirectory() + @"\Models\Google\client_secret.json", FileMode.Open, FileAccess.Read))
            {
                String FolderPath = @"" + Directory.GetCurrentDirectory();
                String FilePath = Path.Combine(FolderPath, "DriveServiceCredentials.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(FilePath, true)).Result;
            }

            //create Drive API service.
            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Otaner-Bank",
            });
            return service;
        }

        // file save to server path
        private static void SaveStream(MemoryStream stream, string FilePath)
        {
            using (System.IO.FileStream file = new FileStream(FilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                stream.WriteTo(file);
            }
        }

        //Delete file from the Google drive
        public static void DeleteFile(GoogleDriveFiles files)
        {
            DriveService service = GetService();
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");

                if (files == null)
                    throw new ArgumentNullException(files.Id);

                // Make the request.
                service.Files.Delete(files.Id).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Files.Delete failed.", ex);
            }
        }

        // Upload file to the Google drive
        public static string UploadImage(string fileName, String imageLocation)
        {
            DriveService service = GetService();
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");

                string folder = "1WGiO9s1F6gtjbuGRsRf7htkoV9PsN0x0";

                var request = service.Files.List();
                request.Fields = "files(id, name)";
                var resultado = request.Execute();
                var arquivos = resultado.Files;

                if (arquivos != null && arquivos.Any())
                {

                    foreach (var arquivo in arquivos)
                    {
                        if (service.Files.Get(arquivo.Id).Execute().Name == fileName)
                        {
                            GoogleDriveFiles file = new GoogleDriveFiles();
                            file.Id = arquivo.Id;
                            DeleteFile(file);
                        }
                    }

                }

                Stream image = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);


                File fileMetadata = new File();
                fileMetadata.Name = fileName;
                fileMetadata.Parents = new List<string>(){ folder };

                service.Files.Create(fileMetadata, image, "").Upload();
                return "200";

            }
            catch (Exception)
            {
                return "500";
            }
        }

        // Upload file to the Google drive
        public static string VerifyImage(string CPF)
        {
            DriveService service = GetService();
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");

                var request = service.Files.List();
                request.Fields = "files(id, name)";
                var resultado = request.Execute();
                var arquivos = resultado.Files;

                if (arquivos != null && arquivos.Any())
                {

                    foreach (var arquivo in arquivos)
                    {
                        if (service.Files.Get(arquivo.Id).Execute().Name == CPF)
                        {
                            return "200";
                        }
                    }

                }

                return "404";

            }
            catch (Exception)
            {
                return "500";
            }
        }

        // Load file from the Google drive
        public static string LoadImage(string CPF)
        {
            DriveService service = GetService();
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");

                string retorno = "";

                var request = service.Files.List();
                request.Fields = "files(id , name)";
                var resultado = request.Execute();
                var arquivos = resultado.Files;

                if (arquivos != null && arquivos.Any())
                {
                    foreach (var arquivo in arquivos)
                    {
                        if (arquivo.Name.Contains(CPF))
                        {
                            retorno = service.Files.Get(arquivo.Id).Execute().Id;
                        }
                    }
                }

                return retorno;

            }
            catch (Exception ex)
            {
                throw new Exception("Request Files.Delete failed.", ex);
            }
        }

    }
}