using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
using System.Windows.Shapes;

namespace Hamaspik.Forms
{
    /// <summary>
    /// Interaction logic for EditGroupForm.xaml
    /// </summary>
    public partial class EditGroupForm : Window
    {
        List<GroupUser> groupUsers;
        ObservableCollection<User> users;
        int groupID;
        LinqToSQLClassDataContext dbContext;

        public EditGroupForm(int ID, LinqToSQLClassDataContext db)
        { 
            InitializeComponent();
            dbContext = db;
            groupID = ID;
            var gu = dbContext.GroupUsers.ToList<GroupUser>();
            groupUsers = gu.Where(g => g.GroupId == ID).ToList();

            users = Application.Current.FindResource("users") as ObservableCollection<User>;
            LoadUsers();
        }

        private void LoadUsers()
        {
            foreach (var u in users)
            {
                var isMember = groupUsers != null && groupUsers.Any(gu => gu.UserId == u.UserId);

                CheckBox cb = new CheckBox
                {
                    Content = $"{u.FName} {u.Lname}",
                    IsChecked = isMember,
                    Tag = u.UserId
                };

                cb.Checked += Cb_Checked;
                cb.Unchecked += Cb_Unchecked;

                spUsers.Children.Add(cb);
            }
        }

        private void Cb_Checked(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            if (cb?.Tag is int userId)
            {
                dbContext.GroupUsers.InsertOnSubmit(new GroupUser
                {
                    GroupId = groupID,
                    UserId = userId
                });
            }
        }
        private void Cb_Unchecked(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            if (cb?.Tag is int userId)
            {
                var groupUser = groupUsers.FirstOrDefault(gu => gu.UserId == userId && gu.GroupId == groupID);
                if (groupUser != null)
                {
                    dbContext.GroupUsers.DeleteOnSubmit(groupUser);

                }
            }

        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtName.Focus();
        }

        private void btnDialogSave_Click(object sender, RoutedEventArgs e)
        {
            dbContext.SubmitChanges();
            DialogResult = true;

        }

        private void btnDialogCancel_Click(object sender, RoutedEventArgs e)
        {
            var changes = dbContext.GetChangeSet();
            changes.Inserts.ToList().ForEach(i => dbContext.GetTable<GroupUser>().DeleteOnSubmit((GroupUser)i));
            changes.Deletes.ToList().ForEach(d => dbContext.GetTable<GroupUser>().InsertOnSubmit((GroupUser)d));

            DialogResult = true; 
        }
    }
}
