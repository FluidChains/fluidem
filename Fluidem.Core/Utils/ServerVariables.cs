using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Fluidem.Core.Utils
{
    public class ServerVariables
    {
        public static async Task<string> AsJson(HttpContext context)
        {
            var variablesCollection = GetServerVariables(context);
            var variablesDic = variablesCollection.AllKeys.ToDictionary(
                key => key, k => variablesCollection[k]);
            return await JsonUtils.SerializeAsync(variablesDic);
        }

        // Taken from elmah core
        public static NameValueCollection GetServerVariables(HttpContext context)
        {
            var serverVariables = new NameValueCollection();
            LoadVariables(serverVariables, () => context.Features, "");
            LoadVariables(serverVariables, () => context.User, "User_");

            var ss = context.RequestServices?.GetService(typeof(ISession));
            if (ss != null)
                LoadVariables(serverVariables, () => context.Session, "Session_");
            LoadVariables(serverVariables, () => context.Items, "Items_");
            LoadVariables(serverVariables, () => context.Connection, "Connection_");
            return serverVariables;
        }

        public static void LoadVariables(NameValueCollection serverVariables, Func<object> getObject, string prefix)
        {
            var obj = getObject();
            if (obj == null) return;

            var props = obj.GetType().GetProperties();
            foreach (var prop in props)
            {
                object value = null;
                try
                {
                    value = prop.GetValue(obj);
                }
                catch
                {
                    // ignored
                }

                var isProcessed = false;
                if (value is IEnumerable en && !(en is string))
                {
                    if (en.GetType().FullName.StartsWith("Microsoft.AspNetCore.Http.ItemsDictionary"))
                    {
                        try
                        {
                            en.GetEnumerator();
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    foreach (var item in en)
                    {
                        try
                        {
                            var keyProp = item.GetType().GetProperty("Key");
                            var valueProp = item.GetType().GetProperty("Value");

                            if (keyProp == null || valueProp == null) continue;
                            isProcessed = true;
                            var valueObj = valueProp.GetValue(item);
                            var valueString = valueObj.ToString();
                            var keyString = keyProp.GetValue(item).ToString();
                            if (valueObj.GetType().ToString() == valueString || keyString == "Authorization") continue;
                            var prfix2 =
                                prop.Name.StartsWith("RequestHeaders",
                                    StringComparison.InvariantCultureIgnoreCase)
                                    ? "Header_"
                                    : prop.Name + "_";
                            serverVariables.Add(prefix + prfix2 + keyString, valueString);
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }

                if (isProcessed) continue;
                try
                {
                    if (value == null || value.GetType().ToString() != value.ToString())
                        serverVariables.Add(prefix + prop.Name, value?.ToString());
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}