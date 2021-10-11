using System;
using System.Collections.Generic;

namespace AzureWebApp.Library
{
    public static class Dummy
    {
        private static Random _randomizer = new Random(DateTime.Now.Millisecond);
        
        public static List<MenuItem> GetMenu(int depth, int maxItemsPerLevel)
        {
            var menu = new List<MenuItem>();
            var items = _randomizer.Next(maxItemsPerLevel+1);
            for (int i = 0; i < items; i++)
            {
                var menuItem = new MenuItem("Item_" + depth + "_" + i);
                if (depth > 0) menuItem.MenuItems = GetMenu(depth - 1, maxItemsPerLevel);
                menu.Add(menuItem);
            }
            return menu;
        }
    }
}
