
namespace Quiz.Web.Controllers {

    using System.IO;
    using System.Xml.Linq;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class DashController
        : Controller {

        public static bool refresh = false;

        // GET: Dash
        public ActionResult Index()
        {
            var xml = new XmlHelper(Server);

            if (!xml.GameExists()) {
                xml.CreateNewGame();
            }
            //http://www.schillmania.com/projects/fireworks/
            var state = xml.GetState();
            return ProcessState(state);
        }

        private ActionResult ProcessState(string state) {
            switch (state) {
                case "":
                    var awaitingModel = new TeamList {
                        Teams = GetTeams()
                    };
                    return View("AwaitingView", awaitingModel);
                case "start":
                    var startModel = new TeamList {
                        Teams = GetTeams()
                    };
                    return View("StartView", startModel);

                    break;
            }
            return View();
        }

        private IEnumerable<Team> GetTeams() {
            var result = new List<Team>();

            var teamsFolder = Server.MapPath("~/Teams/");
            var dir = new DirectoryInfo(teamsFolder);
            foreach (var file in dir.GetFiles("*.xml").OrderBy(f => f.CreationTimeUtc)) {
                XElement xelement = XElement.Load(file.FullName);

                var name = xelement.Attribute("name");
                result.Add(new Team {
                    Name = name.Value
                });
            }

            return result;
        }

        public ActionResult Refresh() {
            if (!refresh)
                return Json(false);

            refresh = false;
            return Json(true);
        }
    }
}