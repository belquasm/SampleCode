using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Taclef.Authentication.Areas.D2LAdmin.Models;
using Taclef.Authentication.LoginProviders;
using Taclef.Authentication.Models;
using Taclef.Authentication.Models.D2LLTI;

namespace Taclef.Authentication.Areas.D2LAdmin.Controllers
{
    public class HomeController : ControllerBase
    {
        // GET: D2LAdmin/Home
        public ActionResult Index(string provider = null)
        {
            var model = new HomeIndexViewModel();

            model.Providers = GetProviders();

            var loginProvider =
                GetProvider(provider);

            if (loginProvider == null)
            {
                throw new ApplicationException("No login provider found");
            }


            model.Provider = loginProvider.Name;
                model.HasUsers =
                    loginProvider.Logins.Any(
                        login =>
                            !login.User.IsDeleted &&
                            (login.User.Roles == null || login.User.Roles.Count == 0 ||
                             login.User.Roles.Any(r => r.IsStandardRole)));

                //model.Users =
                //    loginProvider.Logins.Where(login => !login.User.IsDeleted).Select(login => new UserDetails
                //    {
                //        Id = login.User.Id,
                //        Email = login.User.EmailAddress,
                //        FirstName = login.User.FirstName,
                //        LastName = login.User.LastName,
                //        // note: .MEN may throw an exception, in which case get the .EmployeeNumber and pad with zeros in the .ForEach
                //        EmployeeNumber = login.User.MEN,
                //        RoleNames = login.User.Roles.Select(r => r.Name).ToList(),
                //        Login = login.Credentials
                //    }).ToList();

                //model.Users.ForEach(u =>
                //{
                //    // Extract the D2L user account name from the login credentials
                //    var creds = Desire2LearnLtiLoginProvider.ReadCredentials(u.Login);
                //    u.Login = creds == null ? null : creds.UserId;

                //    // Figure out best role to display
                //    var role = roles.FirstOrDefault(r => u.RoleNames.Contains(r.Name));
                //    if (role != null)
                //    {
                //        u.Role = role.DisplayName;
                //    }
                //});
            


            return View(model);
        }
        
        [HttpGet]
        public ActionResult AddUsers(string provider = null)
        {
            var model = GetAddUsersViewModel(provider);
            return View(model);
        }

        private AddUsersViewModel GetAddUsersViewModel(string provider,string boardId = null)
        {
            var model = new AddUsersViewModel();
            model.Providers = GetProviders();

            var loginProvider =
                GetProvider(provider);

            if (loginProvider == null)
            {
                throw new ApplicationException("No login provider found");
            }

            model.SchoolBoards = GetSchoolBoardList(boardId);
            model.Provider = loginProvider.Name;
            model.Schools = GetSchoolList(boardId);
            model.Roles = GetStandardRoleList();
            return model;
        }

