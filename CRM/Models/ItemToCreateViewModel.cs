namespace CRM.Models
{
	public class ItemToCreateViewModel
	{
		public int Count { get; set; }
		public ItemType ItemType { get; set; }
	}

	public enum ItemType
	{
		Leads,
		Customer,
		AdminTeamMember
	}
}