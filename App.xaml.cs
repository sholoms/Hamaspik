using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Hamaspik
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {      

        App()
        {
            ResourceDictionary myResources = new ResourceDictionary();
            myResources.Add("users", new ObservableCollection<User>());
            myResources.Add("groups", new ObservableCollection<Group>());
            this.Resources = myResources;
        }

    }
}
