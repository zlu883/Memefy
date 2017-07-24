using Memefy.Model;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Memefy
{
    public class AzureManager
    {

        private static AzureManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<MemeCaptions> CaptionTable;

        private AzureManager()
        {
            this.client = new MobileServiceClient("http://memefy.azurewebsites.net");
            this.CaptionTable = this.client.GetTable<MemeCaptions>();
        }

        public MobileServiceClient AzureClient
        {
            get { return client; }
        }

        public static AzureManager AzureManagerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureManager();
                }

                return instance;
            }
        }

        public async Task<List<MemeCaptions>> GetCaptionList()
        {
            return await this.CaptionTable.ToListAsync();
        }
    }
}
