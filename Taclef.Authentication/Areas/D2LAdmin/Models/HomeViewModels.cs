using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;


namespace Taclef.Authentication.Areas.D2LAdmin.Models
{
	public class HomeIndexViewModel
	{
		public List<LoginProviderDetails> Providers { get; set; }
        [DisplayName("Instance")]
		public string Provider { get; set; }
        [DisplayName("Recherche")]
		public string Keywords { get; set; }
        [DisplayName("Résultats par page")]
		public int PageCount { get; set; }

		public int PageNumber { get; set; }

		public List<UserDetails> Users { get; set; }
	    public bool HasUsers { get; set; }
	}

    public class AddUsersViewModel
    {
        public List<LoginProviderDetails> Providers { get; set; }
        [DisplayName("Instance")]
		public string Provider { get; set; }
        public List<SelectListItem> SchoolBoards { get; set; }
        [DisplayName("Conseil scolaire")]
        public string SchoolBoardId { get; set; }
        public List<SelectListItem> Schools { get; set; }
        [DisplayName("Ecole")]
        public string SchoolId { get; set; }
        public List<SelectListItem> Roles { get; set; }
        [DisplayName("Rôle")]
        public string RoleName { get; set; }
       

    }
    

	[Flags]
	public enum UserSortOrders
	{
		LastName = 0x0001,
		FirstName = 0x0002,
		Account = 0x0004,
		Email = 0x0008,
		Number = 0x0010,
		Role = 0x0020,
		Descending = 0x0100
	}
}