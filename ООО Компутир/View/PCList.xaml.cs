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
    /// Логика взаимодействия для PCList.xaml
    /// </summary>
    public partial class PCList : Window
    {
        public PCList()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Информация о пользователе
            //Зарегистрированный пользователь
            if (Helper.User != null)
            {
                tbUserName.Text = "Пользователь: " + Helper.User.UserName;

                if (Helper.User.UserRole == 1)
                {
                    bRentHistory.Visibility = Visibility.Visible;
                    miRent.Visibility = Visibility.Visible;
                    miEdit.Visibility = Visibility.Collapsed;
                }
                else
                {
                    bAddPC.Visibility = Visibility.Visible;
                    miRent.Visibility = Visibility.Collapsed;
                    miEdit.Visibility = Visibility.Visible;
                }
            }
            //Гость
            else
            {
                tbUserName.Text = "Пользователь: Гость";
                cmPC.IsEnabled = false;
                miRent.Visibility = Visibility.Collapsed;
                miEdit.Visibility = Visibility.Collapsed;
            }

            Init();
            Sort();
        }

        private void rbSortAsc_Checked(object sender, RoutedEventArgs e)
        {
            Sort();
        }

        private void rbSortDesc_Checked(object sender, RoutedEventArgs e)
        {
            Sort();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Sort();
        }

        private void cbProgram_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sort();
        }

        private void cbDiscount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sort();
        }

        public void Init()
        {
            //Настройка ComboBox для фильтрации по скидке
            cbDiscount.Items.Clear();
            cbDiscount.Items.Add("Все диапазоны");
            cbDiscount.Items.Add("0-9,99%");
            cbDiscount.Items.Add("10-14,99%");
            cbDiscount.Items.Add("15% и более");
            cbDiscount.SelectedIndex = 0;

            //Перенос списка програм в ComboBox для сортировки
            List<Program> programs = Helper.DB.Program.ToList();
            Program allPrograms = new Program();
            allPrograms.ProgramID = 0;
            allPrograms.ProgramName = "Все ПО";
            allPrograms.ProgramCost = 0;
            programs.Insert(0, allPrograms);
            cbProgram.Items.Clear();
            cbProgram.ItemsSource = programs;

            cbProgram.DisplayMemberPath = "ProgramName";
            cbProgram.SelectedValuePath = "ProgramID";
            cbProgram.SelectedValue = 0;

            rbSortAsc.IsChecked = true;
        }

        public void Sort()
        {

            List<PC> listPC = Helper.DB.PC.ToList();

            List<PCExtended> listPCExt = new List<PCExtended>();

            //Фильтрация по скидке
            double minDiscount = 0;
            double maxDiscount = 100;
            switch (cbDiscount.SelectedIndex)
            {
                case 1:
                    maxDiscount = 9.99;
                    break;
                case 2:
                    minDiscount = 10;
                    maxDiscount = 14.99;
                    break;
                case 3:
                    minDiscount = 15;
                    break;
                default:
                    break;
            }
            listPC = listPC.Where(p => p.PCDiscount <= maxDiscount && p.PCDiscount >= minDiscount).ToList();

            //Фильтрация по программам
            if (cbProgram.SelectedIndex > 0)
            {
                List<Program> programs = Helper.DB.Program.ToList();
                Program selected = programs[(int)cbProgram.SelectedValue - 1];
                listPC = listPC.Where(p => p.Program.Contains(selected)).ToList();
            }

            //Перенос в расширенный список
            foreach (PC PC in listPC)
            {
                PCExtended PCExt = new PCExtended();
                PCExt.PC = PC;
                listPCExt.Add(PCExt);
            }

            //Сортировка по цене
            if ((bool)rbSortAsc.IsChecked)
            {
                listPCExt = listPCExt.OrderBy(p => p.PCDiscountCost).ToList();
            }
            else
            {
                listPCExt = listPCExt.OrderByDescending(p => p.PCDiscountCost).ToList();
            }

            lbPCList.ItemsSource = listPCExt;
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void miRent_Click(object sender, RoutedEventArgs e)
        {
            View.AddSession addSession = new View.AddSession(lbPCList.SelectedItem as PCExtended);
            this.Hide();
            addSession.ShowDialog();
            this.ShowDialog();
        }

        private void miEdit_Click(object sender, RoutedEventArgs e)
        {
            View.PCAddEdit PCAddEdit = new View.PCAddEdit(lbPCList.SelectedItem as PCExtended);
            this.Hide();
            PCAddEdit.ShowDialog();
            this.ShowDialog();
        }

        private void bRentHistory_Click(object sender, RoutedEventArgs e)
        {
            View.PCRentHistory PCRentHistory = new View.PCRentHistory();
            this.Hide();
            PCRentHistory.ShowDialog();
            this.ShowDialog();
        }

        private void bAddPC_Click(object sender, RoutedEventArgs e)
        {
            View.PCAddEdit PCAddEdit = new View.PCAddEdit();
            this.Hide();
            PCAddEdit.ShowDialog();
            this.ShowDialog();
        }
    }
}
