using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tune.TwilioChat.Models
{
    public class TwilioAccount
    {
        public string AccountSid { get; set; }
        public string AuthToken { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string ChatServiceSid { get; set; }
        public string SyncServiceSid { get; set; }
        public string NotificationServiceSid { get; set; }
    }
}
