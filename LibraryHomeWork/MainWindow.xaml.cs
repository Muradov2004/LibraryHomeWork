using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LibraryHomeWork;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    ObservableCollection<string> Authors = new();

    string connectionString = @"Data Source=DESKTOP-NGE2A6O\SQLEXPRESS03;Initial Catalog=Library;Integrated Security=True;";

    private void GetDataFromDataBase()
    {
        Authors.Clear();
        using (SqlConnection connection = new(connectionString))
        {
            SqlDataReader reader = null;

            SqlCommand cmd = new(@"SELECT * FROM Authors", connection);
            connection.Open();
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string author = reader["Id"].ToString() + "." + reader["FirstName"].ToString() + " " + reader["LastName"].ToString();
                Authors.Add(author);
            }
        }
    }

    private void AddDataToDataBase()
    {
        using (SqlConnection connection = new(connectionString))
        {
            string insertQuery = @$"INSERT INTO Authors ([Id], [FirstName], [LastName]) VALUES({Authors.Count + 1}, '{FirstNametxtBox.Text}', '{LastNametxtBox.Text}')";

            SqlCommand insertCommand = new(insertQuery, connection);

            connection.Open();
            insertCommand.ExecuteNonQuery();
        }
    }

    public MainWindow()
    {
        InitializeComponent();

        GetDataFromDataBase();

        lstBox.ItemsSource = Authors;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (LastNametxtBox.Text != string.Empty && FirstNametxtBox.Text != string.Empty)
        {
            AddDataToDataBase();
            GetDataFromDataBase();
            FirstNametxtBox.Clear();
            LastNametxtBox.Clear();
        }
        else
        {
            if (FirstNametxtBox.Text == string.Empty) FirstNametxtBox.Background = Brushes.Red;
            if (LastNametxtBox.Text == string.Empty) LastNametxtBox.Background = Brushes.Red;
        }
    }

    private void TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) => (sender as TextBox).Background = Brushes.White;
}
