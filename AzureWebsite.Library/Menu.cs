using System;
using System.Collections.Generic;

namespace AzureWebsite.Library
{
    public class Menu
    {
        public Guid id { get; set; }
        public List<MenuItem> MenuItems;
        
        public void GenerateDummy()
        {
            id = Guid.NewGuid();
            MenuItems = Dummy.GetMenu(3, 5);
        }
    }
}
