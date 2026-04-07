using System.Linq;

namespace ClothingStoreNew.Services
{
    public class AuthService
    {
        private readonly ClothingStoreEntities2 _db = new ClothingStoreEntities2();

        public Users Login(string email, string password)
        {
            return _db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public bool Register(string email, string password, string fullName)
        {
            if (_db.Users.Any(u => u.Email == email))
                return false;

            var user = new Users
            {
                Email = email,
                Password = password,
                FullName = fullName
            };

            _db.Users.Add(user);
            _db.SaveChanges();
            return true;
        }
    }
}