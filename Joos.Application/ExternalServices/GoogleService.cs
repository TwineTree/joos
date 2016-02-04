using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joos.ExternalServices
{
    public class GoogleService : IGoogleService
    {
        public const string UserProfileFormat = "https://www.googleapis.com/plus/v1/people/{0}?fields=cover%2FcoverPhoto%2Furl%2CdisplayName%2Cemails%2Fvalue%2Cgender%2Cid%2Cimage%2Cname(familyName%2CgivenName)&access_token={1}";

        public async Task<GoogleUserProfile> getUserProfile(string accessToken, string id)
        {
            var esh = new ExternalServiceHelper<GoogleUserProfile>();

            var url = string.Format(UserProfileFormat, id, accessToken);

            var data = await esh.Get(url);

            return data;
        }
    }
}
