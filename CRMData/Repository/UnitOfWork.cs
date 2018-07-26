using CRM.DAL.Contexts;
using CRM.DAL.Entities;
using CRM.Log;

namespace CRM.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BaseContext _dbContext;
        private bool _disposed;

        private IGenericRepository<Lead> _leadsRepository;
        private IGenericRepository<Note> _notesRepository;
        private IGenericRepository<Address> _addressRepository;
        private IGenericRepository<DAddressType> _dAddressTypesRepository;
        private IGenericRepository<DPhoneType> _dPhonesTypesRepository;
        private UserTypeRepository _dUserTypesRepository;
        private IGenericRepository<Email> _emailsRepository;
        private IGenericRepository<LeadConvertedLog> _leadsConvertedLogsRepositoryRepository;
        private IGenericRepository<Phone> _phonesRepository;
        private IGenericRepository<User> _usersRepository;
        private IGenericRepository<Call> _callsRepository;
        private IGenericRepository<Condition> _conditionRepository;
        private IGenericRepository<IgnoreNotifierConfig> _ignoreNotifierConfigRepository;
        private IGenericRepository<IgnoreNotifierWorkDayConfig> _ignoreNotifierWorkDayConfigRepository;

        public UnitOfWork()
        {
            _dbContext = new BaseContext();
            Logger.InfoLogContext.Info("UnitOfWork created");
        }

        public BaseContext Context => _dbContext;

        #region Repositories
        public IGenericRepository<Lead> LeadsRepository => _leadsRepository ?? (_leadsRepository = new EfGenericRepository<Lead>(_dbContext));

        public IGenericRepository<Note> NotesRepository =>
            _notesRepository ?? (_notesRepository = new EfGenericRepository<Note>(_dbContext));

        public IGenericRepository<Address> AddressRepository =>
            _addressRepository ?? (_addressRepository = new EfGenericRepository<Address>(_dbContext));

        public IGenericRepository<DAddressType> DAddressTypesRepository =>
            _dAddressTypesRepository ?? (_dAddressTypesRepository = new EfGenericRepository<DAddressType>(_dbContext));

        public IGenericRepository<DPhoneType> DPhonesTypesRepository =>
            _dPhonesTypesRepository ?? (_dPhonesTypesRepository = new EfGenericRepository<DPhoneType>(_dbContext));

        public UserTypeRepository DUserTypesRepository =>
            _dUserTypesRepository ?? (_dUserTypesRepository = new UserTypeRepository(_dbContext));

        public IGenericRepository<Email> EmailsRepository =>
            _emailsRepository ?? (_emailsRepository = new EfGenericRepository<Email>(_dbContext));


        public IGenericRepository<LeadConvertedLog> LeadsConvertedLogsRepository =>
            _leadsConvertedLogsRepositoryRepository ?? (_leadsConvertedLogsRepositoryRepository =
                new EfGenericRepository<LeadConvertedLog>(_dbContext));

        public IGenericRepository<Phone> PhonesRepository =>
            _phonesRepository ?? (_phonesRepository = new EfGenericRepository<Phone>(_dbContext));

        public IGenericRepository<User> UsersRepository =>
            _usersRepository ?? (_usersRepository = new EfGenericRepository<User>(_dbContext));

        public IGenericRepository<Call> CallsRepository =>
            _callsRepository ?? (_callsRepository = new EfGenericRepository<Call>(_dbContext));

		public IGenericRepository<IgnoreNotifierConfig> IgnoreNotifierConfigRepository =>
			_ignoreNotifierConfigRepository ?? (_ignoreNotifierConfigRepository = new EfGenericRepository<IgnoreNotifierConfig>(_dbContext));

		public IGenericRepository<IgnoreNotifierWorkDayConfig> IgnoreNotifierWorkDayConfigRepository =>
			_ignoreNotifierWorkDayConfigRepository ?? (_ignoreNotifierWorkDayConfigRepository = new EfGenericRepository<IgnoreNotifierWorkDayConfig>(_dbContext));

        public IGenericRepository<Condition> ConditionsRepository =>
            _conditionRepository ?? (_conditionRepository = new EfGenericRepository<Condition>(_dbContext));
        #endregion

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        //public virtual void Dispose(bool disposing)
        //{
        //	if (!_disposed)
        //	{
        //		if (disposing)
        //		{
        //			_dbContext.Dispose();
        //		}

        //		_disposed = true;
        //	}
        //}

        //public void Dispose()
        //{
        //	Dispose(true);
        //	GC.SuppressFinalize(this);
        //}
    }

}