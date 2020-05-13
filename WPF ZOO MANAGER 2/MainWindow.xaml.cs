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
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            
        }

        private void ShowAnimals()
        {
            try
            {
                string query = "select * from Animal a inner join ZooAnimal " + "za on a.Id = za.AnimalId where za.ZooId = @zooId";

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
            catch (Exception ex)
            {

               // MessageBox.Show(ex.ToString());
            }

        }

        //show selected animal
        private void zooList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ShowAnimals();
            ShowSelectedValue();
           
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


      
        private void Delete_Zoo_Btn(object sender, RoutedEventArgs e)
        {


            try
            {
                string query = "delete from Zoo where Id = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", zooList.SelectedValue);
                sqlCommand.ExecuteScalar();
               
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
            }

           
        }

        private void Add_Zoo_Btn(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into Zoo values (@Location)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Location", myTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
                myTextBox.Clear();
            }
        }

        private void Add_Animal_To_Zoo_Btn(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into ZooAnimal values (@ZooId ,@AnimalId)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", zooList.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@AnimalId", allAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAnimals();
                myTextBox.Clear();
            }
        }

        //REMOVE ANIMAL FROM  ZOOOOOOO
        private void RemoveAnimalFromZoo(object sender, RoutedEventArgs e)
        {
            
            try
            {
                string query = "delete from ZooAnimal where Id = @AnimalId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                //sqlCommand.Parameters.AddWithValue("@ZooId", zooList.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@AnimalId", animalsList.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAnimals();
                myTextBox.Clear();
            }
        }

        //Delete animal completely 

        private void DeleteAnimal(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "delete from Animal where Id = @AnimalId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", allAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAllAnimals();
                ShowAnimals();
            }


        }

        private void AddToAnimal(object sender, RoutedEventArgs e)
        {

            try
            {
                string query = "insert into Animal values (@Name)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Name", myTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAllAnimals();
                myTextBox.Clear();
            }

        }

        //show slected values

        private void ShowSelectedValue()
        {
            try
            {
                string query = "select Location from Zoo where Id = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", zooList.SelectedValue);

                    DataTable zooTable = new DataTable();

                    sqlDataAdapter.Fill(zooTable);

                    myTextBox.Text = zooTable.Rows[0]["Location"].ToString();
                    

                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
        }

        private void ShowSelectedAnimal()
        {
          
                try
                {
                    string query = "select Name from Animal where Id = @AnimalId";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                    using (sqlDataAdapter)
                    {
                        sqlCommand.Parameters.AddWithValue("@AnimalId", allAnimals.SelectedValue);

                        DataTable animalTable = new DataTable();

                        sqlDataAdapter.Fill(animalTable);

                        myTextBox.Text = animalTable.Rows[0]["Name"].ToString();


                    }
                }
                catch (Exception e)
                {

                    MessageBox.Show(e.ToString());
                }
            }

        private void allAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowSelectedAnimal();
        }

        //update zoo
        private void Update_Zoo_Btn(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "Update Zoo Set Location = @Location where Id = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", zooList.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Location", myTextBox.Text);
                sqlCommand.ExecuteScalar();

                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
                myTextBox.Clear();
                
            }
        }


        //update animal
        private void Update_Animal_Btn(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "Update Animal Set Name = @Name where Id = @AnimalId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", allAnimals.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Name", myTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAllAnimals();
                myTextBox.Clear();
                
            }
        }


    }
}
