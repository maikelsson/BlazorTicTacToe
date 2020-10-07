    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BlazorServerApp_Chess.Data
{
    public interface IConnectionManager
    {
        void UpdateConnectionId(string username, string connectionId);
        void AddConnection(string username, string connectionId);
        void RemoveConnection(string connectionId);
        HashSet<string> GetConnections(string username);
        IEnumerable<string> OnlineUsers { get; }
    }
}
