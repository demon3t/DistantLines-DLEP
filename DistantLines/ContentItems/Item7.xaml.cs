using DLEPCalcul;
using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using WpfApp.Classes;
using WpfApp.DataGridConverters;
using WpfApp.Interfaces;

namespace WpfApp.ContentItems
{
    /// <summary>
    /// Логика взаимодействия для Item7.xaml
    /// </summary>
    public partial class Item7 : UserControl, IRefresh
    {
        private readonly InitialData Data;
        public Item7(InitialData data)
        {
            Data = data;
            InitializeComponent();
            SetPlotStyle();
            SetColumn();
            Table.ItemsSource = Calcul.QPCollection;
        }

        void IRefresh.Refresh()
        {
            Plot.Plot.Clear();

            (double[] Xs, double[] Ys) q1_n = Calcul.Q1_n(Data);
            (double[] Xs, double[] Ys) q2_n = Calcul.Q2_n(Data);
            (double[] Xs, double[] Ys) q1_k = Calcul.Q1_k(Data);
            (double[] Xs, double[] Ys) q2_k = Calcul.Q2_k(Data);

            Plot.Plot.AddScatter(q1_k.Xs, q1_k.Ys, Color.Green, markerSize: 0, label: "Q1=f(P2)").OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Gap;
            Plot.Plot.AddScatter(q1_n.Xs, q1_n.Ys, Color.Blue, markerSize: 0, label: "Q1=f(P1)").OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Gap;
            Plot.Plot.AddScatter(q2_n.Xs, q2_n.Ys, Color.Red, markerSize: 0, label: "Q2=f(P1)").OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Gap;
            Plot.Plot.AddScatter(q2_k.Xs, q2_k.Ys, Color.Purple, markerSize: 0, label: "Q2=f(P2)").OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Gap;

            var vlines = new ScottPlot.Plottable.VLineVector()
            {
                Xs = new double[] { 0 },
                Color = Color.DarkGray,
                DragLimitMin = 0,
                DragLimitMax = q1_n.Xs.Length - 1,
                DragEnabled = true,
                PositionLabel = true,
                PositionLabelBackground = Color.DarkGray,
                PositionFormatter = (x) =>
                {
                    return $"{x} МВт";
                }
            };
            vlines.Dragged += VlinesDragged;

            Plot.Plot.Add(vlines);

            Plot.Refresh();
        }


        private void SetColumn()
        {
            Table.Columns.Add(new DataGridTextColumn()
            {
                Header = "Мощность",
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
                Header = "0",
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
            Table.Columns[1].Header = Math.Round(value, 2).ToString();
            Calcul.QPCollectionRefresh(Data, value);
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
            Plot.Plot.Legend(true, Alignment.UpperRight);
            Plot.Plot.ManualDataArea(new PixelPadding(80, 1, 50, 15));
            Plot.Plot.XLabel("Активная мощность, МВт");
            Plot.Plot.YLabel("Реактивная мощность, Мвар");
        }
    }
}
