using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crmStart.Models
{
    public class TelephoneType
    {
        private TelephoneTypeEnum _telephoneType;

        public TelephoneType(TelephoneTypeEnum tte)
        {
            this._telephoneType = tte;
        }

        public string SetTelephoneType(TelephoneTypeEnum tte)
        {
            this._telephoneType = tte;
            return this._telephoneType.ToString();
        }

        public string GetTelephoneType()
        {
            return this._telephoneType.ToString();
        }
    }

    public enum TelephoneTypeEnum
    {
        HomePhone = 0b0001,
        WorkPhone = 0b0010,
        MobilePhone = 0b0011,
        EmergencyContactPhone = 0b0100,
        Fax = 0b0101
    }
}