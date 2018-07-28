using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services.Interfaces
{
    public interface IEncryptionService
    {
        string Encrypt(string inputText);

        string Decrypt(string encryptedText);
    }
}
