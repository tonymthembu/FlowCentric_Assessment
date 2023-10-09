using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;

namespace FlowCentric_Assessment.Models
{
    public class User: IdentityUser
    {
        
        public int UserId { get; set; }

        [JsonProperty("username")]
        public string username2 { get; set; }
        public string password { get; set;}
        public string role { get; set;}
        public List<string> orders { get; set;}
    }
}
