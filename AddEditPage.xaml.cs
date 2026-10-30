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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Service _currentService = new Service();
        public AddEditPage(Service SelectedService)
        {
            InitializeComponent();
            if (SelectedService != null)
                _currentService = SelectedService;

            DataContext = _currentService;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentService.Title))
                errors.AppendLine("Укажите название услуг");

            if (_currentService.Cost == 0)
                errors.AppendLine("Укажите стоимость услуги");

            if (string.IsNullOrWhiteSpace(_currentService.Discount.ToString()))
                errors.AppendLine("Укажите скидку");

            if (_currentService.DurationInSeconds == 0)
                errors.AppendLine("Укажите длительность услуги");
            if (_currentService.DurationInSeconds > 240)
                errors.AppendLine("Длительность услуги не может быть больше 240 минут");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return; 
            }
            var allService = BiglovAutoServiceEntities.GetContext().Service.ToList();
            allService = allService.Where(p => p.Title == _currentService.Title && p.ID != _currentService.ID).ToList();
            if(allService.Count == 0)
            {
                if (_currentService.ID == 0)
                {
                    BiglovAutoServiceEntities.GetContext().Service.Add(_currentService);
                }
                try
                {
                    BiglovAutoServiceEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
            {
                MessageBox.Show("Уже существует такая услуга");
            }
        }
    }
}
