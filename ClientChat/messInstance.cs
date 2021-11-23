using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientChat
{
    public class messInstance
    {
        private static messInstance _instance;
        public static messInstance Instance {
            get {
                if (_instance == null)
                    _instance = new messInstance();
                return _instance ;
            }
        }
        public List<mess> LoadMess(string namesend,string namerec,string s) {
            List<mess> result = new List<mess>();
            int count = 0;
            string nameSend = "", nameRec = "", content = "";
            int ID = 0;
            for (int i = 1; i < s.Length; i++){
                if (count == 3){
                    if (namesend == nameSend && namerec == nameRec) {
                        mess m = new mess(ID, 1, content);
                        result.Add(m);
                    }
                    else {
                        mess m = new mess(ID, -1, content);
                        result.Add(m);
                    }
                    nameSend = ""; nameRec = ""; content = "";
                    count = 0;
                    ID++;
                }
                if (s[i] == '*') i++;
                string countS = "";
                while (true){
                    countS += s[i];
                    i++;
                    if (s[i] == '*') break;
                }
                i++;
                int c = Int32.Parse(countS);
                for (int j = 0; j < c; j++){
                    if (count == 0) nameSend += s[i];
                    else if (count == 1) nameRec += s[i];
                    else if (count == 2) content += s[i];
                    i++;
                }
                count++;
            }
            if (count == 3){
                if (namesend == nameSend && namerec == nameRec){
                    mess m = new mess(ID, 1, content);
                    result.Add(m);
                }
                else{
                    mess m = new mess(ID, -1, content);
                    result.Add(m);
                }
                nameSend = ""; nameRec = ""; content = "";
                count = 0;
                ID++;
            }
            return result;
        }
    }
}
