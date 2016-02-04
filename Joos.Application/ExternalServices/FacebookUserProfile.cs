using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joos.ExternalServices
{
    public class FacebookPictureData
    {
        public bool is_silhouette { get; set; }
        public string url { get; set; }
    }

    public class FacebookPicture
    {
        public FacebookPictureData data { get; set; }
    }

    public class FacebookUserProfile
    {
        public string email { get; set; }
        public string name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string gender { get; set; }
        public FacebookPicture picture { get; set; }
        public string id { get; set; }

        public FacebookError error { get; set; }

    }

    public class FacebookError
    {
        public string message { get; set; }
        public string type { get; set; }
        public int code { get; set; }
        public string fbtrace_id { get; set; }
    }
}
