﻿using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OutReach.CoreSharedLib.Model.Common
{
    public class EmailMessage
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public EmailMessage(List<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress("Reciver", x)));
            Subject = subject;
            Content = content;
        }
    }
}
