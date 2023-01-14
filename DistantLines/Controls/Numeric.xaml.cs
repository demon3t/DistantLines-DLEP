using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp.Converters;
using TextBox = System.Windows.Controls.TextBox;

namespace WpfApp.Controls
{
    /// <summary>
    /// Логика взаимодействия для Numeric.xaml
    /// </summary>
    public partial class Numeric : UserControl
    {
        public Numeric()
        {
            InitializeComponent();
            Value = 0;
            Increment = 1;
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        "Value", typeof(double), typeof(Numeric), new PropertyMetadata(default(double)));

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set
            {
                if (value < Minimum || value > Maximum)
                {
                    return;
                }
                SetValue(ValueProperty, value);
            }
        }

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
        "Minimum", typeof(double), typeof(Numeric), new PropertyMetadata(default(double)));

        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
        "Maximum", typeof(double), typeof(Numeric), new PropertyMetadata(default(double)));

        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(
        "Increment", typeof(double), typeof(Numeric), new PropertyMetadata(default(double)));

        public double Increment
        {
            get => (double)GetValue(IncrementProperty);
            set => SetValue(IncrementProperty, value);
        }

        private void TextBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                Value += Increment;
            }
            if (e.Delta < 0)
            {
                Value -= Increment;
            }
            TextBox.Text = Value.ToString();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) || e.Text == "." || e.Text == ","))
            {
                e.Handled = true;
                return;
            }

            if (sender is TextBox textBox)
                if ((e.Text == "." || e.Text == ",") && (textBox.Text.Contains(",") || textBox.Text.Contains(".")))
                {
                    e.Handled = true;
                    return;
                }
            if(double.TryParse(TextBox.Text.Replace('.', ','), out double result))
            {
                Value = result;
            }
        }

        private void TextBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            Value = double.Parse(TextBox.Text.Replace('.', ','));
            if (!TextBox.IsFocused)
            {
                TextBox.Text = Value.ToString();
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(TextBox.Text.Replace('.',','), out double result))
            {
                Value = result;
            }
            TextBox.Text = Value.ToString();
        }
    }
}
