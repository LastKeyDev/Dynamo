using System.Data;
using System.Windows;


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
    }
}