using System.Collections.Generic;

namespace AzureWebsite.Library
{
    public class MenuItem
    {
        public string Title { get; set; }
        public List<MenuItem> MenuItems;

        public MenuItem(string title)
        {
            Title = title;
        }
    }
}
