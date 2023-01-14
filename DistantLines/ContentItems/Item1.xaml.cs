using DLEPCalcul;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp.DataGridConverters;
using WpfApp.Interfaces;

namespace WpfApp.ContentItems
{
    /// <summary>
    /// Логика взаимодействия для Item1.xaml
    /// </summary>
    public partial class Item1 : UserControl, IRefresh
    {
        private readonly InitialData Data;
        public Item1(InitialData data)
        {
            Data = data;
            InitializeComponent();
            SetColumn();
            Table.ItemsSource = Calcul.ParamCollection;
        }

        void IRefresh.Refresh()
        {
            
        }

        private void SetColumn()
        {
            Table.Columns.Add(new DataGridTextColumn()
            {
                Header = "Параметр",
                Width = new DataGridLength(2, DataGridLengthUnitType.Star),
                IsReadOnly = true,
                Binding = new Binding("Name")
                {
                    Mode = BindingMode.TwoWay,
                },
                CanUserResize = false,
                CanUserSort = false,
            });

            Table.Columns.Add(new DataGridTextColumn()
            {
                Header = "Одиночный провод",
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

            Table.Columns.Add(new DataGridTextColumn()
            {
                Header = "Расщеплённый провод",
                Width = new DataGridLength(2, DataGridLengthUnitType.Star),
                IsReadOnly = true,
                Binding = new Binding("Value2")
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
