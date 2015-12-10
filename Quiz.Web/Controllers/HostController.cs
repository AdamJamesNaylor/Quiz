
namespace Quiz.Web.Controllers
{
    using System;
    using System.IO;
    using System.Web.Mvc;

    public class HostController : Controller
    {
        public HostController() {

        }
        // GET: Host
        public ActionResult Index() {
            var xml = new XmlHelper(Server);
            var state = xml.GetState();
            switch (state) {
                case "":
                    return View();
                case "start":
                    return View("Start");
                default: //question
                    return View("Question");

            }
            throw new Exception("unknown state " + state);
        }

        [HttpPost]
        public ActionResult Start() {
            var xml = new XmlHelper(Server);

            xml.ChangeState("start");
            DashController.refresh = true;

            return Redirect("/host");
        }

        [HttpPost]
        public ActionResult Next()
        {
            var xml = new XmlHelper(Server);

            var currentState = xml.GetState();
            var nextQuestion = xml.Next(currentState);
            xml.ChangeState(nextQuestion.ToString());
            DashController.refresh = true;

            return Redirect("/host");
        }

        public ActionResult Reset() {
            var xml = new XmlHelper(Server);

            RemoveAllTeams();

            xml.CreateNewGame();

            DashController.refresh = true;

            return Redirect("/host");
        }

        private void RemoveAllTeams() {
            var teamsFolder = Server.MapPath("~/teams/");
            string[] filePaths = Directory.GetFiles(teamsFolder);
            foreach (string filePath in filePaths)
                System.IO.File.Delete(filePath);
        }

    }
}