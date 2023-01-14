using DLEPCalcul;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using WpfApp.DataGridConverters;
using WpfApp.Interfaces;

namespace WpfApp.ContentItems
{
    /// <summary>
    /// Логика взаимодействия для Item2.xaml
    /// </summary>
    public partial class Item2 : UserControl, IRefresh
    {
        private readonly InitialData Data;
        public Item2(InitialData data)
        {
            Data = data;
            InitializeComponent();
            SetColumn();
            Table.ItemsSource = Calcul.FourPoleCollection;
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
                Header = "С учётом потерь",
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
                Header = "Без учётом потерь",
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