        [HttpPost]
        public ActionResult AddUsers(FormCollection formCollection, string provider)
        {
      
            var logins = formCollection["Login"].Split(',');
            var employeeNumbers = formCollection["EmployeeNumber"].Split(',');
            var roles = formCollection["RoleName"].Split(',');
            var schoolIds = formCollection["SchoolId"].Split(',');
            var schoolBoardId = formCollection["SchoolBoardId"];
            var board = Db.Boards.Find(schoolBoardId);
            // Server validation
            if (string.IsNullOrWhiteSpace(schoolBoardId))
            {
                ModelState.AddModelError("", "Le conseil scolaire est requis");

            }
            if (logins.Any(string.IsNullOrWhiteSpace))
            {
                ModelState.AddModelError("","Le nom d'utilisateur est requis");
               
            }

            if (roles.Any(string.IsNullOrWhiteSpace))
            {
                ModelState.AddModelError("", "Le rôle est requis");

            }

            for (int i = 0; i < logins.Count(); i++)
            {
                if (roles[i] == "Teacher")
                {
                    if (string.IsNullOrEmpty(schoolIds[i]))
                    {
                        if (!ModelState.ContainsKey("TeacherSchool"))
                        ModelState.AddModelError("TeacherSchool", "L'école est requise pour les enseingants");
                    }

                    if (string.IsNullOrEmpty(employeeNumbers[i]))
                    {
                        if (!ModelState.ContainsKey("TeacherEmployeeNumber"))
                            ModelState.AddModelError("TeacherEmployeeNumber", "Le numérod'employé est requise pour les enseingants");
                    }
                }
                if (roles[i] == "SchoolAdmin")
                {
                    if (string.IsNullOrEmpty(schoolIds[i]))
                    {
                        if (!ModelState.ContainsKey("SchoolAdminSchool"))
                            ModelState.AddModelError("SchoolAdminSchool", "L'école est requise pour les directeurs d'écoles");
                    }
                }
            }

            var loginProvider = GetProvider(provider);

            if (loginProvider == null)
            {
                throw new ApplicationException("No login provider found");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.FormCollection = formCollection;
                ViewBag.SchoolBoardId = schoolBoardId;
                return View(GetAddUsersViewModel(provider, schoolBoardId));
            }

            for (int i = 0; i < logins.Count(); i++)
            {
                 var school = Db.Schools.Find(schoolIds[i]);
                var credentials = Desire2LearnLtiLoginProvider.CreateCredentials(logins[i]);
                var roleName = roles[i];
                var role = Db.ApplicationRoles.FirstOrDefault(r => r.Name == roleName);
                var emplyeeNumber = !string.IsNullOrWhiteSpace(employeeNumbers[i]) && employeeNumbers[i].IsAllNumeric()
                    ? int.Parse(employeeNumbers[i])
                    : (int?) null;
                Db.UserLogins.Add(new UserLogin
                {
                    Id = Guid.NewGuid().ToString(),
                    Credentials = credentials,
                    Provider = loginProvider,
                    User = new ApplicationUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        EmployeeNumber = emplyeeNumber,
                        School =  school,
                        Board = role != null && role.Name == "BoardAdmin" ? board: null,
                        Roles = role !=null? new List<ApplicationRole>
                        {
                            role
                        }:null

                    }
                });

                
            }
            Db.SaveChanges();
            return RedirectToAction("Index", new {provider});
        }

        [HttpGet]
        public ActionResult EditUser(string id)
        {
            var login = Db.UserLogins.FirstOrDefault(x => x.User.Id == id);
            if (login == null)
            {
                throw new ApplicationException(string.Format("Login with Id {0} cannot be found!", id));
            }
            ApplicationRole role = null;
            var firstOrDefault = login.User.Roles.FirstOrDefault();
            if (firstOrDefault != null)
            {
                role = firstOrDefault;
            }
            var schoolId = login.User.School == null ? "" : login.User.School.Id;
            var boardId = login.User.School != null
                ? login.User.School.Board.Id
                : login.User.Board != null ? login.User.Board.Id : "";

            var userDetail = new UserDetails
            {
                Id = login.User.Id,
                Email = login.User.EmailAddress,
                EmployeeNumber =
                    login.User.EmployeeNumber,
                FirstName = login.User.FirstName,
                LastName = login.User.LastName,
                Login = Desire2LearnLtiLoginProvider.ReadCredentials(login.Credentials).UserId,
                Role = role == null ? string.Empty : role.DisplayName,
                RoleName = role == null ? string.Empty : role.Name,
                RoleNames = login.User.Roles.Select(r => r.Name).ToList(),
                Roles = GetStandardRoleList(),
                SchoolBoards = GetSchoolBoardList(boardId),
                Schools = GetSchoolList(boardId, schoolId),
                SchoolBoardId =
                    login.User.School == null
                        ? (login.User.Board == null ? "" : login.User.Board.Id)
                        : login.User.School.Board.Id,
                SchoolId = login.User.School == null ? "" : login.User.School.Id
            };


            return View(userDetail);
        }

