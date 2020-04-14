using System;
using System.Windows;
using System.Windows.Controls;
using ClassicCiphers.Ciphers;
using System.IO;
using ClassicCiphers.Exceptions;

namespace ClassicCiphers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        static int AvailableCipherCount = 4;
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

        /*
         * Instantiates different ciphers based on the stack of used ciphers.
         */
        private bool LoadCiphers()
        {
            int i = 0;
            try
            {
                foreach (ListBoxItem currentItem in usedCiphers.Items)
                {
                    if (textBoxes[i].Text.Equals(""))
                    {
                        errorsTextBox.Text = "You must introduce a key for each cipher!";
                        return false;
                    }
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
                        case "Playfair":
                            ciphers[i] = new PlayfairCipher();
                            break;
                    }
                    ciphers[i].SetKey(textBoxes[i].Text);
                    textBoxes[i].Text = ciphers[i].GetKeyValue();
                    i++;
                }
            }
            catch (InvalidKeyFormatException ex)
            {
                errorsTextBox.Text = ex.Message;
                return false;
            }

            return true;
        }

        /*
         * Verifies the validity of the used cipher stack. 
         * If playfair is anywhere other than the top, the stack is invalid.
         */
        private bool VerifyCipherStackValidity()
        {
            if (usedCiphers.Items.Count <= 1)
                return true;
            for (int i = 0; i < usedCiphers.Items.Count; i++)
                if (i > 0 && ciphers[i] is PlayfairCipher)
                {
                    errorsTextBox.Text = "Playfair can only be used as the top cipher on the stack!";
                    return false;
                }
            return true;
        }

        /*
         * Verifies the validity of the input text.
         * Since the ciphers shouldn't generate text that is invalid when going through the stack, 
         * only verifying the validity for the top or the bottom cipher is important.
         */
        private bool CheckInputTextValidity(String text, String mode)
        {
            if (inputTextBox.Text.Equals(""))
            {
                errorsTextBox.Text = "You have not introduced any text in the clear text box!";
                return false;
            }

            for (int i = 0; i < usedCiphers.Items.Count; i++)
            {
                if (!ciphers[i].CheckInputTextValidity(text, mode))
                {
                    errorsTextBox.Text = "The input text is invalid for the top cipher in the stack!";
                    if (ciphers[0] is PlayfairCipher)
                        errorsTextBox.Text += " For playfair, the input needs to have an even number of characters";
                    else
                        errorsTextBox.Text += " The input needs every character to be a part of the polybius square.";
                    return false;
                }
            }
            return true;
        }

        /*
         * Makes all the inputs be lower case.
         */
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

        /*
         * Normalizes the input, checks if the cipher stack is instantiated properly, if the input is valid and if the cipher stack is valid.
         * After that from the top of the stack downwards the input text is encrypted by feeding each cipher the previous' cipher's output.
         */ 
        private void EncryptText(object sender, RoutedEventArgs e)
        {
            String encryptedText = inputTextBox.Text;
            NormalizeInput();
            if (!LoadCiphers() ||
                !CheckInputTextValidity(encryptedText, "encrypt") ||
                !VerifyCipherStackValidity())
                return;

            for (int i = 0; i < usedCiphers.Items.Count; i++)
                encryptedText = ciphers[i].Encrypt(encryptedText);
            errorsTextBox.Text = "";
            outputTextBox.Text = encryptedText;
        }

        /*
         * From the bottom of the stack upwards the input text is decrypted by feeding each cipher the previous' cipher's output.
         */
        private void DecryptText(object sender, RoutedEventArgs e)
        {
            String decryptedText = inputTextBox.Text;
            NormalizeInput();
            if (!LoadCiphers() ||
                !CheckInputTextValidity(decryptedText, "decrypt") ||
                !VerifyCipherStackValidity())
                return;

            for (int i = usedCiphers.Items.Count - 1; i >= 0; i--)
                decryptedText = ciphers[i].Decrypt(decryptedText);
            errorsTextBox.Text = "";

            outputTextBox.Text = decryptedText;
        }

        /*
         * Looks at the used cipher stack to see how many text boxes it has to make visible for input keys.
         */ 
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
                    case "Playfair":
                        textBoxes[i].Text = PlayfairCipher.DefaultKeyString;
                        break;
                }

                textBoxes[i].Visibility = Visibility.Visible;
                buttons[i].Visibility = Visibility.Visible;
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
                    usedCiphers.Items.Insert(0,listBoxItem);

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
        /*
         * Loads the contents of the file into the text box that correspons to the button clicked.
         */ 
        private void LoadFileContents(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                String buttonName = (sender as Button).Name;
                String buttonNamePrefix = buttonName.Substring(0, buttonName.Length - 6);
                (this.FindName(buttonNamePrefix) as TextBox).Text = File.ReadAllText(openFileDialog.FileName);
            }
        }
    }
}
