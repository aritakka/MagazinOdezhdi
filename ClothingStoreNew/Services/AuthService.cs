using System;
using System.Linq;

namespace ClothingStoreNew.Services
{
    public class AuthService
    {
        public Users Login(string email, string password)
        {
            using (var db = new OnlineStoreDbEntities())
            {
                return db.Users.FirstOrDefault(u =>
                    u.Email == email &&
                    u.PasswordHash == password);
            }
        }

        public bool Register(string email, string password, string fullName)
        {
            using (var db = new OnlineStoreDbEntities())
            {
                var exists = db.Users.Any(u => u.Email == email);

                if (exists)
                    return false;

                var user = new Users
                {
                    Email = email,
                    PasswordHash = password,
                    FullName = fullName
                };

                db.Users.Add(user);
                db.SaveChanges();

                return true;
            }
        }
    }
}