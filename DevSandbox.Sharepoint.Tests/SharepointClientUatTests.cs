using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevSandbox.Sharepoint.Tests
{
    [TestClass]
    public class SharepointClientUatTests
    {
        string _baseUrl = @"http://intranetamericas.cshare.net/us/ccs/ProductionControl/Documents";
        string _folder = "USPS EXPRESS MAIL";

        [TestMethod]
        public void ExtractSearchFoldersFromSubPaths()
        {
            var subpath = "/Shared Documents/Reports TEST/";
            var searchFolder = subpath.Split('/')
                .Where(str => !string.IsNullOrWhiteSpace(str))
                .Last().Trim();

            Console.WriteLine($"subpath:{subpath}");
            Console.WriteLine($"searchFolder: {searchFolder}");
            Console.WriteLine();

            subpath = "/<FULLMONTH> <YEAR> Test/"
                          .Replace("<FULLMONTH>", DateTime.Today.ToString("MMMM"))
                          .Replace("<YEAR>", DateTime.Today.Year.ToString());

            searchFolder = subpath.Split('/')
                .Where(str => !string.IsNullOrWhiteSpace(str))
                .Last().Trim();

            Console.WriteLine($"subpath:{subpath}");
            Console.WriteLine($"searchFolder: {searchFolder}");
        }

        [TestMethod]
        public void RetrievePropertiesOfWebsite()
        {
            // Starting with ClientContext, the constructor requires a URL to the
            // server running SharePoint.
            var context = new ClientContext(_baseUrl);

            // The SharePoint web at the URL.
            var web = context.Web;

            // We want to retrieve the web's properties.
            context.Load(web);

            // Execute the query to the server.
            context.ExecuteQuery();

            // Now, the web's properties are available and we could display
            // web properties, such as title.
            Console.WriteLine(web.Title);
        }

        [TestMethod]
        public void RetrieveSelectedPropertiesOfWebsite()
        {
            var context = new ClientContext(_baseUrl);
            var web = context.Web;

            context.Load(web, w => w.Title, w => w.Description);

            context.ExecuteQuery();

            Console.WriteLine(web.Title);
            Console.WriteLine(web.Description);
        }

        [TestMethod]
        public void RetrieveAllListsOfWebsite()
        {
            var context = new ClientContext(_baseUrl);

            var web = context.Web;

            // Retrieve all lists from the server.
            // For each list, retrieve Title and Id.
            context.Load(web.Lists,
                         lists => lists.Include(list => list.Title,
                                                list => list.Id));

            // Execute query.
            context.ExecuteQuery();

            Console.WriteLine(_baseUrl);
            Console.WriteLine();

            // Enumerate the web.Lists.
            foreach (List list in web.Lists)
            {
                Console.WriteLine(list.Title);
            }
        }
        // Alternatively, you can use the LoadQuery method to store the return value in another collection, rather than use the web.Lists property.
        [TestMethod]
        public void RetrieveAllListsOfWebsiteUsingLoadQuery()
        {
            var context = new ClientContext(_baseUrl);
            var web = context.Web;

            // Retrieve all lists from the server, and put the return value in another
            // collection instead of the web.Lists.
            IEnumerable<List> result = context.LoadQuery(
              web.Lists.Include(
                  // For each list, retrieve Title and Id.
                  list => list.Title,
                  list => list.Id
              )
            );
            // Execute query.
            context.ExecuteQuery();

            // Enumerate the web.Lists.
            foreach (List list in result)
            {
                Console.WriteLine($"{list.Id} - {list.Title}");
            }
        }

        [TestMethod]
        public void RetrieveListsOfWebsite()
        {
            var context = new ClientContext(_baseUrl);

            // The SharePoint web at the URL.
            var web = context.Web;

            var list = web.Lists.GetByTitle(_folder);
            context.Load(list.RootFolder);
            context.ExecuteQuery();

            Console.WriteLine(list.RootFolder.ServerRelativeUrl);
        }

        [TestMethod]
        public void ListFoldersOfWebsite()
        {
            var context = new ClientContext(_baseUrl);
            var web = context.Web;
            var list = web.Lists.GetByTitle(_folder);

            context.Load(list.RootFolder.Folders);
            context.ExecuteQuery();

            foreach (var subfolder in list.RootFolder.Folders)
            {
                Console.WriteLine(subfolder.Name);
            }
        }

        [TestMethod]
        public void ListSubFoldersOfFolderForWebsite()
        {
            var folder = @"Shared Documents";

            var context = new ClientContext(_baseUrl);
            var web = context.Web;
            var list = web.Lists.GetByTitle(folder);

            context.Load(list.RootFolder.Folders);
            context.ExecuteQuery();

            foreach (var subfolder in list.RootFolder.Folders)
            {
                Console.WriteLine(subfolder.Name);
            }
        }

        [TestMethod]
        public void FindFolderOfWebsite()
        {
            var searchFolder = "JULY 2019";

            var context = new ClientContext(_baseUrl);
            var web = context.Web;
            var list = web.Lists.GetByTitle(_folder);

            var result = context.LoadQuery(list.RootFolder.Folders
                .Include(f => f.Name)
                .Where(f => f.Name == searchFolder));

            context.ExecuteQuery();

            Console.WriteLine($"Folder Exists: {result?.Any()}");
            Console.WriteLine(result.FirstOrDefault()?.Name);
        }

        [TestMethod]
        public void CreateFolderOfWebsite()
        {
            var searchFolder = "UAT";

            var context = new ClientContext(_baseUrl);
            context.Credentials = new NetworkCredential(AutoProcUser.Id, AutoProcUser.Pw);
            var web = context.Web;
            var list = web.Lists.GetByTitle(_folder);

            var result = context.LoadQuery(list.RootFolder.Folders
                .Include(f => f.Name)
                .Where(f => f.Name == searchFolder));

            context.ExecuteQuery();

            Console.WriteLine($"Folder: '{searchFolder}'.  Exists={result?.Any()}");
            if (result?.Any() != true)
            {
                var newFolder = list.RootFolder.Folders.Add(searchFolder);
                context.Load(newFolder);
                context.ExecuteQuery();
                Console.WriteLine($"Folder: '{searchFolder}' Added.");
            }
        }
        [TestMethod]
        public void CreateSubFolderOfWebsite()
        {
            var subPath = "/UAT <FULLMONTH> <YEAR> Test/"
                          .Replace("<FULLMONTH>", DateTime.Today.ToString("MMMM"))
                          .Replace("<YEAR>", DateTime.Today.Year.ToString());

            var searchFolder = subPath.Split('/').Where(str => !string.IsNullOrWhiteSpace(str)).Last().Trim();

            var context = new ClientContext(_baseUrl);
            context.Credentials = new NetworkCredential(AutoProcUser.Id, AutoProcUser.Pw);
            var web = context.Web;
            var list = web.Lists.GetByTitle(_folder);

            FolderExistOrCreate(context, list, subPath);

            //var result = context.LoadQuery(list.RootFolder.Folders
            //    .Include(f => f.Name)
            //    .Where(f => f.Name == searchFolder));

            //context.ExecuteQuery();

            //Console.WriteLine($"Folder: '{searchFolder}'.  Exists={result?.Any()}");
            //if (result?.Any() != true)
            //{
            //    var newFolder = list.RootFolder.Folders.Add(searchFolder);
            //    context.Load(newFolder);
            //    context.ExecuteQuery();
            //    Console.WriteLine($"Folder: '{searchFolder}' Added.");
            //}
        }

        [TestMethod]
        public void SubFolderOfWebsiteIsBlank()
        {
            var subPath = "";
            var searchFolder = subPath.ToSearchFolder();

            var context = new ClientContext(_baseUrl);
            context.Credentials = new NetworkCredential(AutoProcUser.Id, AutoProcUser.Pw);
            var web = context.Web;
            var list = web.Lists.GetByTitle(_folder);

            if (FolderExistOrCreate(context, list, subPath))
            {
                context.Load(list.RootFolder);
                context.ExecuteQuery();
                var fileURL = list.RootFolder.ServerRelativeUrl.ToFileUrl(searchFolder, "TestFileName.txt");
            };

            //var result = context.LoadQuery(list.RootFolder.Folders
            //    .Include(f => f.Name)
            //    .Where(f => f.Name == searchFolder));

            //context.ExecuteQuery();

            //Console.WriteLine($"Folder: '{searchFolder}'.  Exists={result?.Any()}");
            //if (result?.Any() != true)
            //{
            //    var newFolder = list.RootFolder.Folders.Add(searchFolder);
            //    context.Load(newFolder);
            //    context.ExecuteQuery();
            //    Console.WriteLine($"Folder: '{searchFolder}' Added.");
            //}
        }
        private bool FolderExistOrCreate(ClientContext context, List list, string searchFolder)
        {
            if (string.IsNullOrWhiteSpace(searchFolder)) return true;

            try
            {
                var result = context.LoadQuery(list.RootFolder.Folders
                    .Include(f => f.Name)
                    .Where(f => f.Name == searchFolder));

                context.ExecuteQuery();

                if (result?.Any() == true)
                {
                    Console.WriteLine($"            Found folder: {searchFolder}");
                    return true;
                }

                Console.WriteLine($"            Folder: {searchFolder} was not found, Trying to create instead.");

                var newFolder = list.RootFolder.Folders.Add(searchFolder);
                context.Load(newFolder);
                context.ExecuteQuery();

                Console.WriteLine($"          Folder: {searchFolder} Successfully created.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"          Error Trying to create Folder: {searchFolder}.  Error: {ex.Message}");
                return false;
            }
        }

        [TestMethod]
        [Ignore] // Don't have permissions to run
        public void RetrieveAllUsersAllGroupsSpecificProperties()
        {
            var context = new ClientContext(_baseUrl);
            context.Credentials = new NetworkCredential(AutoProcUser.Id, AutoProcUser.Pw);

            var collGroup = context.Web.SiteGroups;
            context.Load(collGroup,
                groups => groups.Include(
                    group => group.Title,
                    group => group.Id,
                    group => group.Users.Include(
                        user => user.Title,
                        user => user.LoginName)));

            context.ExecuteQuery();

            foreach (Group oGroup in collGroup)
            {
                UserCollection collUser = oGroup.Users;

                foreach (User oUser in oGroup.Users)
                {
                    Console.WriteLine("Group: {0} Group ID: {1} User: {2} Login Name: {3}",
                        oGroup.Title, oGroup.Id, oUser.Title, oUser.LoginName);
                }
            }
        }

    }
}

