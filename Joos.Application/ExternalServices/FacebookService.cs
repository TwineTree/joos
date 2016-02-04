using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Joos.ExternalServices
{
    public class FacebookService : IFacebookService
    {
        public const string UserProfileFormat = "https://graph.facebook.com/v2.5/me?access_token={0}&fields=email%2Cname%2Cbio%2Cfirst_name%2Clast_name%2Cgender%2Cpicture";

        public async Task<FacebookUserProfile> GetUserProfile(string accessToken)
        {
            var esh = new ExternalServiceHelper<FacebookUserProfile>();

            var url = string.Format(UserProfileFormat, accessToken);

            return await esh.Get(url);
        }
    }
}
