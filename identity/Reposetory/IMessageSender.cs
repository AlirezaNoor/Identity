using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace identity.Reposetory
{
    public interface IMessageSender
    {
        public Task SendEmailAsync(string toEmail, string subject, string message, bool isMessageHtml = false);
    }
}
