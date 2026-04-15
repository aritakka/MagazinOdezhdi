using ClothingStoreNew;
using System.Linq;
public class Test
{
    public void Check()
    {
        var db = new ClothingStoreEntities2();

        var users = db.Users.ToList();
    }
}