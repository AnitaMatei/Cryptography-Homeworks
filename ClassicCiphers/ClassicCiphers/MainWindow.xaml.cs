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

namespace ClassicCiphers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        TextBox[] textBoxes = new TextBox[5];
        GenericCipher[] ciphers = new GenericCipher[5];
        String[] usedCiphersNames = { "Caesar", "OTP", "Nihilist", "Bifid", "Trifid" };

        public MainWindow()
        {
            InitializeComponent();

            for (int i = 0; i < usedCiphersNames.Length; i++)
            {
                textBoxes[i] = this.FindName(usedCiphersNames[i] + "KeyTextBox") as TextBox;
                textBoxes[i].Visibility = Visibility.Hidden;
                textBoxes[i].PreviewMouseDown += new MouseButtonEventHandler(EmptyTextBox);
                textBoxes[i].Tag = "unpressed";
            }
        }

        private void SwapText(object sender, RoutedEventArgs e)
        {
            String tempString = inputTextBox.Text;
            inputTextBox.Text = outputTextBox.Text;
            outputTextBox.Text = tempString;
        }

        private void EmptyTextBox(object sender, MouseButtonEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Tag.Equals("unpressed"))
            {
                textBox.Text = "";
                textBox.Tag = "pressed";
            }
        }

        private void UpdateCipherKeyTextBoxes()
        {
            for (int i = 0; i < usedCiphers.Items.Count; i++)
            {
                if (textBoxes[i].Visibility == Visibility.Hidden)
                    break;
                ListBoxItem currentItem = usedCiphers.Items.GetItemAt(i) as ListBoxItem;

                textBoxes[i].Text = currentItem.Content + " Key";
                textBoxes[i].Name = currentItem.Content + "Key" + "TextBox";
            }
        }

        private void SelectAvailableCipher(object sender, RoutedEventArgs e)
        {
            var myItem = sender as ListBoxItem;
            myItem.Selected -= new RoutedEventHandler(SelectAvailableCipher);

            availableCiphers.Items.Remove(myItem);
            myItem.Selected += new RoutedEventHandler(RemoveUsedCipher);
            usedCiphers.Items.Add(myItem);

            for (int i = 0; i < usedCiphersNames.Length; i++)
            {
                if (!textBoxes[i].IsVisible)
                {
                    textBoxes[i].Visibility = Visibility.Visible;
                    textBoxes[i].Text = myItem.Content + " Key";
                    break;
                }
            }

            UpdateCipherKeyTextBoxes();
            myItem.IsSelected = false;

        }

        private void RemoveUsedCipher(object sender, RoutedEventArgs e)
        {
            var myItem = (ListBoxItem)sender;
            myItem.Selected -= new RoutedEventHandler(RemoveUsedCipher);

            usedCiphers.Items.Remove(myItem);
            myItem.Selected += new RoutedEventHandler(SelectAvailableCipher);
            availableCiphers.Items.Add(myItem);


            for (int i = usedCiphersNames.Length - 1; i >= 0; i--)
            {
                if (textBoxes[i].IsVisible)
                {
                    textBoxes[i].Visibility = Visibility.Hidden;
                    textBoxes[i].Tag = "unpressed";
                    break;
                }
            }

            UpdateCipherKeyTextBoxes();
            myItem.IsSelected = false;
        }
        private bool LoadCiphers()
        {
            try
            {
                if (inputTextBox.Text.Equals(""))
                    throw new FormatException("You have not introduced any text in the clear text box!");

                for (int i = 0; i < usedCiphers.Items.Count; i++)
                {
                    if (textBoxes[i].Text.Equals(""))
                        throw new FormatException("You must introduce a key for each ciphers!");
                    switch (textBoxes[i].Name)
                    {
                        case "CaesarKeyTextBox":
                            ciphers[i] = new CaesarCipher();
                            break;
                        case "NihilistKeyTextBox":
                            ciphers[i] = new NihilistCipher();
                            break;
                    }
                    ciphers[i].SetKey(textBoxes[i].Text);
                    textBoxes[i].Text = ciphers[i].GetKeyValue();

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

        private void EncryptText(object sender, RoutedEventArgs e)
        {
            if (!LoadCiphers())
                return;
            String encryptedText = inputTextBox.Text;
            for (int i = 0; i < usedCiphers.Items.Count; i++)
                encryptedText = ciphers[i].Encrypt(encryptedText);
            outputTextBox.Text = encryptedText;
        }

        private void DecryptText(object sender, RoutedEventArgs e)
        {
            if (!LoadCiphers())
                return;
            String decryptedText = inputTextBox.Text;
            for (int i = usedCiphers.Items.Count - 1; i >= 0; i--)
                decryptedText = ciphers[i].Decrypt(decryptedText);
            outputTextBox.Text = decryptedText;
        }

    }
}