        [HttpPost]
        public ActionResult EditUser(UserDetails userDetails)
        {
            var loginToUpddate = Db.UserLogins.FirstOrDefault(x => x.User.Id == userDetails.Id);
            if (loginToUpddate == null)
            {
                throw new ApplicationException(string.Format("Login with Id {0} cannot be found!", userDetails.Id));
            }


            var schoolId = loginToUpddate.User.School == null ? "" : loginToUpddate.User.School.Id;
            var boardId = loginToUpddate.User.School != null
                ? loginToUpddate.User.School.Board.Id
                : loginToUpddate.User.Board != null ? loginToUpddate.User.Board.Id : "";
            userDetails.Roles = GetStandardRoleList();
            userDetails.SchoolBoards = GetSchoolBoardList(boardId);
            userDetails.Schools = GetSchoolList(boardId, schoolId);


            // To Do validation based on requirements when it's clarified
            // check if Role is Teacher, then School is mandatory
            var roleName = userDetails.RoleName;


            if (!string.IsNullOrWhiteSpace(userDetails.Login) && LoginTaken(userDetails.Login, userDetails.Id))
            {
                ModelState.AddModelError("Login", "Le compte est déja pris");
                
            }

            if (userDetails.EmployeeNumber.HasValue && !userDetails.EmployeeNumber.ToString().IsAllNumeric())
            {
                ModelState.AddModelError("EmployeeNumber", "Le numéro d'employée doit être un nombre");
               
            }

            if (roleName == "Teacher"&& !userDetails.EmployeeNumber.HasValue)
            {
                ModelState.AddModelError("EmployeeNumber", "Le numéro d'employée est requis pour les enseignats");
            }

            if ((roleName == "Teacher" || roleName == "SchoolAdmin") && string.IsNullOrWhiteSpace(schoolId))
            {
                ModelState.AddModelError("SchoolId", "L'école est requise pour les enseignats et directeurs d'écoles");
            }

            if (!ModelState.IsValid)
            {
                return View(userDetails);
            }
            // Save changes
            Db.Configuration.ValidateOnSaveEnabled = false;
            loginToUpddate.User.FirstName = userDetails.FirstName;
            loginToUpddate.User.LastName = userDetails.LastName;
            loginToUpddate.User.EmailAddress = userDetails.Email;
            loginToUpddate.User.EmployeeNumber = userDetails.EmployeeNumber;
            loginToUpddate.User.School = Db.Schools.Find(userDetails.SchoolId);
            loginToUpddate.User.Board = Db.Boards.Find(userDetails.SchoolBoardId);
            loginToUpddate.User.Roles.Clear();
            loginToUpddate.User.Roles.Add(Db.ApplicationRoles.FirstOrDefault(r => r.Name == userDetails.RoleName));
            loginToUpddate.Credentials = Desire2LearnLtiLoginProvider.CreateCredentials(userDetails.Login);


            Db.SaveChanges();
            Db.Configuration.ValidateOnSaveEnabled = true;

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteUser(string id)
        {
            var user = Db.ApplicationUsers.Find(id);
            user.IsDeleted = true;
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetSchoolsByBoardId(string boardId)
        {
            var isBoardNullOrWhiteSpace = string.IsNullOrWhiteSpace(boardId);
            var schools = Db.Schools.Where(s => s.Board.Id == boardId || isBoardNullOrWhiteSpace);
            var result = (from s in schools
                          select new
                          {
                              id = s.Id,
                              name = s.Name
                          }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllUsers(JQueryDataTableParamModel param, string provider)
        {
            var loginProvider = Db.GetD2LProviders().FirstOrDefault(p => string.Equals(p.Name, provider));

            if (loginProvider == null)
            {
                throw new ArgumentNullException("provider");

            }

            var allUsers =
                loginProvider.Logins.Where(
                    login =>
                        !login.User.IsDeleted &&
                        (login.User.Roles == null || login.User.Roles.Count == 0 ||
                         login.User.Roles.Any(r => r.IsStandardRole))).Select(login => new UserDetails
                         {
                             Id = login.User.Id,
                             Email = login.User.EmailAddress,
                             FirstName = login.User.FirstName,
                             LastName = login.User.LastName,
                             // note: .MEN may throw an exception, in which case get the .EmployeeNumber and pad with zeros in the .ForEach
                             EmployeeNumber = login.User.EmployeeNumber,
                             RoleNames = login.User.Roles.Select(r => r.Name).ToList(),
                             Login = login.Credentials,
                             Role = login.User.Roles == null || login.User.Roles.Count == 0 ? "Aucun rôle" : login.User.Roles.FirstOrDefault().DisplayName
                         }).ToList();

            allUsers.ForEach(u =>
            {
                // Extract the D2L user account name from the login credentials
                var creds = Desire2LearnLtiLoginProvider.ReadCredentials(u.Login);
                u.Login = creds == null ? null : creds.UserId;


            });

            IEnumerable<UserDetails> filteredUsers;

            // perform search
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Optionally check whether the columns are searchable at all 
                //
                var isLastNameSearchable = Convert.ToBoolean(Request["bSearchable_0"]);
                var isFirstNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isLoginSearchable = Convert.ToBoolean(Request["bSearchable_2"]);
                var isEmailSearchable = Convert.ToBoolean(Request["bSearchable_3"]);
                var isEmployeeNumberSearchable = Convert.ToBoolean(Request["bSearchable_4"]);
                var isRoleSearchable = Convert.ToBoolean(Request["bSearchable_5"]);

                filteredUsers = allUsers
                    .Where(
                        c =>
                            isLastNameSearchable && c.LastName != null &&
                            c.LastName.ToLower().Contains(param.sSearch.ToLower())
                            ||
                            isFirstNameSearchable && c.FirstName != null &&
                            c.FirstName.ToLower().Contains(param.sSearch.ToLower())
                            ||
                            isLoginSearchable && c.Login.ToLower().Contains(param.sSearch.ToLower())
                            ||
                            isEmailSearchable && c.Email != null && c.Email.ToLower().Contains(param.sSearch.ToLower())
                            ||
                            isEmployeeNumberSearchable && c.EmployeeNumber != null &&
                            c.EmployeeNumber.ToString().Contains(param.sSearch.ToLower())
                            ||
                            isRoleSearchable && c.Role != null && c.Role.Contains(param.sSearch.ToLower())).ToList();
            }
            else
            {
                filteredUsers = allUsers;
            }

            //perform sort
            var isLastNameSortable = Convert.ToBoolean(Request["bSortable_0"]);
            var isFirstNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isLoginSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isEmailSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var isEmployeeNumberSortable = Convert.ToBoolean(Request["bSortable_4"]);
            var isRoleSortable = Convert.ToBoolean(Request["bSortable_5"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<UserDetails, string> orderingFunction = (u => sortColumnIndex == 0 && isLastNameSortable ? u.LastName :
                                                               sortColumnIndex == 1 && isFirstNameSortable ? u.FirstName :
                                                               sortColumnIndex == 2 && isLoginSortable ? u.Login :
                                                               sortColumnIndex == 3 && isEmailSortable ? u.Email :
                                                               sortColumnIndex == 4 && isEmployeeNumberSortable && u.EmployeeNumber != null ? u.EmployeeNumber.ToString() :
                                                               sortColumnIndex == 5 && isRoleSortable ? u.Role :
                                                                "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredUsers = filteredUsers.OrderBy(orderingFunction);
            else
                filteredUsers = filteredUsers.OrderByDescending(orderingFunction);

            // perform paging
            var displayedUsers = filteredUsers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength).ToList();



            var result = displayedUsers.Select(u => new[]
            {
                u.LastName,
                u.FirstName,
                u.Login,
                u.Email,
                u.EmployeeNumber == null?"":u.EmployeeNumber.ToString(),
                u.Role,
                "<a title='Mettre à jour' href='" + Url.Action("EditUser", null, new {id = u.Id}) + "'>"
                + "<span class='glyphicon glyphicon-edit'></span></a>"
                + "<span>&nbsp;&nbsp;</span><a title='Supprimer' href='" +
                Url.Action("DeleteUser", null, new {id = u.Id}) +
                "'><span class='glyphicon glyphicon-trash'></span></a>"
            });
            return Json(new
            {
                param.sEcho,
                iTotalRecords = allUsers.Count(),
                iTotalDisplayRecords = filteredUsers.Count(),
                aaData = result
            },
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckLoginEmployeeNumber(int employeeNumber,string login, string provider)
        {
            var loginProvider =
              GetProvider(provider);

            if (loginProvider != null)
            {
                var userEmployeeNumber =
                    loginProvider.Logins.FirstOrDefault(
                        l => l.User.EmployeeNumber == employeeNumber);

                var credentials = Desire2LearnLtiLoginProvider.CreateCredentials(login);
                var userLogin = loginProvider.Logins.FirstOrDefault(
                        l => l.Credentials == credentials);
                if (userEmployeeNumber == null && userLogin == null)
                {

                    return Json(new
                    {
                        MessageEmployee = "",
                        MessageLogin = "",
                        Success = true
                    });
                }
                else
                {
                    
                    string loginName = string.Empty;

                    if (userEmployeeNumber != null)
                    {
                        loginName = Desire2LearnLtiLoginProvider.ReadCredentials(userEmployeeNumber.Credentials).UserId;
                    }
                    return Json(new
                        {
                            MessageEmployee = userEmployeeNumber == null?"": string.Format("Numéro {0} est déja associé avec {1}", employeeNumber, loginName),
                            MessageLogin = userLogin == null?"" : string.Format("Compte {0} est déja utilisé ",login),
                            Success = false
                        });
                    
                }
            }

            return Json(new
            {
                Message = "Une erreur s'est produite",
                Success = false
            });
        }

        #region Private methods

        private LoginProvider GetProvider(string provider)
        {
            return Db.GetD2LProviders().FirstOrDefault(p => string.Equals(p.Name, provider) || provider == null);
        }

        private List<LoginProviderDetails> GetProviders()
        {
            return Db.GetD2LProviders().OrderBy(p => p.DisplayName).Select(p => new LoginProviderDetails
            {
                Name = p.Name,
                DisplayName = p.DisplayName
            }).ToList();
        }

        private bool LoginTaken(string login, string userId)
        {
            var credentials = Desire2LearnLtiLoginProvider.CreateCredentials(login);
            return Db.UserLogins.Any(l => l.User.Id != userId && l.Credentials == credentials);
        }


        private int? ParseEmployeeNumber(string employeeNumber)
        {
            int i = 0;

            if (int.TryParse(employeeNumber, out i))
                return i;

            return null;
        }

       
        private List<SelectListItem> GetSchoolList(string boardId = "", string schoolId = "")
        {
            var list = Db.Schools.Where(s => boardId == null || boardId == ""|| s.Board.Id == boardId ).Select(s => new SelectListItem
            {
                Selected = s.Id == schoolId,
                Value = s.Id,
                Text = s.Name
            }).ToList();

            bool isNoneSelected = !list.Any(i => i.Selected);
            list.Insert(0, new SelectListItem
            {
                Selected = isNoneSelected,
                Text = "Selectionner une école",
                Value = ""
            });
            return list;
        }

        private List<SelectListItem> GetSchoolBoardList(string boardId = "")
        {
            var list = Db.Boards.Select(b => new SelectListItem
            {
                Selected = b.Id == boardId,
                Text = b.Name,
                Value = b.Id
            }).ToList();

            bool isNoneSelected = !list.Any(i => i.Selected);
            list.Insert(0, new SelectListItem
            {
                Selected = isNoneSelected,
                Text = "Selectionner un conseil",
                Value = ""
            });
            return list;
        }

        private List<SelectListItem> GetStandardRoleList(string roleId = "")
        {
            var list =
                Db.ApplicationRoles.Where(x => x.IsStandardRole)
                    .Select(x => new SelectListItem
                    {
                        Selected = x.Name == roleId,
                        Text = x.DisplayName,
                        Value = x.Name
                    }).ToList();

            bool isNoneSelected = !list.Any(i => i.Selected);
            list.Insert(0, new SelectListItem
            {
                Selected = isNoneSelected,
                Text = "Selectionner un rôle",
                Value = ""
            });
            return list;
        }

        #endregion
    }
}