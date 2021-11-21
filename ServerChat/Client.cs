using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerChat
{
    public class Client{
        public Client(string name,string ipPort) {
            this.name = name;
            this.ipPort = ipPort;   
        }
        private string name;
        private string ipPort;

        public string IpPort { get => ipPort; set => ipPort = value; }
        public string Name { get => name; set => name = value; }
    }
}
