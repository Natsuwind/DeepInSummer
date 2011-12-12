using System;
using System.Collections.Generic;
using System.Text;

namespace LiteCMS.Plugin
{
    public class ProviderUtitily
    {
        public static IUserProvider GetUserProvider(string name)
        {
            return (IUserProvider)Activator.CreateInstance(Type.GetType(string.Format("LiteCMS.Plugin.UserProvider.{0}, LiteCMS.Plugin.UserProvider.{0}", name), false, true));
        }
    }
}
