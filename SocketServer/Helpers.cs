using System.Collections.Generic;
using System.Linq;

namespace SocketServer
{
    public static class Helpers
    {
        public static string ClientDataToView(Dictionary<string, List<int>> dict)
        {
            var data = string.Empty;

            foreach (string key in dict.Keys)
            {
                var sum = dict[key].ToArray().Aggregate((a, b) => a + b);
                data += key + " : " + sum + "\r\n";
            }

            return data;
        }
    }
}
