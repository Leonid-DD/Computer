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
using ООО_Компутир.Classes;
using ООО_Компутир.Model;

namespace ООО_Компутир.View
{
    /// <summary>
    /// Логика взаимодействия для PCRentHistory.xaml
    /// </summary>
    public partial class PCRentHistory : Window
    {
        public PCRentHistory()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Session> sessions = Helper.DB.Session.ToList();
            List<Classes.SessionExtended> sessionsExt = new List<SessionExtended>();

            foreach (Session session in sessions)
            {
                if (session.User.Equals(Helper.User))
                {
                    SessionExtended sessionExt = new SessionExtended();
                    sessionExt.Session = session;
                    sessionsExt.Add(sessionExt);
                }
            }

            lbPCRent.ItemsSource = sessionsExt;
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
