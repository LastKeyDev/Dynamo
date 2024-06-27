using System.Data;
using System.IO;
using System.Windows;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.Win32;


namespace Dynamo
{
    /// <summary>
    /// hacer la conversion y viceversa
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataTable dt = new DataTable();
        private int column;
        public FormatConverter converter = new FormatConverter();
        public MainWindow()
        {
            InitializeComponent();
        }

     
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dt = converter.PopulateGrid();
            dtExcel.ItemsSource = dt.DefaultView;

            
        }

        private void convertFight_Click(object sender, RoutedEventArgs e)
        {
            if ((column == 0 || column == null) || dtExcel == null) {
                MessageBox.Show("Necesita cargar un excel o una columna");
            }
            else 
            {
               txtJson.Text =  converter.JsonConverter(dt, column);

            };

        }

        private void openColumn(object sender, RoutedEventArgs e)
        {
            ColumnNumber columnNumber = new ColumnNumber();

            if (columnNumber.ShowDialog() == false)
            {
                column = columnNumber.ColumnNum;
            };
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtJson.Text))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Archivos JSON (*.json)|*.json|Todos los archivos (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        // Obtener la ruta del archivo seleccionado
                        string filePath = saveFileDialog.FileName;

                        // Generar el contenido del archivo
                        string fileContent = txtJson.Text;

                        // Escribir el contenido en el archivo
                        File.WriteAllText(filePath, fileContent);

                        Console.WriteLine("Archivo guardado exitosamente.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al guardar el archivo: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Primero se debe generar un JSON", "", MessageBoxButton.OK);
            }
        }
        


    }
}