namespace CRM.DAL.Entities
{
    public class Note
    {
        public int Id { get; set; }       
        public string Text { get; set; }
        public int? LeadId { get; set; }
        public Lead Lead { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }
    }
}