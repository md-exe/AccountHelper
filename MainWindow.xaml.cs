using AccountHelper.Models;
using AccountHelper.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AccountHelper
{
    public partial class MainWindow : Window
    {
        // Инициализация окна менеджера
        private AccountManager _accountManager;

        // Хук окон
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        const int SW_RESTORE = 9;
        string WoWprocessName = "Wow";

        public MainWindow()
        {
            InitializeComponent();
            _accountManager = new AccountManager();
            _accountManager.LoginsUpdated += AccountManager_LoginsUpdated;
            _accountManager.Show();
            _accountManager.Hide();
        }

        // Апдейт логинов
        private void AccountManager_LoginsUpdated(object sender, List<string> logins)
        {
            // Обновление данных в комбобоксе
            ChooseAccComboBox.ItemsSource = logins;
        }

        // Кнопка менеджера аккаунтов
        private void AddAccountButton_Click(object sender, RoutedEventArgs e)
        {
            _accountManager.Show();

        }

        // Полное закрытие приложения
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        // Войти
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Поиск процесса
            Process[] processes = Process.GetProcessesByName(WoWprocessName);
            

            // Получение логина и пароля из датагрида
            string selectedLogin = ChooseAccComboBox.SelectedItem as string;
            string selectedPassword = _accountManager.GetPasswordForLogin(selectedLogin);

            if (processes.Length > 0)
            {
                IntPtr mainWindowHandle = processes[0].MainWindowHandle;
                ShowWindowAsync(mainWindowHandle, SW_RESTORE);
                if (selectedLogin != null)
                {
                    if (selectedPassword != null)
                    {
                        
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("У аккаунта отсутствует пароль.", "Подожди!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Нужно выбрать аккаунт.", "Подожди!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //SetForegroundWindow(mainWindowHandle);
                if (SetForegroundWindow(mainWindowHandle)) // Если окно развёрнуто
                {
                    SendKeys.SendWait(selectedLogin);
                    SendKeys.SendWait("{TAB}");
                    SendKeys.SendWait(selectedPassword);
                    SendKeys.SendWait("{ENTER}");
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Игра не запущена.","Ошибка!",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            
        }
    }
}