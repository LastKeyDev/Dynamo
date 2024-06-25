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

namespace Dynamo
{
    /// <summary>
    /// Interaction logic for ColumnNumber.xaml
    /// </summary>
    public partial class ColumnNumber : Window
    {
        private int _columnNumber;
        public ColumnNumber()
        {
            InitializeComponent();
        }

        public int ColumnNum
        {
            get { return _columnNumber; }
        }

        private void setNumberColumn()
        {
        
            try
            {
                if (cantColumn.Text.Trim().Length > 0)
                {
                    _columnNumber = Convert.ToInt32(cantColumn.Text.Trim());
                    
                }

                else
                {
                    MessageBox.Show("Necesita ingresar un valor, preferentemente un numero, no sea Enano Comunista");

                }
            }
            catch (Exception ex)
            {


                if (MessageBox.Show("Fue advertido... Jugó con las fuerzas del cielo, le caerá todo el poder", "Fuerzas del cielo incomming", MessageBoxButton.OK, MessageBoxImage.Warning, (MessageBoxResult)MessageBoxButton.OK, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.OK)
                {

                }

            }

            if (_columnNumber != 0) 
            {
                Window.GetWindow(this).Close();     
            }


        
        }

        private void insert_Click(object sender, RoutedEventArgs e)
        {
            setNumberColumn();
        }
    }
}
