﻿using Business.Resources.Account;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Business.Resources.Authentication
{
    public class AccessTokenResource : AccountResource
    {
        [Display(Name = "Access Token")]
        [JsonProperty(Order = 1)]
        public string AccessToken { get; set; }

        [Display(Name = "Token Resource")]
        [JsonProperty(Order = 2)]
        public TokenResource TokenResource { get; set; }
    }

    public class TokenResource
    {
        public int Id { get; set; }

        [Display(Name = "Refresh Token")]
        public string RefreshToken { get; set; }

        [Display(Name = "Expire Time")]
        public DateTime ExpireTime { get; set; }
    }
}
