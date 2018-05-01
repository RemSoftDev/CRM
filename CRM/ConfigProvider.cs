using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CRM
{
    public static class ConfigProvider
    {
        public static string EncryptionKey => ConfigurationManager.AppSettings["encKey"];

        public static string UseMoq => ConfigurationManager.AppSettings["useMoqDB"];
    }
}