using System.ComponentModel.DataAnnotations;

namespace CRM.Models
{
	public class ItemToCreateViewModel
	{
		[Required]
		[Range(1, 100)]
		[Display(Name = "Count: ")]
		public int Count { get; set; }
		[Required]
		[Display(Name = "Chose type of gerating data : ")]
		public ItemType ItemType { get; set; }
	}

	public enum ItemType
	{
		Leads,
		Customer,
		AdminTeamMember
	}
}