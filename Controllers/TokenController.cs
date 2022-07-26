using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tune.TwilioChat.Models;
using Twilio;
using Twilio.Jwt.AccessToken;

namespace Tune.TwilioChat.Controllers
{
    public class TokenController : Controller
    {
        private readonly TwilioAccount _twilioAccount;
        public TokenController(IOptions<TwilioAccount> twilioAccount)
        {
            if (twilioAccount == null)
            {
                throw new ArgumentNullException(nameof(twilioAccount));
            }
            _twilioAccount = twilioAccount.Value;

            // if the Sync Service SID is blank, use the default Sync Service instead
            if (_twilioAccount.SyncServiceSid == string.Empty)
            {
                _twilioAccount.SyncServiceSid = "default";
            }
            TwilioClient.Init(
                    _twilioAccount.ApiKey,
                    _twilioAccount.ApiSecret,
                    _twilioAccount.AccountSid
                );
        }

        // POST: Token
        [HttpPost("/token")]
        public ActionResult Index(string identity)
        {
            var token = CreateTokenResult(identity);
            return Json(new { identity, token });
        }
        private string CreateTokenResult(string identity)
        {
            var grants = new HashSet<IGrant>();

            if (_twilioAccount.ChatServiceSid != String.Empty)
            {
                // Create a "grant" which enables a client to use IPM as a given user,
                var chatGrant = new ChatGrant()
                {
                    ServiceSid = _twilioAccount.ChatServiceSid
                };
                grants.Add(chatGrant);
            }
            var token = new Token(
                _twilioAccount.AccountSid,
                _twilioAccount.ApiKey,
                _twilioAccount.ApiSecret,
                identity,
                grants: grants
            ).ToJwt();
            return token;
        }
    }
}
