using System;
using System.Collections.Generic;
using System.Data.SQLite;
using ClassLibrary1;
using System.IO;

namespace WpfApp1
{
    public class DbCreator
    {
        public SQLiteConnection dbConnection;
        SQLiteCommand command;
        string sqlCommand;
        string dbPath = System.Environment.CurrentDirectory + "\\DB";
        string dbFilePath;
        int lastId = 1;
        public List<User> ExistingUsers;
        public List<User> AllUsers;
        public void createDbFile()
        {
            if (!string.IsNullOrEmpty(dbPath) && !Directory.Exists(dbPath))
                Directory.CreateDirectory(dbPath);
            dbFilePath = dbPath + "\\UsersDB.db";
            if (!System.IO.File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);
            }
        }

        public string createDbConnection()
        {
            string strCon = string.Format("Data Source={0};", dbFilePath);
            dbConnection = new SQLiteConnection(strCon);
            dbConnection.Open();
            command = dbConnection.CreateCommand();
            return strCon;
        }

        public int createTables()
        {
            if (!checkIfExist("USERS"))
            {
                sqlCommand = "CREATE TABLE USERS(id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                    "surname VARCHAR, name VARCHAR, patronymic VARCHAR, login VARCHAR, created DATETIME, deleted BOOLEAN)";
                executeQuery(sqlCommand);
                lastId = 1;
            } else
            {
                SQLiteCommand cmd = new SQLiteCommand("Select max(id) from USERS", dbConnection);
                SQLiteDataReader dr = cmd.ExecuteReader();
                if(dr.HasRows)
                    while (dr.Read())
                        lastId = Convert.ToInt32(dr[0]) + 1;
                dr.Close();


            }
            return lastId;

        }

        public bool checkIfExist(string tableName)
        {
            command.CommandText = "SELECT name FROM sqlite_master WHERE name='" + tableName + "'";
            var result = command.ExecuteScalar();

            return result != null && result.ToString() == tableName ? true : false;
        }

        public void executeQuery(string sqlCommand)
        {
            SQLiteCommand triggerCommand = dbConnection.CreateCommand();
            triggerCommand.CommandText = sqlCommand;
            triggerCommand.ExecuteNonQuery();
        }

        public bool checkIfTableContainsData(string tableName)
        {
            command.CommandText = "SELECT count(*) FROM " + tableName;
            var result = command.ExecuteScalar();

            return Convert.ToInt32(result) > 0 ? true : false;
        }

        public List<User> getExsistingValues()
        {
            ExistingUsers = new();
            command.CommandText = "select * from USERS where deleted=0";
            SQLiteDataReader dr = command.ExecuteReader();
            if (dr.HasRows)
                while (dr.Read())
                {
                    ExistingUsers.Add(new User( (int)(long)dr["id"], (string)dr["surname"], (string)dr["name"], (string)dr["patronymic"],
                                                (string)dr["login"], dr["created"].ToString() ));
                }
            dr.Close();
            return ExistingUsers;
        }
        
    }
}
