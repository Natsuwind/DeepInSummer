using System;
using System.Collections.Generic;
using System.Text;

namespace Natsuhime.WebApp.Discuz
{
    public class Security
    {
        public static string GetFormHash(string pageContent)
        {
            string formhash;
            formhash = RegexUtility.GetMatch(pageContent, "name=\"formhash\" value=\"(.*)\"");
            if (formhash == string.Empty)
            {
                formhash = RegexUtility.GetMatch(pageContent, "formhash=(.*)\"");
            }
            return formhash;
        }
    }
}
