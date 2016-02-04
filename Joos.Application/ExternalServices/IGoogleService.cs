using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joos.ExternalServices
{
    public interface IGoogleService : IApplicationService
    {
        Task<GoogleUserProfile> getUserProfile(string accessToken, string id);
    }
}
