using filecriptwpf.model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace filecriptwpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string valueFile = null;
        
        public MainWindow()
        {
            InitializeComponent();
            
        }
        

        private async Task DataEntry(int a)
        {
            int shift = -1;
            int keyshift = -1;
            if (valueFile == null)
            {
                MessageBox.Show("Файл не выбран. Пожалуйста, выберите файл для продолжения.");
                return;
            }
            try
            {
                shift = int.Parse(NumericTextBox.Text);
                keyshift = int.Parse(NumericTextBoxKey.Text);
            }
            catch
            {
                MessageBox.Show("Некорректно введены сдвиги файла или пароля. Пожалуйста, проверьте введенные данные и попробуйте снова.");
                return;
            }

            if ((shift <= 0 && shift >= 255) || (keyshift <= 0 && keyshift >= 255))
            {
                MessageBox.Show("Некорректно введены сдвиги файла или пароля. Пожалуйста, проверьте введенные данные и попробуйте снова.");
                return;
            }
            string key = NumericTextPass.Password;
            if (key == null || key == "")
            {
                MessageBox.Show("Пожалуйста, введите пароль.");
                return;
            }
            





            if (a == 1)
            {
                try
                {
                    await Task.Run(() => BaseModal.Encript(valueFile, shift, key, keyshift));
                }
                catch
                {
                    MessageBox.Show("Произошла ошибка во время шифрования. Возможно, программа или файл повреждены. Пожалуйста, проверьте файл и попробуйте снова. Если проблема сохраняется, переустановите программу.");
                }
                
            }
            if (a == 2)
            {
                try
                {
                    await Task.Run(() => BaseModal.Decript(valueFile, shift, key, keyshift));
                }
                catch
                {
                    MessageBox.Show("Произошла ошибка расшифровки. Это может быть связано с повреждением файла, некорректным форматом, неверным паролем или сдвигами. Пожалуйста, проверьте данные и попробуйте снова.");
                }
            }


        }
        
        
        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Разрешаем ввод только цифр
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }
        public static string SaveFileName()
        {
            // Создаем новый диалог сохранения файла
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Устанавливаем фильтр для типов файлов
            saveFileDialog.Filter = "All Files (*.*)|*.*";

            // Показываем диалог и проверяем, был ли выбран файл
            if (saveFileDialog.ShowDialog() == true)
            {
                // Получаем путь к файлу
                return saveFileDialog.FileName;
            }
            return null;
        }

        private void Button_Click_decript(object sender, RoutedEventArgs e)
        {
            DataEntry(2);
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Выберите файл",
                Filter = "Все файлы (*.*)|*.*",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                SelectedFileText.Text = "Выбран файл: " + openFileDialog.FileName;
                valueFile = openFileDialog.FileName;
            }
        }

        private void Button_Click_encript(object sender, RoutedEventArgs e)
        {
            DataEntry(1);
        }
    }
}
