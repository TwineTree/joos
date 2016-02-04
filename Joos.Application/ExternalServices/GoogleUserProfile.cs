using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joos.ExternalServices
{

    public class GoogleName
    {
        public string givenName { get; set; }
        public string familyName { get; set; }
    }

    public class GoogleImage
    {
        public string url { get; set; }
        public bool isDefault { get; set; }
    }

    public class GoogleEmail
    {
        public string value { get; set; }
    }

    public class GoogleUserProfile
    {
        public string displayName { get; set; }
        public GoogleName name { get; set; }
        public string gender { get; set; }
        public GoogleImage image { get; set; }
        public string id { get; set; }
        public List<GoogleEmail> emails { get; set; }

        public GoogleError error { get; set; }
    }


    public class GoogleError
    {
        public string message { get; set; }
        public string type { get; set; }
        public int code { get; set; }
    }
}
