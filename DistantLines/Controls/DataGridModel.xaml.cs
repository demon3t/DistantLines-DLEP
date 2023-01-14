using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
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

namespace WpfApp.Controls
{
    /// <summary>
    /// Логика взаимодействия для DataGridModel.xaml
    /// </summary>
    public partial class DataGridModel : UserControl
    {
        public class DataShell
        {
            public string Name { get; set; }
            public Complex[] Value { get; set; }
        }

        public DataGridModel(string FLabel, double[] coumumnCollection, List<DataShell> numbers)
        {
            InitializeComponent();
            SetColumn(FLabel, coumumnCollection);
            Table.ItemsSource = new ObservableCollection<DataShell>(numbers);
        }

        private void SetColumn(string FLabel, double[] headers)
        {
            Table.Columns.Add(new DataGridTextColumn()
            {
                Header = FLabel,
                Width = new DataGridLength(5, DataGridLengthUnitType.Star),
                IsReadOnly = true,
                Binding = new Binding("Name")
                {
                    Mode = BindingMode.TwoWay,
                },
                CanUserResize = false,
                CanUserSort = false,
            });

            for (int i = 0; i < headers.Length; i++)
            {
                Table.Columns.Add(new DataGridTextColumn()
                {
                    Header = $"{headers[i]}",
                    Width = new DataGridLength(2, DataGridLengthUnitType.Star),
                    IsReadOnly = true,
                    Binding = new Binding($"Value[{i}]")
                    {
                        Mode = BindingMode.TwoWay,
                        Converter = new ComplexConverter()
                    },
                    CanUserResize = false,
                    CanUserSort = false,
                });
            }
        }
    }
}
