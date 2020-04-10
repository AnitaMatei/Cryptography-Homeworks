using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassicCiphers.Ciphers;
using System.IO;

namespace ClassicCiphers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        static int AvailableCipherCount = 5;
        TextBox[] textBoxes = new TextBox[AvailableCipherCount];
        Button[] buttons = new Button[AvailableCipherCount];
        GenericCipher[] ciphers = new GenericCipher[AvailableCipherCount];

        public MainWindow()
        {
            InitializeComponent();

            for (int i = 0; i < AvailableCipherCount; i++)
            {
                textBoxes[i] = this.FindName("textBox" + i.ToString()) as TextBox;
                buttons[i] = this.FindName(textBoxes[i].Name + "Button") as Button;
                textBoxes[i].Visibility = Visibility.Hidden;
                buttons[i].Visibility = Visibility.Hidden;
            }
        }

        private void SwapText(object sender, RoutedEventArgs e)
        {
            String tempString = inputTextBox.Text;
            inputTextBox.Text = outputTextBox.Text;
            outputTextBox.Text = tempString;
        }

        private bool LoadCiphers()
        {
            try
            {
                int i = 0;
                if (inputTextBox.Text.Equals(""))
                    throw new FormatException("You have not introduced any text in the clear text box!");

                foreach (ListBoxItem currentItem in usedCiphers.Items)
                {
                    if (textBoxes[i].Text.Equals(""))
                        throw new FormatException("You must introduce a key for each ciphers!");
                    switch (currentItem.Content)
                    {
                        case "Caesar":
                            ciphers[i] = new CaesarCipher();
                            break;
                        case "Nihilist":
                            ciphers[i] = new NihilistCipher();
                            break;
                        case "Bifid":
                            ciphers[i] = new BifidCipher();
                            break;
                    }
                    ciphers[i].SetKey(textBoxes[i].Text);
                    textBoxes[i].Text = ciphers[i].GetKeyValue();
                    i++;
                }
            }
            catch (FormatException ex)
            {
                errorsTextBox.Text = ex.Message;
                return false;
            }
            errorsTextBox.Text = "";
            return true;
        }

        private void NormalizeInput()
        {
            inputTextBox.Text = inputTextBox.Text.ToLower();
            foreach (TextBox currentTextBox in textBoxes)
            {
                if (!currentTextBox.IsVisible)
                    return;
                currentTextBox.Text = currentTextBox.Text.ToLower();
            }
        }

        private void EncryptText(object sender, RoutedEventArgs e)
        {
            if (!LoadCiphers())
                return;
            NormalizeInput();
            String encryptedText = inputTextBox.Text;
            for (int i = 0; i < usedCiphers.Items.Count; i++)
                encryptedText = ciphers[i].Encrypt(encryptedText);
            outputTextBox.Text = encryptedText;
        }

        private void DecryptText(object sender, RoutedEventArgs e)
        {
            if (!LoadCiphers())
                return;
            NormalizeInput();
            String decryptedText = inputTextBox.Text;
            for (int i = usedCiphers.Items.Count - 1; i >= 0; i--)
                decryptedText = ciphers[i].Decrypt(decryptedText);
            outputTextBox.Text = decryptedText;
        }

        private void UpdateCipherKeyTextBoxes()
        {
            int i = 0;
            foreach (TextBox currentTextBox in textBoxes)
            {
                currentTextBox.Visibility = Visibility.Hidden;
                buttons[i].Visibility = Visibility.Hidden;
                i++;
            }
            i = 0;
            foreach (ListBoxItem currentItem in usedCiphers.Items)
            {
                textBoxes[i].Visibility = Visibility.Visible;
                buttons[i].Visibility = Visibility.Visible;
                switch (currentItem.Content)
                {
                    case "Nihilist":
                        textBoxes[i].Text = NihilistCipher.DefaultKeyString;
                        break;
                    case "Caesar":
                        textBoxes[i].Text = CaesarCipher.DefaultKeyString;
                        break;
                    case "Bifid":
                        textBoxes[i].Text = BifidCipher.DefaultKeyString;
                        break;
                }
                textBoxes[i].Name = currentItem.Content + "Key" + "TextBox";
                i++;
            }
        }
        private void SelectAvailableCipher(object sender, RoutedEventArgs e)
        {
            foreach (ListBoxItem listBoxItem in availableCiphers.Items)
            {
                if (listBoxItem.IsSelected)
                {
                    availableCiphers.Items.Remove(listBoxItem);
                    usedCiphers.Items.Add(listBoxItem);

                    UpdateCipherKeyTextBoxes();

                    break;
                }
            }
        }

        private void RemoveUsedCipher(object sender, RoutedEventArgs e)
        {
            foreach (ListBoxItem listBoxItem in usedCiphers.Items)
            {
                if (listBoxItem.IsSelected)
                {
                    usedCiphers.Items.Remove(listBoxItem);
                    availableCiphers.Items.Add(listBoxItem);

                    UpdateCipherKeyTextBoxes();

                    break;
                }
            }

        }

        private void LoadFileContents(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                String buttonName = (sender as Button).Name;
                String buttonNamePrefix = buttonName.Substring(0,buttonName.Length - 6);
                (this.FindName(buttonNamePrefix) as TextBox).Text = File.ReadAllText(openFileDialog.FileName);
            }
        }
    }
}
