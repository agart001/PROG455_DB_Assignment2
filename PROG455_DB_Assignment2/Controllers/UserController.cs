using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PROG455_DB_Assignment2.Models;

namespace PROG455_DB_Assignment2.Controllers
{
    public class UserController : Controller
    {
        static string BaseUrl = "http://ec2-3-138-190-113.us-east-2.compute.amazonaws.com/index.php?";
        static API? api = new(BaseUrl);

        // GET: SignInController
        /*
        public async Task<ActionResult> Index()
        {
            await api.AsyncGET("check-connection");
            var res = api.GETResult;


            return View();
        }
        */

        public ActionResult Index()
        {
            return View();
        }

        // GET: SignInController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SignInController/Create
        public ActionResult SignIn()
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
                User? user;
                var name  = (string)collection["Name"] 
                    ?? throw new InvalidCastException($"{nameof(collection)} : Name : string");

                var password = (string)collection["Password"]
                    ?? throw new InvalidCastException($"{nameof(collection)} : Password : string");

                await api.AsyncPOST(new Dictionary<string, string>
                    {
                        {"get-user",  name}
                    });

                if (api.POSTResult != null)
                {
                    user = api.NSJsonDeserialize<User>(api.POSTResult);
                }
                else { user = null; }

                if (user == null) throw new NullReferenceException($"{nameof(user)} : Login User : Deserialize Fail");



                return RedirectToAction(nameof(Index));
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

                var json = api.NSJsonSerialize(user);


                await api.AsyncPOST(new Dictionary<string, string>
                {
                    {"add-user",  json }
                });


                return RedirectToAction(nameof(Index));
            }
            catch 
            {
                return View();
            }
        }

        // GET: SignInController/Edit/5
        public async Task<ActionResult> Account(int id)
        {
            await api.AsyncPOST(new Dictionary<string, string>
            {
                {"add-user",  $"{id}" }
            });

            var res = api.POSTResult;
            User user = JsonConvert.DeserializeObject<User>(res) 
                ?? throw new NullReferenceException($"{nameof(user)} : Deserialization Failure");

            

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
