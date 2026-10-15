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

namespace BiglovAutoservice
{
    /// <summary>
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {

        private void UpdateServices()
        {
            var currentServices = BiglovAutoserviceEntities.GetContext().Service.ToList();

            if (ComboType.SelectedIndex == 0)
            {
                currentServices = currentServices.Where(s => (s.Discount >= 0 && s.Discount <= 100)).ToList();
            }
            if (ComboType.SelectedIndex == 1)
            {
                currentServices = currentServices.Where(s => (s.Discount >= 0 && s.Discount < 5)).ToList();
            }
            if (ComboType.SelectedIndex == 2)
            {
                currentServices = currentServices.Where(s => (s.Discount >= 5 && s.Discount < 15)).ToList();
            }
            if (ComboType.SelectedIndex == 3)
            {
                currentServices = currentServices.Where(s => (s.Discount >= 15 && s.Discount < 30)).ToList();
            }
            if (ComboType.SelectedIndex == 4)
            {
                currentServices = currentServices.Where(s => (s.Discount >= 30 && s.Discount < 70)).ToList();
            }
            if (ComboType.SelectedIndex == 5)
            {
                currentServices = currentServices.Where(s => (s.Discount >= 70 && s.Discount <= 100)).ToList();
            }

            currentServices = currentServices.Where(s => s.Title.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            if (RbuttonUP.IsChecked.Value)
            {
                currentServices = currentServices.OrderBy(i => i.Cost).ToList();
            }

            if (RButtonDown.IsChecked.Value)
            {
                currentServices = currentServices.OrderByDescending(i => i.Cost).ToList();
            }
            ServiceListView.ItemsSource = currentServices;

        }

        public ServicePage()
        {
            InitializeComponent();
            var currentServices = BiglovAutoserviceEntities.GetContext().Service.ToList();
            ServiceListView.ItemsSource = currentServices;

            ComboType.SelectedIndex = 0;

            UpdateServices();
        }

        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateServices();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }

        private void RbuttonUP_Checked(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }

        private void RButtonDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }
    }
}
