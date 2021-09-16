using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LusieyBackgroundService.Interface
{
    public interface IEmailTemplateService
    {
        public string SetEmailMessage(string Message/*string Template_Url, EmailMessage email*/);
    }
}
