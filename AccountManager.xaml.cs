using AccountHelper.Models;
using AccountHelper.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AccountHelper
{
    public partial class AccountManager : Window
    {
        // Путь json
        private readonly string PATH = $"{Environment.CurrentDirectory}\\Accounts.json";
        private BindingList<AccountListModel> _accounts;
        private IOService _ioService;

        // Апдейт логинов
        public event EventHandler<List<string>> LoginsUpdated;

        private void OnLoginsUpdated(List<string> logins)
        {
            LoginsUpdated?.Invoke(this, logins);
        }

        private void UpdateLogins()
        {
            OnLoginsUpdated(_accounts.Select(account => account.Login).ToList());
        }

        // Апдейт паролей
        public event EventHandler<List<string>> PasswordsUpdated;

        private void OnpasswordsUpdate(List<string> passwords)
        {
            PasswordsUpdated?.Invoke(this, passwords);
        }
        
        private void UpdatePasswords()
        {
            OnpasswordsUpdate(_accounts.Select(account => account.Password).ToList());
        }

        public AccountManager()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void AccountManagerList_Loaded(object sender, RoutedEventArgs e)
        {
            // Получение данных из файла
            _ioService = new IOService(PATH);
            _accounts = _ioService.LoadData();

            try
            {
                // Загрузка данных из файла
                _accounts = _ioService.LoadData();

                // Проверка на null после загрузки данных
                if (_accounts == null)
                {
                    _accounts = new BindingList<AccountListModel>();
                }

                // Обновление логинов при первом запуске
                UpdateLogins();
                UpdatePasswords();
            }
            catch (Exception ex)
            {
                // Обработка исключения
                //MessageBox.Show(ex.Message);
                //Close();
                return; // Выход из метода, если возникла ошибка при загрузке данных
            }

            // Отображение списка в DataGrid
            AccountManagerList.ItemsSource = _accounts;

            // Функция при изменении списка
            _accounts.ListChanged += _accounts_ListChanged;
        }


        // Проверка изменений списка
        private void _accounts_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded ||
                e.ListChangedType == ListChangedType.ItemDeleted ||
                e.ListChangedType == ListChangedType.ItemChanged)
            {
                // Если успешно
                try
                {
                    _ioService.SaveData(sender);
                    UpdateLogins();
                    UpdatePasswords();
                }
                // Исключение
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Close();
                }
            }
        }

        // Закрытие окна
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        // Получение пароля по логину
        public string GetPasswordForLogin(string login)
        {
            AccountListModel account = _accounts.FirstOrDefault(a => a.Login == login);
            return account != null ? account.Password : null;
        }

        // Предотвращение редактирования
        private void AccountManagerList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridCell cell = GetCellFromMouseClick(e);
            if (cell != null && !cell.IsEditing)
            {
                e.Handled = true;
                cell.Focus();
                AccountManagerList.SelectedItem = cell.DataContext;
            }
        }

        private DataGridCell GetCellFromMouseClick(MouseButtonEventArgs e)
        {
            // Получаем элемент, на котором произошло событие
            var source = (DependencyObject)e.OriginalSource;

            // Поиск элемента типа DataGridCell
            while (source != null && !(source is DataGridCell))
            {
                source = VisualTreeHelper.GetParent(source);
            }

            return source as DataGridCell;
        }
    }
}