using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountHelper.Models
{
    class AccountListModel : INotifyPropertyChanged
    {
        // Объявление переменных
        private string _login;
        private string _password;
        // Логин
        public string Login
        {
            get { return _login; }
            set
            {
                if (_login == value)
                {
                    return;
                }
                else
                {
                    _login = value;
                    OnPropertyChange("Логин");
                }
                ;
            }
        }
        // Пароль
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value)
                {
                    return;
                }
                else
                {
                    _password = value;
                    OnPropertyChange("Пароль");
                }
                ;
            }
        }
        // События изменения списка
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChange(string propertyName = "")
        {
            // Проверка НЕ пустоты события
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}