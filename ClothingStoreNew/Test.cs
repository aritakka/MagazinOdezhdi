public class Test
{
    public void Check()
    {
        var db = new ClothingStoreEntities();

        var users = db.Users.ToList();
    }
}