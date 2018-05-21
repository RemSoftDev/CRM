using CRM.Attributes;
using CRM.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRM.Models
{
	public class CreateLeadViewModel : IUser
	{
		public int Id { get; set; }

		[Required]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		[Display(Name = "Last Name")]
		public string LastName { get; set; }

        [Grid(ShowOnGrid = true)]
        public string Name
		{
			get => $"{FirstName} {LastName}";
			set
			{
				if (value.Contains(" "))
				{
					var name = value.Split(' ');
					FirstName = name[0];
					LastName = name[1];

				}
				else
				{
					FirstName = value;
				}
			}
		}

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
        [Grid(ShowOnGrid = true)]
        public string Email { get; set; }

		[Display(Name = "Phone Number")]
		public string Phone { get; set; }

		[Display(Name = "Message")]
		public string Message { get; set; }

        [Grid(ShowOnGrid = true)]
        public List<PhoneViewModel> Phones { get; set; }

        //      public IEnumerable<PhoneViewModel> Phones
        //{
        //	get
        //	{
        //		yield return new PhoneViewModel { PhoneNumber = Phone };
        //	}
        //}
    }
}