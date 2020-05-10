using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

namespace WPF_ZOO_MANAGER_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;
        public MainWindow() 
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["WPF_ZOO_MANAGER_2.Properties.Settings.diyDBConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);

            ShowZoos();
            ShowAllAnimals();
        }

        private void ShowZoos()
        {
            try
            {
                string query = "select * from Zoo";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable zooTable = new DataTable();

                    sqlDataAdapter.Fill(zooTable);

                    zooList.DisplayMemberPath = "Location";
                    zooList.SelectedValuePath = "Id";
                    zooList.ItemsSource = zooTable.DefaultView;

                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
            
        }

        private void ShowAnimals()
        {
            try
            {
                string query = "select * from Animal a inner join ZooAnimal za on a.Id = za.AnimalId where za.ZooId = @zooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@zooId", zooList.SelectedValue);

                    DataTable animalTable = new DataTable();

                    sqlDataAdapter.Fill(animalTable);

                    animalsList.DisplayMemberPath = "Name";
                    animalsList.SelectedValuePath = "Id";
                    animalsList.ItemsSource = animalTable.DefaultView;

                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }

        private void zooList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ShowAnimals();
        }


        private void ShowAllAnimals()
        {
            try
            {
                string query = "select * from Animal";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable allAnimalsTable = new DataTable();

                    sqlDataAdapter.Fill(allAnimalsTable);

                    allAnimals.DisplayMemberPath = "Name";
                    allAnimals.SelectedValuePath = "Id";
                    allAnimals.ItemsSource = allAnimalsTable.DefaultView;

                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }

        private void allAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ShowAllAnimals();

        }
    }
}
