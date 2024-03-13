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
    /// Логика взаимодействия для AddSession.xaml
    /// </summary>
    public partial class AddSession : Window
    {

        PCExtended PC;

        public AddSession()
        {
            InitializeComponent();
        }

        public AddSession(PCExtended selectedPC)
        {
            InitializeComponent();
            PC = selectedPC;
            Init();
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void Init()
        {
            List<Session> sessions = Helper.DB.Session.ToList();
            tbSessionNumber.Text = "Талон сессии №" + (sessions[sessions.Count - 1].SessionID + 1);
            tbSessionDate.Text = "Дата оформления сессии: " + DateTime.Now.ToString("dd/MM/yyyy");
            tbPCModel.Text = PC.PC.PCName;
            tbPCNumber.Text = PC.PC.PCNumber.ToString();
            foreach (Program program in PC.PC.Program.ToList()) 
            {
                tbProgramList.Text += program.ProgramName + "\n";
            }            
        }

        private void bAddSession_Click(object sender, RoutedEventArgs e)
        {
            DateTime SessionStartDate;
            DateTime SessionStartTime;
            int SessionLength;

            try
            {
                SessionStartDate = DateTime.Parse(dpSessionStartDate.Text);
                SessionStartTime = DateTime.Parse(tbSessionStartTime.Text);
                SessionLength = int.Parse(tbSessionLength.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Данные не корректны", "Ошибка", MessageBoxButton.OK);
                return;
            }

            DateTime SessionStartDateTime = SessionStartDate.Date.Add(SessionStartTime.TimeOfDay);
            DateTime SessionEndDateTime = SessionStartDateTime.AddHours(SessionLength);

            Session newSession = new Session();
            newSession.SessionUser = Helper.User.UserID;
            newSession.SessionPC = PC.PC.PCNumber;
            newSession.SessionStartDateTime = SessionStartDateTime;
            newSession.SessionEndDateTime = SessionEndDateTime;
            newSession.SessionStatus = 1;

            Helper.DB.Session.Add(newSession);
            Helper.DB.SaveChanges();

            MessageBox.Show("Сессия успешно зарегистрирована", "Успех", MessageBoxButton.OK);
        }

        private void tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            int SessionLength;

            try
            {
                SessionLength = int.Parse(tbSessionLength.Text);
            }
            catch (Exception)
            {
                tbSessionCost.Text = "Стоимость сессии: -";
                tbSessionDiscount.Text = "Скидка: -";
                return;
            }

            tbSessionCost.Text = "Стоимость сессии: " + (Math.Round(PC.PCDiscountCost * SessionLength, 2)).ToString();
            tbSessionDiscount.Text = "Скидка: " + (Math.Round((PC.PCResultCost - PC.PCDiscountCost) * SessionLength , 2)).ToString();
        }
    }
}
