using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace Joos.Api.Controllers.Results
{
    public class ApiChallengeResult : UnauthorizedResult
    {
        public ApiChallengeResult(string provider, string redirectUri, HttpRequestMessage message)
            : this(provider, redirectUri, null, message)
        {
        }

        public ApiChallengeResult(string provider, string redirectUri, string userId, HttpRequestMessage message)
            :base(new List<AuthenticationHeaderValue>() { new AuthenticationHeaderValue("WWW-Authenticate", "Bearer")}, message)
        {
            LoginProvider = provider;
            RedirectUri = redirectUri;
            UserId = userId;
        }

        public string LoginProvider { get; set; }
        public string RedirectUri { get; set; }
        public string UserId { get; set; }
    }
}
