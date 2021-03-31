using Newtonsoft.Json;

namespace WDLT.Clients.Steam.Models.Json
{
    public class JLogin : JSuccess<bool>
    {
        [JsonProperty("emailauth_needed")]
        public bool EmailAuthNeeded { get; set; }

        [JsonProperty("captcha_needed")]
        public bool CaptchaNeeded { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("captcha_gid")]
        public string CaptchaGid { get; set; }

        [JsonProperty("emailsteamid")]
        public string EmailId { get; set; }

        [JsonProperty("bad_captcha")]
        public bool BadCaptcha { get; set; }

        [JsonProperty("requires_twofactor")]
        public bool RequiresTwoFactor { get; set; }

        [JsonProperty("login_complete")]
        public bool LoginComplete { get; set; }
    }
}