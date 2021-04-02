using HR_Management.Resources.Account;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources.Authentication
{
    public class TokenResource : AccountResource
    {
        [Display(Name = "Access Token")]
        [JsonProperty(Order = 1)]
        public string AccessToken { get; set; }
    }
}
