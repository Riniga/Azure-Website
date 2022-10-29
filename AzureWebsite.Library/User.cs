using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AzureWebsite.Library
{
    public class User
    {
        public string id => Username;

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        User,
        Editor,
        Admin
    }
}
