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
using System.Numerics;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;

namespace Blum_Blum_Shub_CSPRBG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            textBoxPInput.Text = "11";
            textBoxQInput.Text = "23";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BigInteger p, q;
            double size;

            String fileName = "output_" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".txt";
            String filePath = textBoxFolderInput.Text + "\\" + fileName;
            BinaryWriter binaryWriter;


            try
            {
                size = Double.Parse(textBoxFileSize.Text);
                binaryWriter = new BinaryWriter(File.Open(filePath, FileMode.Create));
                p = BigInteger.Parse(textBoxPInput.Text);
                q = BigInteger.Parse(textBoxPInput.Text);
            }
            catch (Exception ex)
            {
                if (ex is FormatException ||
                    ex is NotSupportedException)
                {
                    textBoxErrors.Text = "Incorrect input introduced in one of the boxes.";
                    return;
                }

                throw;
            }

            textBoxErrors.Text = "";
            BBSGenerator.SetParameters(p, q);
            String randomBitString = BBSGenerator.GetRandomBitString((long)(size * 1000000));
            binaryWriter.Write(randomBitString);
            binaryWriter.Close();
            textBoxErrors.Text = randomBitString.Length.ToString();
        }
        
        private void checkBoxParameters_Click(object sender, RoutedEventArgs e)
        {
            textBoxQInput.IsEnabled = textBoxPInput.IsEnabled = checkBoxParameters.IsChecked == true ? false : true;
        }

        private void SelectFolder(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;


            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                textBoxFolderInput.Text = dialog.FileName;
            }

        }
    }
}
