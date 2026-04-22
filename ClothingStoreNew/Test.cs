using System.Linq;

namespace ClothingStoreNew
{
    public class Test
    {
        public void Check()
        {
            using (var db = new OnlineStoreDbEntities1())
            {
                var users = db.Users.ToList();
            }
        }
    }
}