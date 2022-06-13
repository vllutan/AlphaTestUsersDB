using System;

namespace WpfApp1
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
}
