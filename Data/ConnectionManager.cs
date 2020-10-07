using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp_Chess.Data
{
    public class ConnectionManager : IConnectionManager
    {
        private static Dictionary<string, HashSet<string>> userMap = new Dictionary<string, HashSet<string>>();

        public IEnumerable<string> OnlineUsers { get { return userMap.Keys; } }

        public void UpdateConnectionId(string username, string connectionId)
        {
            lock (userMap)
            {
                if (!userMap.ContainsKey(username))
                {
                    return;
                } 
                else
                {
                    userMap[username] = new HashSet<string>();
                }
            }
        }

        public void AddConnection(string username, string connectionId)
        {
            lock (userMap)
            {
                if (!userMap.ContainsKey(username))
                {
                    userMap[username] = new HashSet<string>();
                }
                userMap[username].Add(connectionId);
            }
        }

        public void RemoveConnection(string connectionId)
        {

            lock (userMap)
            {
                foreach (var username in userMap.Keys)
                {
                    if (userMap.ContainsKey(username))
                    {
                        if (userMap[username].Contains(connectionId))
                        {
                            userMap[username].Remove(connectionId);
                            break;
                        }
                    }
                }
            }
        }

        public HashSet<string> GetConnections(string username)
        {
            var conn = new HashSet<string>();
            
            try
            {
                lock (userMap)
                {
                    conn = userMap[username];
                }
            }
            catch
            {
                conn = null;
            }

            return conn;
        }

        
    }
}
