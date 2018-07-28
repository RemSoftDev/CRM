using CRM.Attributes;

namespace CRM.Models
{
    public class NoteViewModel
    {
        public int Id { get; set; }

        [Grid(ShowOnGrid = true)]
        public string Text { get; set; }
    }
}