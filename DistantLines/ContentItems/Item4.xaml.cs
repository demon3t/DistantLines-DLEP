using DLEPCalcul;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using ScottPlot;
using ScottPlot.Plottable;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using WpfApp.Classes;
using WpfApp.Controls;
using WpfApp.Interfaces;

namespace WpfApp.ContentItems
{
    /// <summary>
    /// Логика взаимодействия для Item4.xaml
    /// </summary>
    public partial class Item4 : UserControl, IRefresh
    {
        private readonly InitialData Data;
        public Item4(InitialData data)
        {
            Data = data;
            InitializeComponent();
            SetPlotStyle();
            ((IRefresh)this).Refresh();
        }

        void IRefresh.Refresh()
        {
            PlotA.Plot.Clear();
            PlotB.Plot.Clear();
            PlotC.Plot.Clear();
            PlotD.Plot.Clear();
            PlotR.Plot.Clear();
            PlotPower.Plot.Clear();
            PlotAlpha.Plot.Clear();

            (double[] Xs, double[] Ys, Complex[] Cs) A = Calcul.A_react(Data, 0, 4, 1);
            (double[] Xs, double[] Ys, Complex[] Cs) B = Calcul.B_react(Data, 0, 4, 1);
            (double[] Xs, double[] Ys, Complex[] Cs) C = Calcul.C_react(Data, 0, 4, 1);
            (double[] Xs, double[] Ys, Complex[] Cs) D = Calcul.D_react(Data, 0, 4, 1);

            Xs = A.Xs;

            CplxA = A.Cs;
            CplxB = B.Cs;
            CplxC = C.Cs;
            CplxD = D.Cs;

            (double[] Xs, double[] Ys) R = Calcul.Zv_react(Data, 0, 4, 1);
            (double[] Xs, double[] Ys) Power = Calcul.Pc_react(Data, 0, 4, 1);
            (double[] Xs, double[] Ys) Alpha = Calcul.Alpha_react(Data, 0, 4, 1);

            plotA = PlotA.Plot.AddScatter(A.Xs, A.Ys, Color.Blue, label: "A");
            plotA.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Gap;

            plotB = PlotB.Plot.AddScatter(B.Xs, B.Ys, Color.Blue, label: "B");
            plotB.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Gap;

            plotC = PlotC.Plot.AddScatter(C.Xs, C.Ys, Color.Blue, label: "C");
            plotC.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Gap;

            plotD = PlotD.Plot.AddScatter(D.Xs, D.Ys, Color.Blue, label: "D");
            plotD.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Gap;

            plotR = PlotR.Plot.AddScatter(R.Xs, R.Ys, Color.Blue, label: "Сопротивление");
            plotR.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Gap;

            plotPower = PlotPower.Plot.AddScatter(Power.Xs, Power.Ys, Color.Blue, label: "Мощность");
            plotPower.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Gap;

            plotAlpha = PlotAlpha.Plot.AddScatter(Alpha.Xs, Alpha.Ys, Color.Blue, label: "Угол");
            plotAlpha.OnNaN = ScottPlot.Plottable.ScatterPlot.NanBehavior.Gap;

            PlotA.Refresh();
            PlotB.Refresh();
            PlotC.Refresh();
            PlotD.Refresh();
            PlotR.Refresh();
            PlotPower.Refresh();
            PlotAlpha.Refresh();
        }

        ScatterPlot plotR;
        ScatterPlot plotAlpha;
        ScatterPlot plotPower;
        ScatterPlot plotA;
        ScatterPlot plotB;
        ScatterPlot plotC; 
        ScatterPlot plotD;

        double[] Xs;
        Complex[] CplxA;
        Complex[] CplxB;
        Complex[] CplxC;
        Complex[] CplxD;

