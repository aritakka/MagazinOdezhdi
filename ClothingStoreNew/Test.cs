using System.Linq;

namespace ClothingStoreNew
{
    public class Test
    {
        public void Check()
        {
            using (var db = new OnlineStoreDbEntities())
            {
                var users = db.Users.ToList();
            }
        }
    }
}