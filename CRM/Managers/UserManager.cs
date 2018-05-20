using CRM.DAL.Entities;
using CRM.DAL.Repository;
using CRM.Services.Interfaces;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;

namespace CRM.Managers
{
	public class UserManager : IUserManager
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IEncryptionService _encryptionService;

		public UserManager(IUnitOfWork unitOfWork, IEncryptionService encryptionService)
		{
			_unitOfWork = unitOfWork;
			_encryptionService = encryptionService;
		}

		public IdentityResult Create(User user, string password)
		{
			var errors = ValidateAndGetError(user);


			// ReSharper disable once PossibleMultipleEnumeration
			if (!errors.Any())
			{
				user.Password = _encryptionService.Encrypt(user.Password);

				_unitOfWork.UsersRepository.Create(user);
				_unitOfWork.Save();

				return IdentityResult.Success;
			}

			// ReSharper disable once PossibleMultipleEnumeration
			return new IdentityResult(errors);
		}

		private IEnumerable<string> ValidateAndGetError(User user)
		{
			if (IsEmailExist(user.Email))
			{
				yield return $"Email {user.Email} alrady exist";
			}

			if (IsUserNameExist(user.FirstName, user.LastName))
			{
				yield return $"User with first name{user.FirstName} and lase name {user.LastName} alrady exist";
			}
		}

		public User GetUser(string email, string password)
		{
			string encryptedPassword = _encryptionService.Encrypt(password);

			return _unitOfWork.UsersRepository
				.Get(u => u.Email == email && u.Password == encryptedPassword)
				.FirstOrDefault();
		}

		private bool IsEmailExist(string email)
		{
			return _unitOfWork.UsersRepository.Any(u => u.Email == email);
		}
		private bool IsUserNameExist(string userFirstName, string userLastName)
		{
			return _unitOfWork.UsersRepository.Any(u => u.FirstName.Equals(userFirstName) && u.LastName.Equals(userLastName));
		}
	}
}