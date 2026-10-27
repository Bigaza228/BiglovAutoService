using System.Windows;

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
    /// Логика взаимодействия для SingUpPage.xaml
    /// </summary>
    public partial class SingUpPage : Page
    {
        private Service _currentService = new Service();
        public SingUpPage(Service SelectedService)
        {
            InitializeComponent();
            if(SelectedService != null)
                this._currentService = SelectedService;
            DataContext = _currentService;

            var _currentClient = BiglovAutoServiceEntities.GetContext().Client.ToList();
            ComboClient.ItemsSource = _currentClient;
        }


        private ClientService _currentClientSerevice = new ClientService();

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (ComboClient.SelectedItem == null)
                errors.AppendLine("Укажите ФИО клиента");
            if (StartDate.Text == "")
                errors.AppendLine("Укажите дату услуги");
            if (TBStart.Text == "")
                errors.AppendLine("Укажите время начала услуги");
            if(errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            _currentClientSerevice.ClientID = ComboClient.SelectedIndex + 1;
            _currentClientSerevice.ServiceID = _currentService.ID;
            _currentClientSerevice.StartTime = Convert.ToDateTime(StartDate.Text + " " + TBStart.Text);
            
            if(_currentClientSerevice.ID == 0)
                BiglovAutoServiceEntities.GetContext().ClientService.Add(_currentClientSerevice);
            try
            {
                BiglovAutoServiceEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            } catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = TBStart.Text;
            if (s.Length < 3 || !s.Contains(':'))
                TBEnd.Text = "";
            else
            {
                string[] start = s.Split(new char[] { ':' });
                int CheckStart1, CheckStart2;
                int.TryParse(start[0], out CheckStart1);
                int.TryParse(start[1], out CheckStart2);
                if (CheckStart1 > 23 || CheckStart2 > 60)
                {
                    MessageBox.Show("Ошибка, некорректное время");
                    TBStart.Text = "";
                    return;
                }
                int startHour = Convert.ToInt32(start[0].ToString()) * 60;
                int startMin = 0;
                if (!int.TryParse(start[1], out startMin)) return;


                int sum = startHour + startMin + _currentService.DurationInSeconds;

                int EndHour = sum / 60;
                int EndMin = sum % 60;
                if(EndHour > 23)
                {
                    EndHour = EndHour - 24;
                }
                string StrEndHour = (EndHour < 10) ? "0" + EndHour.ToString() : EndHour.ToString();
                string StrEndMin = (EndMin < 10) ? "0" + EndMin.ToString() : EndMin.ToString();
                s = StrEndHour + ":" + StrEndMin;
                TBEnd.Text = s;
            }
        }
    }
}
