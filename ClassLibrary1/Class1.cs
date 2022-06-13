using System;
using Microsoft.Data.Sqlite;
using System.Data.SQLite;

namespace ClassLibrary1
{
    public class User
    {
        public int id { get; set; }
        public string surname { get; set; }
        public string name { get; set; }
        public string patronymic { get; set; }
        public string login { get; set; }
        public string created { get; set; }
        public bool deleted = false;

        public User()
        {
            id = 0;
            surname = "";
            name = "";
            patronymic = "";
            login = "";
            created = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            deleted = false;
        }

        public User(int _id, string _surname, string _name, string _patronymic, string _login, string _created)
        {
            id = _id;
            surname = _surname;
            name = _name;
            patronymic = _patronymic;
            login = _login;
            created = _created;
            deleted = false;
        }
    }

    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        if (!File.Exists(@"UsersDB.db"))
    //        {
    //            SQLiteConnection.CreateFile(@"UsersDB.db");
    //        }


    //        SQLiteConnection Connect = new SQLiteConnection(@"Data Source=UsersDB.db;");

    //        string commandText = "CREATE TABLE IF NOT EXISTS Users ( id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
    //            "surname VARCHAR, name VARCHAR, patronymic VARCHAR, login VARCHAR, created DATETIME, deleted BOOLEAN)";
    //        SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
    //        Connect.Open(); 
    //        Command.ExecuteNonQuery(); 

    //        User user = new(6464576, "buba", "boba", "biba", "bbbbbb", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    //        Connect.Close();

    //    }
    //}
}