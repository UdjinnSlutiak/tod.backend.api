using System;
namespace Tod.Services.Abstractions
{
	public interface IPasswordHasher
	{
		public string GetHash(string password);
		public bool VerifyPassword(string password, string passwordHash);
	}
}

