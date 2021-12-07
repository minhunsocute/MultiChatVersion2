using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace ServerChat
{
    public class sql_manage
    {
        private string conStr = @"Data Source=LAPTOP-DI57MUOG;Initial Catalog=MULTICHAT;Integrated Security=True";
        private SqlConnection conn;
        private SqlDataAdapter myAdapter;
        private SqlCommand comm;
        private DataSet ds;
        private DataTable dt;

        //reload datagridview when data change
        public void reLoadgridview(string s, string sql, DataGridView table)
        {
            conn = new SqlConnection(conStr);
            string sqlString = sql;
            myAdapter = new SqlDataAdapter(sqlString, conn);
            ds = new DataSet();
            myAdapter.Fill(ds, "id");
            dt = ds.Tables["id"];   
            table.DataSource = dt;
            conn.Close();
        }
        // count number 
        public int returnNo(string username, string pass, int type)
        {
            int check = 0;
            conn = new SqlConnection(conStr);
            conn.Open();
            string sqlString = "";
            if (type == 1)
            {
                sqlString = $"SELECT COUNT(*) FROM CLIENT WHERE USERNAME ='{username}' AND PASSWORD='{pass}'";
            }
            else
            {
                sqlString = $"SELECT COUNT(*) FROM CLIENT WHERE USERNAME ='{username}'";
            }
            comm = new SqlCommand(sqlString, conn);
            Int32 count = (Int32)comm.ExecuteScalar();
            conn.Close();
            if (count != 0)
                return -1;
            return check;
        }
        public void inserAccount(string username, string pass, string name,string avtName)
        {
            conn = new SqlConnection(conStr);
            conn.Open();
            try
            {
                string sqlString = $"INSERT INTO CLIENT (USERNAME,PASSWORD,NAME_INMESSAGE,TYPE_ACTI,AVT) VALUES('{username}'," +
                                $"'{pass}',N'{name}',0,N'{avtName}')";
                comm = new SqlCommand(sqlString, conn);
                comm.ExecuteNonQuery();
            }
            catch { }
            conn.Close();
        }
        public void updateActi(string userName, int type)
        {
            conn = new SqlConnection(conStr);
            conn.Open();
            string sqlString = "";
            if (type == 1)
            {
                sqlString = $"UPDATE CLIENT SET TYPE_ACTI = 1 WHERE USERNAME = '{userName}'";
            }
            else
            {
                sqlString = $"UPDATE CLIENT SET TYPE_ACTI = 0 WHERE USERNAME = '{userName}'";
            }
            comm = new SqlCommand(sqlString, conn);
            comm.ExecuteNonQuery();
            conn.Close();
        }
        public void Loaddata(DataGridView table, string ipPort, string userName, int type)
        {
            if (type == 0){
                Client c = new Client(userName, ipPort);
                Server.listCList.Add(c);
            }
            foreach (Client item in Server.listCList)
            {
                DataGridViewRow row = (DataGridViewRow)table.Rows[0].Clone();
                row.Cells[0].Value = item.Name;
                row.Cells[1].Value = item.IpPort;
                table.Rows.Add(row);
            }
        }
        public string getListClientActi(string userName)
        {
            string sendString = "6";
            conn = new SqlConnection(conStr);
            conn.Open();
            string sqlString = $"SELECT USERNAME,AVT FROM CLIENT WHERE TYPE_ACTI = 1";
            myAdapter = new SqlDataAdapter(sqlString, conn);
            ds = new DataSet();
            dt = new DataTable();
            myAdapter.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sendString += $"{dt.Rows[i][0].ToString()}:{dt.Rows[i][1].ToString()}";
                if (i != dt.Rows.Count-1) sendString += ":";
            }
            conn.Close();
            return sendString;
        }
        public string getListClient(string userName)
        {
            string sendString = "a";
            conn = new SqlConnection(conStr);
            conn.Open();
            string sqlString = $"SELECT USERNAME,AVT FROM CLIENT";
            myAdapter = new SqlDataAdapter(sqlString, conn);
            ds = new DataSet();
            dt = new DataTable();
            myAdapter.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sendString += $"{dt.Rows[i][0].ToString()}:{dt.Rows[i][1].ToString()}";
                if (i != dt.Rows.Count-1) sendString += ":";
            }
            conn.Close();
            return sendString;
        }
        public void refreshAllData()
        {
            conn = new SqlConnection(conStr);
            conn.Open();
            string sqlString = "UPDATE CLIENT SET TYPE_ACTI = 0";
            comm = new SqlCommand(sqlString, conn);
            comm.ExecuteNonQuery();
            conn.Close();
        }
        public void updateAvt(string userName, string Image)
        {
            conn = new SqlConnection(conStr);
            conn.Open();
            string sqlString = $"UPDATE CLIENT SET AVT={Image} WHERE USERNAME ={userName}";
            comm = new SqlCommand(sqlString, conn);
            comm.ExecuteNonQuery();
            conn.Close();
        }
        public string LoadMess(string nameSend, string nameRec)
        {
            string sendString = "7";
            conn = new SqlConnection(conStr);
            conn.Open();
            string sqlString = $"EXEC LOAD_MESS '{nameSend}','{nameRec}' ";
            myAdapter = new SqlDataAdapter(sqlString, conn);
            ds = new DataSet();
            myAdapter.Fill(ds, "id");
            dt = ds.Tables["id"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string name_send = dt.Rows[i]["NAMESEND"].ToString();
                string name_rec = dt.Rows[i]["NAMERECEVIE"].ToString();
                string content = dt.Rows[i]["CONTENT"].ToString();
                string typeMess = dt.Rows[i]["TYPE_MESS"].ToString();
                sendString += $"*{name_send.Length.ToString()}*{name_send}*{name_rec.Length.ToString()}*{name_rec}*{(content.Length+1).ToString()}*{typeMess}{content}";
            }
            return sendString;
        }

        public void InsertMess(string nameSend, string nameRec, string content,int type)
        {
            conn = new SqlConnection(conStr);
            conn.Open();
            string sqlString = $"EXEC INSERT_MESS '{nameSend}','{nameRec}',N'{content}',{type.ToString()}";
            comm = new SqlCommand(sqlString, conn);
            comm.ExecuteNonQuery();
            conn.Close();
        }

        public string getAvtName(string userName)
        {
            string avtName = "";
            conn = new SqlConnection(conStr);
            conn.Open();
            string sqlString = "SELECT AVT FROM CLIENT WHERE USERNAME = N'" + userName + "'";
            myAdapter = new SqlDataAdapter(sqlString, conn);
            ds = new DataSet();
            myAdapter.Fill(ds, "AVT");
            dt = ds.Tables["AVT"];
            avtName = dt.Rows[0]["AVT"].ToString();
            return avtName;
        }

        public void InsertGroup(string nameGroup) {
            conn = new SqlConnection(conStr);
            conn.Open();
            string sqlString = $"INSERT INTO ROOM (NAME_ROOM) VALUES (N'{nameGroup}')";
            comm = new SqlCommand(sqlString, conn);
            comm.ExecuteNonQuery();
            conn.Close();
        }
        public void insertMemGroup(string nameGroup,string allMem) {
            conn = new SqlConnection(conStr);
            conn.Open();
            string sqlString = $"SELECT ID FROM ROOM WHERE NAME_ROOM = N'{nameGroup}'";
            myAdapter = new SqlDataAdapter(sqlString, conn);
            ds = new DataSet();
            myAdapter.Fill(ds, "ID");
            dt = ds.Tables["ID"];
            string iDroom = dt.Rows[0][0].ToString();
            string[] listClient = allMem.Split(':');
            foreach(string item in listClient) {
                if (!string.IsNullOrEmpty(item))
                {
                    sqlString = $"INSERT INTO MEM_ROOM (IDROOM,IDCLIENT) VALUES({iDroom},N'{item}')";
                    comm = new SqlCommand(sqlString, conn);
                    comm.ExecuteNonQuery();
                }
            }
            conn.Close();
        }
        public string loadGroup(string userName) {
            string group = "b";
            conn = new SqlConnection(conStr);
            conn.Open();
            string sqlString = $"EXEC LOADGROUP '{userName}'";
            myAdapter = new SqlDataAdapter(sqlString, conn);
            ds = new DataSet();
            myAdapter.Fill(ds, "ID");
            dt = ds.Tables["ID"];
            SqlDataAdapter myAdapter1;
            DataSet ds1;
            DataTable dt1;
            for(int i = 0; i < dt.Rows.Count; i++) {
                group += $"{dt.Rows[i]["ID"].ToString()}:{dt.Rows[i]["NAME_ROOM"].ToString()}:";
                string sqlString1 = $"EXEC LOADMEMGROUP {dt.Rows[i]["ID"].ToString()}";
                myAdapter1 = new SqlDataAdapter(sqlString1, conn);
                ds1 = new DataSet();
                myAdapter1.Fill(ds1, "IDCLIENT");
                dt1 = ds1.Tables["IDCLIENT"];
                group += $"{dt1.Rows.Count.ToString()}:";
                for(int j = 0; j < dt1.Rows.Count; j++) {
                    group += $"{dt1.Rows[j][0].ToString()}:{dt1.Rows[j][1].ToString()}:";
                }
            }
            return group;
        }
        public string loadMessageGroup(string idGroup) {
            string str = "c";
            conn = new SqlConnection(conStr);
            conn.Open();
            string sqlString = $"exec LOADMESSGROUP {idGroup}";
            myAdapter = new SqlDataAdapter(sqlString, conn);
            ds = new DataSet();
            myAdapter.Fill(ds, "ID");
            dt = ds.Tables["ID"];
            for(int i=0;i<dt.Rows.Count;i++) {
                string name_send = dt.Rows[i]["NAMESEND"].ToString();
                string name_rec = dt.Rows[i]["NAMERECEVIE"].ToString();
                string content = dt.Rows[i]["CONTENT"].ToString();
                string typeMess = dt.Rows[i]["TYPE_MESS"].ToString();
                str += $"*{name_send.Length.ToString()}*{name_send}*{name_rec.Length.ToString()}*{name_rec}*{(content.Length + 1).ToString()}*{typeMess}{content}";
            }
            return str;
        }
    }
}
