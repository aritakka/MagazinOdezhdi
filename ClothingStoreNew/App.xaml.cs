using System.Windows;

namespace ClothingStoreNew
{
    public partial class App : Application
    {
        public static Users CurrentUser { get; set; }

        public static bool IsAdmin =>
            CurrentUser != null && CurrentUser.Role == "Admin";
    }
}