using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PROG455_DB_Assignment2.Models;
using System;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace PROG455_DB_Assignment2.Controllers
{
    public class UserController : Controller
    {
        static string BaseUrl = "http://ec2-18-223-162-6.us-east-2.compute.amazonaws.com/handle.php?";
        static API? api = new(BaseUrl);
        

        T? GETResultDeserialization<T>()
        {
            if (api == null) throw new NullReferenceException($"{nameof(api)} : Null API");
            if (api.GETResult == null) throw new NullReferenceException($"{nameof(api.GETResult)} : API post res failed");

            var lis = api.NSJsonDeserialize<List<T>>(api.GETResult);

            if (lis == null) throw new NullReferenceException($"{nameof(lis)} : Deserialization Failed");

            return lis.FirstOrDefault();
        }

        public async Task<ActionResult> Index()
        {
            await api.AsyncGET(new KeyValuePair<string, string>(
                "get-query",
                new APIQuery
                {
                Table = "PROG455_DB",
                Query = $"SELECT * FROM Users"
                }.ToString()
            ));
            
            var lis = api.NSJsonDeserialize<List<User>>(api.GETResult);
            //var session_json = HttpContext.Session.GetString("Repo");
            return View(lis);
        }


        // GET: SignInController/Create
        public async Task<ActionResult> SignIn()
        {
            return View();
        }

        // POST: SignInController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignIn(IFormCollection collection)
        {
            try
            {
                var name  = (string)collection["Name"] 
                    ?? throw new InvalidCastException($"{nameof(collection)} : Name : string");

                var password = (string)collection["Password"]
                    ?? throw new InvalidCastException($"{nameof(collection)} : Password : string");

                await api.AsyncGET(new KeyValuePair<string, string>(
                    "get-query",
                    new APIQuery
                    {
                        Table = "PROG455_DB",
                        Query = $"SELECT * FROM Users WHERE Name = '{name}'"
                    }.ToString()
                    ));

                var res = api.GETResult;

                var lis = api.NSJsonDeserialize<List<User>>(res);

                if (lis == null) throw new NullReferenceException($"{nameof(lis)} : Deserialization Failed");
                User user = lis.FirstOrDefault();

                if (user.Password != password) throw new Exception("Incorret Password");


                HttpContext.Session.SetString("UserID", $"{user.ID}");

                return RedirectToAction(nameof(Account));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignUp(IFormCollection collection)
        {
            try
            {

                var name = (string)collection["Name"]
                    ?? throw new InvalidCastException($"{nameof(collection)} : Name : string");


                var password = (string)collection["Password"]
                    ?? throw new InvalidCastException($"{nameof(collection)} : Password : string");

                User user = new User(name, password);


                await api.AsyncPOST(new Dictionary<string, string>
                {
                    {"post-query", new APIQuery
                    {
                        Table = "PROG455_DB",
                        Query = "INSERT INTO Users (ID, Name, Password, Location) " +
                            $"VALUES ('{user.ID}', '{user.Name}', '{user.Password}', '{user.Location}')"
                    }.ToString()}
                });

                HttpContext.Session.SetString("UserID", $"{user.ID}");

                return RedirectToAction(nameof(Account));
            }
            catch 
            {
                return View();
            }
        }

        // GET: SignInController/Edit/5
        public async Task<ActionResult> Account()
        {
            var id = HttpContext.Session.GetString("UserID");

            await api.AsyncGET(new KeyValuePair<string, string>(
                    "get-query",
                    new APIQuery
                    {
                        Table = "PROG455_DB",
                        Query = $"SELECT * FROM Users WHERE ID = '{id}'"
                    }.ToString()
                    ));

            User user = GETResultDeserialization<User>()
                ?? throw new NullReferenceException($"{nameof(GETResultDeserialization)} : Deserialization Failure");

            await api.AsyncGET(new KeyValuePair<string, string>(
                    "get-query",
                    new APIQuery
                    {
                        Table = "PROG455_DB",
                        Query = $"SELECT FrJson FROM Friends WHERE UserID = '{id}'"
                    }.ToString()
                    ));

            var res2 = api.POSTResult;
            ViewBag["Main"] = true;
            return View();
        }

        public async Task<ActionResult> Details(int id)
        {
            await api.AsyncGET(new KeyValuePair<string, string>(
                    "get-query",
                    new APIQuery
                    {
                        Table = "PROG455_DB",
                        Query = $"SELECT * FROM Users WHERE ID = '{id}'"
                    }.ToString()
                    ));

            User user = GETResultDeserialization<User>()
                ?? throw new NullReferenceException($"{nameof(GETResultDeserialization)} : Deserialization Failure");

            await api.AsyncGET(new KeyValuePair<string, string>(
                    "get-query",
                    new APIQuery
                    {
                        Table = "PROG455_DB",
                        Query = $"SELECT FrJson FROM Friends WHERE UserID = '{id}'"
                    }.ToString()
                    ));

            var res2 = api.POSTResult;
            ViewBag["Main"] = false;
            return View();
        }

        // POST: SignInController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SignInController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SignInController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
