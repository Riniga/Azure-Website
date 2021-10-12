using System.Collections.Generic;

namespace AzureWebsite.Library
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
