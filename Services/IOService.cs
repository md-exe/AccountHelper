using Newtonsoft.Json;
using AccountHelper.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountHelper.Services
{
    class IOService
    {
        // Объявление переменной пути
        private readonly string PATH;
        public IOService(string path)
        {
            PATH = path;
        }
        // Загрузка данных из файла
        public BindingList<AccountListModel> LoadData()
        {
            // Проверка наличия файла
            if (!File.Exists(PATH))
            {
                // Если файла нет, создайте новый и верните пустой список
                File.WriteAllText(PATH, string.Empty);
                return new BindingList<AccountListModel>();
            }

            // Чтение данных из файла
            var fileText = File.ReadAllText(PATH);

            // Проверка на пустоту файла
            if (string.IsNullOrWhiteSpace(fileText))
            {
                // Если файл пуст, создайте новый и верните пустой список
                File.WriteAllText(PATH, string.Empty);
                return new BindingList<AccountListModel>();
            }

            var accounts = JsonConvert.DeserializeObject<BindingList<AccountListModel>>(fileText);

            // Проверка на null после десериализации
            if (accounts == null)
            {
                accounts = new BindingList<AccountListModel>();
            }

            return accounts;
        }


        // Сохранение списка
        public void SaveData(object toDoData)
        {
            using (StreamWriter writer = File.CreateText(PATH))
            {
                string output = JsonConvert.SerializeObject(toDoData);
                writer.WriteLine(output);
                writer.Close();
            }
        }
    }
}
