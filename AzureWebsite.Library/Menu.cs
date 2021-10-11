using System.Collections.Generic;

namespace AzureWebApp.Library
{
    public class Menu
    {
        public List<MenuItem> MenuItems;
        public Menu()
        {
            MenuItems = Dummy.GetMenu(3, 5);
        }
    }
}