        private void SetPlotStyle()
        {
            // График сопротивлений
            PlotR.Configuration.Zoom = false;
            PlotR.Configuration.Pan = false;
            PlotR.Plot.Style(PlotStyle.Default);
            PlotR.Plot.Legend(true, Alignment.LowerCenter);
            PlotR.Plot.ManualDataArea(new PixelPadding(80, 1, 50, 15));
            PlotR.Plot.XLabel("Колличесво реакторов");
            PlotR.Plot.YLabel("Волновое сопротивление, Ом");

            // График Альфа
            PlotAlpha.Configuration.Zoom = false;
            PlotAlpha.Configuration.Pan = false;
            PlotAlpha.Plot.Style(PlotStyle.Default);
            PlotAlpha.Plot.Legend(true, Alignment.LowerCenter);
            PlotAlpha.Plot.ManualDataArea(new PixelPadding(80, 5, 50, 15));
            PlotAlpha.Plot.XLabel("Колличесво реакторов");
            PlotAlpha.Plot.YLabel("Угол, градусы");

            // График Мощности
            PlotPower.Configuration.Zoom = false;
            PlotPower.Configuration.Pan = false;
            PlotPower.Plot.Style(PlotStyle.Default);
            PlotPower.Plot.Legend(true, Alignment.LowerCenter);
            PlotPower.Plot.ManualDataArea(new PixelPadding(80, 5, 50, 15));
            PlotPower.Plot.XLabel("Колличесво реакторов");
            PlotPower.Plot.YLabel("Натуральная мощность, МВт");

            // График A
            PlotA.Configuration.Zoom = false;
            PlotA.Configuration.Pan = false;
            PlotA.Plot.Style(PlotStyle.Default);
            PlotA.Plot.Legend(true, Alignment.LowerCenter);
            PlotA.Plot.ManualDataArea(new PixelPadding(80, 5, 50, 15));
            PlotA.Plot.XLabel("Колличесво реакторов");
            PlotA.Plot.YLabel("Постоянная А");

            // График B
            PlotB.Configuration.Zoom = false;
            PlotB.Configuration.Pan = false;
            PlotB.Plot.Style(PlotStyle.Default);
            PlotB.Plot.Legend(true, Alignment.LowerCenter);
            PlotB.Plot.ManualDataArea(new PixelPadding(80, 5, 50, 15));
            PlotB.Plot.XLabel("Колличесво реакторов");
            PlotB.Plot.YLabel("Постоянная В, Ом");

            // График C
            PlotC.Configuration.Zoom = false;
            PlotC.Configuration.Pan = false;
            PlotC.Plot.Style(PlotStyle.Default);
            PlotC.Plot.Legend(true, Alignment.LowerCenter);
            PlotC.Plot.ManualDataArea(new PixelPadding(80, 5, 50, 15));
            PlotC.Plot.XLabel("Колличесво реакторов");
            PlotC.Plot.YLabel("Постоянная С, См");

            // График D
            PlotD.Configuration.Zoom = false;
            PlotD.Configuration.Pan = true;
            PlotD.Plot.Style(PlotStyle.Default);
            PlotD.Plot.Legend(true, Alignment.LowerCenter);
            PlotD.Plot.ManualDataArea(new PixelPadding(80, 5, 50, 15));
            PlotD.Plot.XLabel("Колличесво реакторов");
            PlotD.Plot.YLabel("Постоянная D");
        }

        private Complex[] GetComplexCollection(ScatterPlot plot)
        {
            Complex[] result = new Complex[plot.Ys.Length];

            for (int i = 0; i < plot.Ys.Length; i++)
            {
                result[i] = new Complex(plot.Ys[i], 0);
            }
            return result;
        }

        private void DataClick(object sender, RoutedEventArgs e)
        {
            Dialog.Show(new DataGridModel("Колличество реакторов", Xs, new List<DataGridModel.DataShell>()
            {
                new DataGridModel.DataShell()
                {
                    Name = "Волновое сопротивление, Ом",
                    Value = GetComplexCollection(plotR),
                },
                new DataGridModel.DataShell()
                {
                    Name = "Натуральная мощность, МВт",
                    Value = GetComplexCollection(plotPower),
                },
                new DataGridModel.DataShell()
                {
                    Name = "Угол, градусы",
                    Value = GetComplexCollection(plotAlpha),
                },
                new DataGridModel.DataShell()
                {
                    Name = "Постоянная А",
                    Value = CplxA,
                },
                new DataGridModel.DataShell()
                {
                    Name = "Постоянная B",
                    Value = CplxB,
                },
                new DataGridModel.DataShell()
                {
                    Name = "Постоянная C",
                    Value = CplxC,
                },
                new DataGridModel.DataShell()
                {
                    Name = "Постоянная D",
                    Value = CplxD,
                },
            }));
        }

        private void PlotSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is WpfPlot plot)
            {
                plot.Height = plot.ActualWidth * 2 / 4;
            }
        }
    }
}
