using DaemonOfPain.Encryption;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaemonOfPain.Services.APIClasses
{
    public class ServerKeyAsker : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            EncryptionKeysManager.SetNewServerKey(await APIService.GetServerPublicKey());
        }
    }
}
