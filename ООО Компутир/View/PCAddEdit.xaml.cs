using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
using System.Windows.Shapes;
using ООО_Компутир.Classes;
using ООО_Компутир.Model;

namespace ООО_Компутир.View
{
    /// <summary>
    /// Логика взаимодействия для PCAddEdit.xaml
    /// </summary>
    public partial class PCAddEdit : Window
    {
        PCExtended PCExtended;

        string PCImagePath;

        public PCAddEdit()
        {
            InitializeComponent();

            tbTitle.Text = "Добавление";
            Init();

            bEditPC.Visibility = Visibility.Collapsed;
        }

        public PCAddEdit(PCExtended selectedPC)
        {
            InitializeComponent();

            tbTitle.Text = "Редактирование";
            Init();

            bAddPC.Visibility = Visibility.Collapsed;
            bDeletePC.Visibility = Visibility.Visible;

            tbPCNumber.Text = selectedPC.PC.PCNumber.ToString();
            tbPCName.Text = selectedPC.PC.PCName.ToString();
            tbPCCost.Text = selectedPC.PC.PCCost.ToString();
            tbPCDiscount.Text = selectedPC.PC.PCDiscount.ToString();
            cbPCKeyboard.SelectedValue = selectedPC.PC.PCKeyboardID;
            cbPCMouse.SelectedValue = selectedPC.PC.PCMouseID;
            cbPCMonitor.SelectedValue = selectedPC.PC.PCMonitorID;
            PCImagePath = selectedPC.PC.PCImagePath;
            tbPCDescription.Text = selectedPC.PC.PCDescription.ToString();

            PCExtended = selectedPC;

            List<Program> selectedPrograms = selectedPC.PC.Program.ToList();

            foreach (Program program in selectedPrograms)
            {
                lbPCProgram.SelectedItem = program;
            }
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void Init()
        {
            List<Program> programs = Helper.DB.Program.ToList();
            lbPCProgram.Items.Clear();
            lbPCProgram.DisplayMemberPath = "ProgramName";
            lbPCProgram.SelectedValuePath = "ProgramID";
            lbPCProgram.SelectionMode = SelectionMode.Multiple;
            lbPCProgram.ItemsSource = programs;

            List<Model.Keyboard> keyboards = Helper.DB.Keyboard.ToList();
            cbPCKeyboard.Items.Clear();
            cbPCKeyboard.DisplayMemberPath = "KeyboardName";
            cbPCKeyboard.SelectedValuePath = "KeyboardID";
            cbPCKeyboard.ItemsSource = keyboards;

            List<Model.Mouse> mice = Helper.DB.Mouse.ToList();
            cbPCMouse.Items.Clear();
            cbPCMouse.DisplayMemberPath = "MouseName";
            cbPCMouse.SelectedValuePath = "MouseID";
            cbPCMouse.ItemsSource = mice;

            List<Model.Monitor> monitors = Helper.DB.Monitor.ToList();
            cbPCMonitor.Items.Clear();
            cbPCMonitor.DisplayMemberPath = "MonitorName";
            cbPCMonitor.SelectedValuePath = "MonitorID";
            cbPCMonitor.ItemsSource = monitors;
        }

        public void AddOrEdit()
        {
            int PCNumber;
            string PCName;
            int PCRentCost;
            int PCDiscount;
            int PCKeyboardID;
            int PCMouseID;
            int PCMonitorID;
            string PCDescription;

            List<int> PCPrograms = new List<int>();

            try
            {
                PCNumber = int.Parse(tbPCNumber.Text);
                PCName = tbPCName.Text;
                PCRentCost = int.Parse(tbPCCost.Text);
                PCDiscount = int.Parse(tbPCDiscount.Text);
                PCKeyboardID = (int)cbPCKeyboard.SelectedValue;
                PCMouseID = (int)cbPCMouse.SelectedValue;
                PCMonitorID = (int)cbPCMonitor.SelectedValue;
                PCDescription = tbPCDescription.Text;

                foreach (Program program in lbPCProgram.SelectedItems)
                {
                    PCPrograms.Add(program.ProgramID);
                }

                List<PC> PCs = Helper.DB.PC.ToList();
                foreach (PC pc in PCs)
                {
                    if (pc.PCNumber == PCNumber)
                    {
                        throw new Exception();
                    }
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Введены некорректные или неполные данные", "Ошибка", MessageBoxButton.OK);
                return;
            }

            try
            {
                //Создание и заполнение нового PC
                PC newPC = new PC();
                if (PCExtended != null) newPC = PCExtended.PC;
                newPC.PCNumber = PCNumber;
                newPC.PCName = PCName;
                newPC.PCCost = PCRentCost;
                newPC.PCDiscount = PCDiscount;
                newPC.PCKeyboardID = PCKeyboardID;
                newPC.PCMouseID = PCMouseID;
                newPC.PCMonitorID = PCMonitorID;
                if (PCImagePath != null) newPC.PCImagePath = PCImagePath;
                else newPC.PCImagePath = null;
                newPC.PCDescription = PCDescription;

                List<Program> programs = Helper.DB.Program.ToList();
                foreach (var program in PCPrograms)
                {
                    newPC.Program.Add(programs[program - 1]);
                }

                //Добавление в БД
                Helper.DB.PC.AddOrUpdate(newPC);
                Helper.DB.SaveChanges();

                MessageBox.Show("Добавление записи в базу данных успешно", "Успех", MessageBoxButton.OK);
                this.Close();
            }
            catch(Exception) 
            {
                MessageBox.Show("Добавление записи в базу данных прервано", "Ошибка", MessageBoxButton.OK);
                return;
            }
        }

        private void bAddPC_Click(object sender, RoutedEventArgs e)
        {
            AddOrEdit();
        }

        private void bEditPC_Click(object sender, RoutedEventArgs e)
        {
            AddOrEdit();
        }

        private void bPCImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.InitialDirectory = Directory.GetCurrentDirectory() + "\\images\\";
            dialog.FileName = null; // Default file name
            dialog.DefaultExt = ".png"; // Default file extension
            dialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg";

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                string fullPath = dialog.FileName;
                string[] pathSplit = fullPath.Split(new char[] { '\\' });
                PCImagePath = "/" + pathSplit[pathSplit.Length - 1];
            }
        }

        private void bDeletePC_Click(object sender, RoutedEventArgs e)
        {
            List<Session> sessions = PCExtended.PC.Session.ToList();
            foreach(Session session in sessions)
            {
                if (session.SessionStatus == 1 || session.SessionStatus == 2)
                {
                    MessageBox.Show("ПК находится в сессии или в очереди, удаление заблокировано", "Ошибка", MessageBoxButton.OK);
                    return;
                }
            }
            Helper.DB.PC.Remove(PCExtended.PC);
            Helper.DB.SaveChanges();

            MessageBox.Show("ПК успешно удален", "Успех", MessageBoxButton.OK);
            this.Close();
        }
    }
}
