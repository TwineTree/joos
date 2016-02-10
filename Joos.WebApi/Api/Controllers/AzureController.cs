using Abp.Web.Models;
using Abp.WebApi.Authorization;
using Abp.WebApi.Controllers;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Globalization;
using Joos.Api.Models;

namespace Joos.Api.Controllers
{
    [AbpApiAuthorize]
    public class AzureController : AbpApiController
    {
        private readonly CloudStorageAccount _storageAccount;

        public AzureController()
        {
            _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
        }

        [HttpGet]
        public AjaxResponse GetSass(string blobUri, string containerName, string method)
        {
            var blobClient = _storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);
            container.CreateIfNotExists();

            var sas = GetSasForBlob(_storageAccount.Credentials, blobUri, method);
            return new AjaxResponse(sas);
        }

        private string GetSasForBlob(StorageCredentials credentials, string blobUri, string method)
        {
            var blob = new CloudBlockBlob(new Uri(blobUri), credentials);
            var permission = SharedAccessBlobPermissions.Write;

            var sas = blob.GetSharedAccessSignature(new SharedAccessBlobPolicy()
            {
                Permissions = permission,
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15),
            });

            return string.Format(CultureInfo.InvariantCulture, "{0}{1}", blob.Uri, sas);
        }
    }
}
