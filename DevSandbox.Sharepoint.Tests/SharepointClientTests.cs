using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace DevSandbox.Sharepoint.Tests
{
    [TestClass]
    public class SharepointClientTests
    {
        string _baseUrl = "";
        string _folderRelativeUrl = "";

        [TestMethod]
        [Ignore]
        public void UpdatePropertiesOfWebsite()
        {
            var context = new ClientContext(_baseUrl);

            var web = context.Web;

            web.Title = "New Title";
            web.Description = "New Description";

            // Note that the web.Update() doesn't trigger a request to the server.
            // Requests are only sent to the server from the client library when
            // the ExecuteQuery() method is called.
            //web.Update();

            // Execute the query to server.
            //context.ExecuteQuery();
        }

        [TestMethod]
        [Ignore]
        public void CreateNewWebsite()
        {
            var context = new ClientContext(_baseUrl);

            WebCreationInformation creation = new WebCreationInformation();
            creation.Url = "web1";
            creation.Title = "Hello web1";
            Web newWeb = context.Web.Webs.Add(creation);

            // Retrieve the new web information.
            context.Load(newWeb, w => w.Title);
            //            context.ExecuteQuery();

            Console.WriteLine(newWeb.Title);
        }

        [TestMethod]
        public void ListFoldersByRelativeUrlOfWebsite()
        {
            var context = new ClientContext(_baseUrl);
            var web = context.Web;
            var folder = web.GetFolderByServerRelativeUrl(_folderRelativeUrl);

            //context.Load(list.RootFolder.Folders);
            context.Load(folder.Folders);
            context.ExecuteQuery();

            //Console.WriteLine($"Root Folders");
            //foreach (var subfolder in list.RootFolder.Folders)
            //{
            //    Console.WriteLine($"{subfolder.Name}: {subfolder.ServerRelativeUrl}");

            //}

            Console.WriteLine("");
            Console.WriteLine($"Sub Folders");
            foreach (var subfolder in folder.Folders)
            {
                Console.WriteLine($"{subfolder.Name}: {subfolder.ServerRelativeUrl}");
            }

        }

        [TestMethod]
        public void CreateFolderByRelativeUrlOfWebsite()
        {
            var subFolder = "/Test <FULLMONTH> <YEAR> Test/"
                          .Replace("<FULLMONTH>", DateTime.Today.ToString("MMMM"))
                          .Replace("<YEAR>", DateTime.Today.Year.ToString());

            var searchFolder = subFolder.Split('/')
                .Where(str => !string.IsNullOrWhiteSpace(str))
                .Last().Trim();

            var context = new ClientContext(_baseUrl);
            var web = context.Web;
            var folder = web.GetFolderByServerRelativeUrl(_folderRelativeUrl);

            var result = context.LoadQuery(folder.Folders
                .Include(f => f.Name)
                .Where(f => f.Name == searchFolder));

            context.ExecuteQuery();

            Console.WriteLine($"Folder: '{searchFolder}'.  Exists={result?.Any()}");
            if (result?.Any() != true)
            {
                var newFolder = folder.Folders.Add(searchFolder);
                context.Load(newFolder);
                context.ExecuteQuery();
                Console.WriteLine($"Folder: '{searchFolder}' Added.");
            }
        }

    }
}
