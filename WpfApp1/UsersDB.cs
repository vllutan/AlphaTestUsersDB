using System;
using System.Collections.Generic;
using System.Windows;
using System.Data.SQLite;
using System.ComponentModel;
using System.Data;
using ClosedXML.Excel;
using Microsoft.Win32;

namespace WpfApp1
{
    public class UsersDB : INotifyPropertyChanged
    {
        DbCreator dbCreator = new DbCreator();
        public List<User> ListOfNotNullUsers
        {
            get { return dbCreator.getExsistingValues(); }
        }

        User selectedUser;
        public User SelectedUser
        {
            get { return selectedUser; }
            set { selectedUser = value; OnPropertyChanged(nameof(SelectedUser)); }
        }

        int idCounter { get; set; } = 1;

        string surname;
        public string Surname
        {
            get { return surname; }
            set { surname = value; OnPropertyChanged(nameof(Surname)); }
        }
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }
        string patronymic;
        public string Patronymic
        {
            get { return patronymic; }
            set { patronymic = value; OnPropertyChanged(nameof(Patronymic)); }
        }
        string login;
        public string Login
        {
            get { return login; }
            set { login = value; OnPropertyChanged(nameof(Login)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string property_name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property_name));
            }
        }

        public RelayCommand EditUser { get; private set; }
        public RelayCommand DeleteUser { get; private set; }
        public RelayCommand UpdateUser { get; private set; }
        public RelayCommand AddUser { get; private set; }
        public RelayCommand ConvertToExcel { get; private set; }

        public UsersDB()
        {
            dbCreator = new DbCreator();
            dbCreator.createDbFile();
            dbCreator.createDbConnection();
            idCounter = dbCreator.createTables();

            EditUser = new RelayCommand(_ =>
            {
                if (SelectedUser != null)
                {
                    Surname = SelectedUser.surname;
                    Name = SelectedUser.name;
                    Patronymic = SelectedUser.patronymic;
                    Login = SelectedUser.login;
                }
            } 
            , (_) => (SelectedUser != null));

            DeleteUser = new RelayCommand(_ =>
            {
                if (SelectedUser != null)
                {
                    string sqlCmd = "update USERS set deleted=1 where id=" + SelectedUser.id.ToString();
                    dbCreator.executeQuery(sqlCmd);
                    OnPropertyChanged(nameof(ListOfNotNullUsers));
                }
            } 
             , (_) => (SelectedUser != null));

            UpdateUser = new RelayCommand(_ =>
            {
                string sqlCmd = "update USERS set name=\'"+Name+"\', surname=\'"+Surname+"\', patronymic=\'"+Patronymic+"\', login=\'"+Login+"\'" +
                                " where id=" + SelectedUser.id.ToString();
                dbCreator.executeQuery(sqlCmd);
                OnPropertyChanged(nameof(ListOfNotNullUsers));
            }
            , (_) => (SelectedUser != null));

            AddUser = new RelayCommand(_ =>
            {   
                var dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string sqlCmd = $"INSERT INTO USERS VALUES ({idCounter}, \'{Surname}\', \'{Name}\', \'{Patronymic}\', \'{Login}\', " +
                                $"\'{dt}\', 0)";
                dbCreator.executeQuery(sqlCmd);
                idCounter++;
                OnPropertyChanged(nameof(ListOfNotNullUsers));

            });

            ConvertToExcel = new RelayCommand(_ =>
            {
                SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbool|*.xlsx" };
                //using (sfd)
                //{
                if (sfd.ShowDialog() == true)
                {
                    try
                    {
                        //using (var con = dbCreator.dbConnection)
                        using (SQLiteConnection con = new SQLiteConnection())
                        {
                            using (SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM USERS"))
                            {
                                using (SQLiteDataAdapter sda = new SQLiteDataAdapter())
                                {
                                    cmd.Connection = dbCreator.dbConnection; //con;
                                    sda.SelectCommand = cmd;
                                    using (DataTable dt = new DataTable())
                                    {
                                        sda.Fill(dt);
                                        using (XLWorkbook workbook = new XLWorkbook())
                                        {
                                            var ws = workbook.Worksheets.Add(dt, "Users");
                                            // bool TryGetWorksheet(string sheetName, out IXLWorksheet worksheet);
                                            //IXLWorksheet worksheet;
                                            //workbook.Worksheets.TryGetWorksheet("Users", out worksheet);


                                            workbook.SaveAs(sfd.FileName);
                                        }
                                        MessageBox.Show("Success.", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                //}
            });

        }


    }
}
