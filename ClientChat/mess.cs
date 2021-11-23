using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientChat
{
    public class mess
    {
        public mess(int id, int type, string content)
        {
            this.ID = id;
            this.type = type;
            this.content = content;
        }
        private int ID;
        public int ID1 { get => ID; set => ID = value; }
        public int Type { get => type; set => type = value; }
        public string Content { get => content; set => content = value; }
        private int type;
        private string content;
    }
}
