using System.Linq;

namespace ClothingStoreNew.Services
{
    public class AuthService
    {
        private readonly ClothingStoreEntities2 _db = new ClothingStoreEntities2();

        public Users Login(string email, string password)
        {
            return _db.Users.FirstOrDefault(u =>
                u.Email == email &&
                u.PasswordHash == password);
        }

        public bool Register(string email, string password, string fullName)
        {
            if (_db.Users.Any(u => u.Email == email))
                return false;

            _db.Users.Add(new Users
            {
                Email = email,
                PasswordHash = password,
                FullName = fullName
            });

            _db.SaveChanges();
            return true;
        }
    }
}