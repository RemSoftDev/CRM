
using CRM.Models;
using System.Collections.Generic;

namespace CRM.Interfaces
{
    public interface IUser
    {
        int Id { get; set; }
        string Email { get; set; }
    }
}
