using Hamaspik.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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


namespace Hamaspik
{
    public partial class MainWindow : Window
    {
        ObservableCollection<Group> groups;
        ObservableCollection<User> users;

        LinqToSQLClassDataContext db = new LinqToSQLClassDataContext(Properties.Settings.Default.HamaspikDBConnectionString);

        public MainWindow()
        {
            InitializeComponent();

            // Try to fetch collections from application resources; create and register fallback if missing.
            users = Application.Current.FindResource("users") as ObservableCollection<User>;
            groups = Application.Current.FindResource("groups") as ObservableCollection<Group>;

            string filePath = "people.json"; // Path to your JSON file
            string jsonContent = File.ReadAllText(filePath);
            var people = (JsonConvert.DeserializeObject<List<User>>(jsonContent));
            
            if (people != null && people.Count > 0)
            {
                db.Users.InsertAllOnSubmit(people);
                db.SubmitChanges();
            }

            File.WriteAllText(filePath, string.Empty);

            SyncGroups();
            db.Users.ToList().ForEach(u => users.Add(u));
            

            lbUsers.ItemsSource = users;
            lbGroups.ItemsSource = groups;
        }

        private void btnAddGroup_Click(object sender, RoutedEventArgs e)
        {
            AddGroupForm inputDialog = new AddGroupForm("Please enter Group name:");
            if (inputDialog.ShowDialog() == true)
                AddGroup(inputDialog.Answer);
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            AddUserForm inputDialog = new AddUserForm();
            if (inputDialog.ShowDialog() == true)
            {
                var newUser = (inputDialog.Answer);
                db.Users.InsertOnSubmit(newUser);
                db.SubmitChanges();
                users.Clear();
                db.Users.ToList().ForEach(u => users.Add(u));
            }
        }

        private void btnEditGroup_Click(object sender, RoutedEventArgs e)
        {
            if (lbGroups.SelectedItem != null)
            {
                var g = (lbGroups.SelectedItem as Group);
                EditGroupForm inputDialog = new EditGroupForm(g.GroupId, db);
                inputDialog.ShowDialog();
            }
        }

        private void btnDeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            if (lbGroups.SelectedItem != null)
            {
                db.Groups.DeleteOnSubmit((Group)lbGroups.SelectedItem);

                db.SubmitChanges();

                SyncGroups();

            }
        }

        private void AddGroup(string name)
        {
            db.Groups.InsertOnSubmit(new Group() { Name = name });
            db.SubmitChanges();
            SyncGroups();
        }

        private void SyncGroups()
        {
            groups.Clear();
            db.Groups.ToList().ForEach(g => groups.Add(g));

        }
    }

    
}
