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
            myItem.IsSelected = false;

            availableCiphers.Items.Remove(myItem);
            myItem.Selected -= new RoutedEventHandler(SelectAvailableCipher);
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

        }

        private void RemoveUsedCipher(object sender, RoutedEventArgs e)
        {
            var myItem = (ListBoxItem)sender;
            myItem.IsSelected = false;

            usedCiphers.Items.Remove(myItem);
            myItem.Selected -= new RoutedEventHandler(RemoveUsedCipher);
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
        }
        private bool LoadCiphers()
        {
            try
            {
                if (inputTextBox.Text.Equals(""))
                    throw new FormatException("You have not introduced any text in the clear text box!");

                for (int i = 0; i < usedCiphers.Items.Count; i++)
                {
                    switch (textBoxes[i].Name)
                    {
                        case "CaesarKeyTextBox":
                            if (CaesarKeyTextBox.Text.Equals(""))
                                throw new FormatException("You must introduce a key for the caesar cipher!");
                            ciphers[i] = new CaesarCipher();
                            ciphers[i].SetKey(CaesarKeyTextBox.Text);
                            CaesarKeyTextBox.Text = ciphers[i].Key.StringValue;
                            break;
                    }
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

            for (int i = 0; i < usedCiphers.Items.Count; i++)
                outputTextBox.Text = ciphers[i].Encrypt(inputTextBox.Text);
        }

        private void DecryptText(object sender, RoutedEventArgs e)
        {
            if (!LoadCiphers())
                return;

            for (int i = usedCiphers.Items.Count - 1; i >= 0; i--)
                outputTextBox.Text = ciphers[i].Decrypt(inputTextBox.Text);
        }

    }
}
