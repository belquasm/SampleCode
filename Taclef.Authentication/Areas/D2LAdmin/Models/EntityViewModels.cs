using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Taclef.Authentication.Models;

namespace Taclef.Authentication.Areas.D2LAdmin.Models
{
	public class LoginProviderDetails
	{
		public string Name { get; set; }
		public string DisplayName { get; set; }
	}

	public class UserDetails
	{
		public string Id { get; set; }
        [DisplayName("Prénom")]
		public string FirstName { get; set; }
        [DisplayName("Nom")]
		public string LastName { get; set; }
        [DisplayName("Courriel")]
		public string Email { get; set; }
        [DisplayName("Numéro")]
        public int? EmployeeNumber { get; set; }
        [DisplayName("Rôle")]
		public string Role { get; set; }
        [Required(ErrorMessage = "Le rôle est requis")]
		public string RoleName { get; set; }
		public List<string> RoleNames { get; set; }
        [DisplayName("Compte")]
        [Required(ErrorMessage = "Le compte est requis")]
		public string Login { get; set; }
        [DisplayName("Selectionner un rôle")]
        public List<SelectListItem> Roles { get; set; }
        [DisplayName("Selectionner un conseil")]
        public List<SelectListItem> SchoolBoards { get; set; }
        [DisplayName("Selectionner une école")]
        public List<SelectListItem> Schools { get; set; }
        public string SchoolBoardId { get; set; }
        public string SchoolId { get; set; }
        
	}

	public class RoleDetails
	{
        public string Id { get; set; }
		public string Name { get; set; }
		public string DisplayName { get; set; }
	}

    public class SchoolDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public SchoolBoard SchoolBoard { get; set; }
    }

    public class SchoolBoardDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
       
    }


    
}