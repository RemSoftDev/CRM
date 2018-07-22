using CRM.DAL.Contexts;
using CRM.DAL.Entities;

namespace CRM.DAL.Repository
{
    public interface IUnitOfWork //: IDisposable
	{
		BaseContext Context { get; }
		IGenericRepository<Lead> LeadsRepository { get; }
		IGenericRepository<Note> NotesRepository { get; }
		IGenericRepository<Address> AddressRepository { get; }
		IGenericRepository<DAddressType> DAddressTypesRepository { get; }
		IGenericRepository<DPhoneType> DPhonesTypesRepository { get; }
		UserTypeRepository DUserTypesRepository { get; }
		IGenericRepository<Email> EmailsRepository { get; }
		IGenericRepository<LeadConvertedLog> LeadsConvertedLogsRepository { get; }
		IGenericRepository<Phone> PhonesRepository { get; }
		IGenericRepository<User> UsersRepository { get; }
		IGenericRepository<Call> CallsRepository { get; }
        IGenericRepository<Condition> ConditionsRepository { get; }
        void Save();
		//void Dispose(bool disposing);
	}
}