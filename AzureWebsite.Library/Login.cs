using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AzureWebsite.Library
{
    public class Login
    {
        public string id => Username;
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        public string Name { get; set; }
        public UserRole Role { get; set; }
        public string Token { get; set; }
    }
}
