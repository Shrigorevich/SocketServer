using System.Collections.Generic;

namespace SocketServer
{
    public class Storage
    {
        private Dictionary<string, List<int>> Data { get; set; } 

        public Storage()
        {
            Data = new Dictionary<string, List<int>>();
        }

        public void UpdateClientData(string ip, int number)
        {
            Data[ip].Add(number);
        }

        public void AddClient(string ip)
        {
            Data.Add(ip, new List<int>());
        }

        public void RemoveClient(string ip)
        {
            Data.Remove(ip);
        }

        public Dictionary<string, List<int>> QueryAll()
        {
            return Data;
        }

        public List<int> QuerySingle(string ip)
        {
            return Data[ip];
        }
    }
}
