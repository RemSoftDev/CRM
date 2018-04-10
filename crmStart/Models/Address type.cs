using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crmStart.Models
{
    public class AddressType
    {
        public AddressType(AddressTypeEnum ate)
        {
            this.SetTypeAddress(ate); 
        }

        private AddressTypeEnum _actualType;

        public string SetTypeAddress(AddressTypeEnum ate)
        {
            this._actualType = ate;
            return _actualType.ToString();
        }

        public string GetTypeAddress()
        {
            return _actualType.ToString();
        }
    }

    public enum AddressTypeEnum
    {
        Billing = 0b01,
        Contact = 0b10,
        Emergency = 0b11,
    }
}