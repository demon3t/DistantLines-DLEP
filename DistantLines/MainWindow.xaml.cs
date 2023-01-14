using System;
using System.Windows;
using System.Windows.Controls;
using DLEPCalcul;
using WpfApp.ContentItems;
using WpfApp.Interfaces;

namespace DistantLines
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly InitialData Data = new InitialData();

        public MainWindow()
        {
            Data.ValueChange += OnValueChange;
            InitializeComponent();
            SetContent();
        }

        private void OnValueChange()
        {
            foreach (TabItem item in ContentViewer.Items)
            {
                ((IRefresh)item.Content).Refresh();
            }
            Calcul.Refresh(Data);
        }

        private void SetContent()
        {
            ContentViewer.Items.Add(new TabItem()
            {
                Header = "Расчитанные данные",
                Content = new Item1(Data),
            });
            ContentViewer.Items.Add(new TabItem()
            {
                Header = "Данные четырёхполюсника",
                Content = new Item2(Data),
            });
            ContentViewer.Items.Add(new TabItem()
            {
                Header = "Поправочные коэффициенты",
                Content = new Item3(Data),
            });
            ContentViewer.Items.Add(new TabItem()
            {
                Header = "Реакторы",
                Content = new Item4(Data),
            });
            ContentViewer.Items.Add(new TabItem()
            {
                Header = "УПК",
                Content = new Item5(Data),
            });
            ContentViewer.Items.Add(new TabItem()
            {
                Header = "Распреденение напряжения",
                Content = new Item6(Data),
            });
            ContentViewer.Items.Add(new TabItem()
            {
                Header = "Поддержание напряжения",
                Content = new Item7(Data),
            });
        }

        #region Исходные данные

        public decimal VoltNom
        {
            get { return (decimal)Data.VoltNom; }
            set { Data.VoltNom = (double)value; }
        }

        public decimal Length
        {
            get { return (decimal)Data.Length; }
            set { Data.Length = (double)value; }
        }

        public decimal N_split
        {
            get { return Data.N_split; }
            set { Data.N_split = (int)value; }
        }

        public decimal R0
        {
            get { return (decimal)Data.R0; }
            set
            {
                value = Math.Round(value, 6);
                Data.R0 = (double)value;
            }
        }

        public decimal D_phase
        {
            get { return (decimal)Data.D_phase; }
            set
            {
                value = Math.Round(value, 6);
                Data.D_phase = (double)value;
            }
        }

        public decimal A_splitwires
        {
            get { return (decimal)Data.A_splitwires; }
            set { Data.A_splitwires = (double)value; }
        }

        public decimal D_wire
        {
            get { return (decimal)Data.D_wire; }
            set
            {
                value = Math.Round(value, 6);
                Data.D_wire = (double)value;
            }
        }

        public decimal F_st
        {
            get { return (decimal)Data.F_st; }
            set { Data.F_st = (double)value; }
        }

        public decimal F_al
        {
            get { return (decimal)Data.F_al; }
            set { Data.F_al = (double)value; }
        }

        public decimal K1
        {
            get { return (decimal)Data.K1; }
            set
            {
                value = Math.Round(value, 6);
                Data.K1 = (double)value;
            }
        }

        public decimal K2
        {
            get { return (decimal)Data.K2; }
            set
            {
                value = Math.Round(value, 6);
                Data.K2 = (double)value;
            }
        }

        #endregion

    }
}

