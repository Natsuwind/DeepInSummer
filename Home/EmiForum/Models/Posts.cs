using System;
using System.Collections.Generic;
using Natsuhime.Data;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace EmiForum.Models
{
    public class Posts
    {
        public static List<PostInfo> GetPostList()
        {
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, "SELECT * FROM posts ORDER BY pid DESC");

            List<PostInfo> list = new List<PostInfo>();
            while (dr.Read())
            {
                PostInfo p = new PostInfo();
                p.Pid = Convert.ToInt32(dr["pid"]);
                p.Poster = dr["poster"].ToString();
                p.PosterId = Convert.ToInt32(dr["posterid"]);
                p.Content = dr["content"].ToString();
                p.PostDate = Convert.ToDateTime(dr["postdate"]);
                p.Ip = dr["ip"].ToString();
                p.Email = dr["email"].ToString();
                p.Website = dr["website"].ToString();
                list.Add(p);
            }
            dr.Close();
            return list;
        }

        public static void CreatePost(PostInfo newPost)
        {
            DbParameter[] prams = 
		    {
			    DbHelper.MakeInParam("?poster", (DbType)MySqlDbType.String, 50,newPost.Poster),
			    DbHelper.MakeInParam("?posterid", (DbType)MySqlDbType.Int32, 4,newPost.PosterId),
			    DbHelper.MakeInParam("?content", (DbType)MySqlDbType.String, 5000,newPost.Content),
			    DbHelper.MakeInParam("?postdate", (DbType)MySqlDbType.DateTime, 8,newPost.PostDate),
			    DbHelper.MakeInParam("?ip", (DbType)MySqlDbType.String, 50,newPost.Ip),
			    DbHelper.MakeInParam("?email", (DbType)MySqlDbType.String, 100,newPost.Email),
			    DbHelper.MakeInParam("?website", (DbType)MySqlDbType.String, 100,newPost.Website)
		    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "INSERT INTO posts (poster,posterid,content,postdate,ip,email,website) VALUES(?poster,?posterid,?content,?postdate,?ip,?email,?website)", prams);
        }
    }
}