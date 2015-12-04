
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

        public DashController() {
            _xml = new XmlHelper(Server);
        }

        // GET: Dash
        public ActionResult Index()
        {
            //http://www.schillmania.com/projects/fireworks/
            var state = _xml.GetState();
            return ProcessState(state);
        }

        private ActionResult ProcessState(string state) {
            switch (state) {
                case "":
                    var model = new TeamList {
                        Teams = GetTeams()
                    };
                    return View("AwaitingView", model);
                case "start":

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

        private XmlHelper _xml;
    }
}