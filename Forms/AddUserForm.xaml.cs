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
using System.Windows.Shapes;

namespace Hamaspik.Forms
{
    /// <summary>
    /// Interaction logic for AddUserForm.xaml
    /// </summary>
    public partial class AddUserForm : Window
    {
        public AddUserForm()
        { 
             InitializeComponent();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            FName.SelectAll();
            FName.Focus();
        }

        public User Answer
        {
            get
            {
                return new User
                {
                    FName = FName.Text,
                    Lname = LName.Text,
                    NickName = NickName.Text,
                    email = email.Text,
                    ADD1 = Add1.Text,
                    City = City.Text,
                    State = State.Text,
                    Zip = Zip.Text,
                };
            }
        }
    }
}
