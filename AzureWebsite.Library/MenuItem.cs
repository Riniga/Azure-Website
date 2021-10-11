﻿using System.Collections.Generic;

namespace AzureWebApp.Library
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
