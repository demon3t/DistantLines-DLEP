using DLEPCalcul;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp.DataGridConverters;
using WpfApp.Interfaces;

namespace WpfApp.ContentItems
{
    /// <summary>
    /// Логика взаимодействия для Item3.xaml
    /// </summary>
    public partial class Item3 : UserControl, IRefresh
    {
        private readonly InitialData Data;
        public Item3(InitialData data)
        {
            Data = data;
            InitializeComponent();
            SetColumn();
            Table1.ItemsSource = Calcul.Coeff1Collection;
            Table2.ItemsSource = Calcul.Coeff2Collection;
        }

        void IRefresh.Refresh()
        {
        }

        private void SetColumn()
        {
            // Для первой таблицы
            Table1.Columns.Add(new DataGridTextColumn()
            {
                Header = "Поправочные коэффициенты",
                Width = new DataGridLength(2, DataGridLengthUnitType.Star),
                IsReadOnly = true,
                Binding = new Binding("Name")
                {
                    Mode = BindingMode.TwoWay,
                },
                CanUserResize = false,
                CanUserSort = false,
            });

            Table1.Columns.Add(new DataGridTextColumn()
            {
                Header = "Значение",
                Width = new DataGridLength(2, DataGridLengthUnitType.Star),
                IsReadOnly = true,
                Binding = new Binding("Value1")
                {
                    Mode = BindingMode.TwoWay,
                    Converter = new ComplexConverter(),
                },
                CanUserResize = false,
                CanUserSort = false,
            });

            // Для второй таблицы
            Table2.Columns.Add(new DataGridTextColumn()
            {
                Header = "Поправочные коэффициенты",
                Width = new DataGridLength(2, DataGridLengthUnitType.Star),
                IsReadOnly = true,
                Binding = new Binding("Name")
                {
                    Mode = BindingMode.TwoWay,
                },
                CanUserResize = false,
                CanUserSort = false,
            });

            Table2.Columns.Add(new DataGridTextColumn()
            {
                Header = "Значение",
                Width = new DataGridLength(2, DataGridLengthUnitType.Star),
                IsReadOnly = true,
                Binding = new Binding("Value1")
                {
                    Mode = BindingMode.TwoWay,
                    Converter = new ComplexConverter(),
                },
                CanUserResize = false,
                CanUserSort = false,
            });

        }
    }
}
