using DLEPCalcul;
using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WpfApp.Classes;
using WpfApp.DataGridConverters;
using WpfApp.Interfaces;

namespace WpfApp.ContentItems
{
    /// <summary>
    /// Логика взаимодействия для Item6.xaml
    /// </summary>
    public partial class Item6 : UserControl, IRefresh
    {
        private readonly InitialData Data;
        public Item6(InitialData data)
        {
            Data = data;
            InitializeComponent();
            SetPlotStyle();
            SetColumn();
            Table.ItemsSource = Calcul.UCollection;
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
                Header = Math.Round(Data.Length / 2, 2).ToString(),
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
                Header = "0",
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

        void IRefresh.Refresh()
        {
            Plot.Plot.Clear();

            (double[] Xs, double[] Ys) more = Calcul.U_More(Data);
            (double[] Xs, double[] Ys) less = Calcul.U_Less(Data);
            (double[] Xs, double[] Ys) xx = Calcul.U_XX(Data);
            (double[] Xs, double[] Ys) nat = Calcul.U_Nat(Data);
            Plot.Plot.AddScatter(nat.Xs, nat.Ys, Color.Purple, markerSize: 0, label: "Мощность равна натуральной").OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Gap;
            Plot.Plot.AddScatter(more.Xs, more.Ys, Color.Green, markerSize: 0, label: "Мощность больше натуральной").OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Gap;
            Plot.Plot.AddScatter(less.Xs, less.Ys, Color.Red, markerSize: 0, label: "Мощность меньше натуральной").OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Gap;
            Plot.Plot.AddScatter(xx.Xs, xx.Ys, Color.Blue, markerSize: 0, label: "Одностороннее включение").OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Gap;

            var vlines = new ScottPlot.Plottable.VLineVector()
            {
                Xs = new double[] { 0 },
                Color = Color.DarkGray,
                DragLimitMin = 0,
                DragLimitMax = Data.Length,
                DragEnabled = true,
                PositionLabel = true,
                PositionLabelBackground = Color.DarkGray,
                PositionFormatter = (x) =>
                {
                    return $"{x} км";
                }
            };
            vlines.Dragged += VlinesDragged;

            Plot.Plot.Add(vlines);
            Plot.Refresh();


            Table.Columns[1].Header = Math.Round(Data.Length / 2, 2).ToString();
        }

        private void VlinesDragged(object sender, System.EventArgs e)
        {
            if (sender is VLineVector line)
            {
                line.Xs[0] = (int)line.Xs[0];
                DataRefresh(line.Xs[0]);
                Plot.Refresh();
            }
        }

        private void DataRefresh(double value)
        {
            Table.Columns[2].Header = Math.Round(value, 2).ToString();
            Calcul.UCollectionRefresh(Data, value);
        }

        private void PlotSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is WpfPlot plot)
            {
                plot.Height = plot.ActualWidth * 2 / 4;
            }
        }

        private void SetPlotStyle()
        {
            Plot.Configuration.Zoom = false;
            Plot.Configuration.Pan = false;
            Plot.Plot.Style(PlotStyle.Default);
            Plot.Plot.Legend(true, Alignment.UpperLeft);
            Plot.Plot.ManualDataArea(new PixelPadding(80, 1, 50, 15));
            Plot.Plot.XLabel("Длина линии, км");
            Plot.Plot.YLabel("Напряжение, кВ");
        }
    }
}
