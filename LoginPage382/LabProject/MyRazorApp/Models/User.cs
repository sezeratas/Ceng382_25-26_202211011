using System;
using System.Text.Json.Serialization;

namespace MyRazorApp.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        
        [JsonPropertyName("CreatedAt")]
        public DateTime CreatedAt { get; set; }
    }
}