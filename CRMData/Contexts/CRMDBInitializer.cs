using CRMData.Entities;
using System.Data.Entity;

namespace CRMData.Contexts
{
	public class CRMDBInitializer : CreateDatabaseIfNotExists<BaseContext>
    {
        protected override void Seed(BaseContext context)
        {
            context.DPhoneTypes.Add(new DPhoneType() { TypeName = "HomePhone" });
            context.DPhoneTypes.Add(new DPhoneType() { TypeName = "WorkPhone" });
            context.DPhoneTypes.Add(new DPhoneType() { TypeName = "MobilePhone" });
            context.DPhoneTypes.Add(new DPhoneType() { TypeName = "EmergencyContactPhone" });
            context.DPhoneTypes.Add(new DPhoneType() { TypeName = "Fax" });

            context.DAddressTypes.Add(new DAddressType() { TypeName = "BillingAddress" });
            context.DAddressTypes.Add(new DAddressType() { TypeName = "ContactAddress" });
            context.DAddressTypes.Add(new DAddressType() { TypeName = "EmergencyContactAddress" });

            context.DUserTypes.Add(new DUserType() { TypeName = "AdminTeamMember" });
            context.DUserTypes.Add(new DUserType() { TypeName = "Customer" });

            base.Seed(context);
        }
    }
}