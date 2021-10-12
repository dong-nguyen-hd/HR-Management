using Business.Resources.Account;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Business.Resources.Authentication
{
    public class TokenResource : AccountResource
    {
        [Display(Name = "Access Token")]
        [JsonProperty(Order = 1)]
        public string AccessToken { get; set; }
    }
}
