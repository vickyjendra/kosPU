using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using kosPU.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Mvc;
namespace kosPU.selectroom
{
    public class db
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString);
    
    public DataSet Get_TorsoType(string kost_id)
    {

        SqlCommand com = new SqlCommand("select * from roomid where kost_id_room = @kost_id", con); /*where sw_charID = @sw_charID*/
        com.Parameters.AddWithValue("@kost_id", kost_id);
        SqlDataAdapter da = new SqlDataAdapter(com);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;

        }
    }
}