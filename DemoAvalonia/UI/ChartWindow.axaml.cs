// DemoAvalonia (c) 2021/23 Baltasar MIT License <jbgarcia@uvigo.es>


using System;
using Avalonia.Media;

namespace DemoAvalonia.UI {
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;
    

    public partial class ChartWindow : Window
    {
        public ChartWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            this.Chart = this.GetControl<Chart>( "ChGrf" );
            var rbBars = this.GetControl<RadioButton>( "RbBars" );
            var rbLine = this.GetControl<RadioButton>( "RbLine" );
            var edThickness = this.GetControl<NumericUpDown>( "EdThickness" );
            var cbBold = this.GetControl<CheckBox>( "CbBold" );
            var cbItalic = this.GetControl<CheckBox>( "CbItalic" );
            
            rbBars.IsCheckedChanged += (_, _) => this.OnChartFormatChanged();
            rbLine.IsCheckedChanged += (_, _) => this.OnChartFormatChanged();
            edThickness.ValueChanged += (_, evt) =>
                this.OnChartThicknessChanged( ( (double?) evt.NewValue ) ?? 1.0 );
            cbBold.Click += (_, _) => this.OnFontsStyleChanged();
            cbItalic.Click += (_, _) => this.OnFontsStyleChanged();
            
            this.Chart.LegendY = "Sells (in thousands)";
            this.Chart.LegendX = "Months";
            this.Chart.Values = new []{ 10, 20, 30, 40, 25, 21, 11, 2, 28, 33, 18, 45 };
            this.Chart.Labels = new []{ "En", "Fb", "Ma", "Ab", "My", "Jn", "Jl", "Ag", "Sp", "Oc", "Nv", "Dc" };
        }

        void OnChartFormatChanged()
        {
            var rbBars = this.GetControl<RadioButton>( "RbBars" );
            var rbLine = this.GetControl<RadioButton>( "RbLine" );
            var edThickness = this.GetControl<NumericUpDown>( "EdThickness" );

            if ( rbBars.IsChecked.HasValue
             && rbBars.IsChecked.Value )
            {
                this._chartType = Chart.ChartType.Bars;
            }
            else
            if ( rbLine.IsChecked.HasValue
              && rbLine.IsChecked.Value )
            {
                this._chartType = Chart.ChartType.Lines;
            }
            
            if ( this._chartType == Chart.ChartType.Lines ) {
                this.Chart.Type = Chart.ChartType.Lines;
                this.Chart.DataPen = new Pen( Brushes.Red, 2 * ( ( (double?) edThickness.Value ) ?? 1 ) );
            } else {
                this.Chart.Type = Chart.ChartType.Bars;
                this.Chart.DataPen = new Pen( Brushes.Navy, 20 * ( ( (double?) edThickness.Value ?? 1 ) ) );
            }
            
            this.Chart.Draw();
        }

        void OnChartThicknessChanged(double thickness)
        {
            if ( this.Chart.Type == Chart.ChartType.Bars ) {
                this.Chart.DataPen = new Pen( this.Chart.DataPen.Brush, 20 * thickness );
            } else {
                this.Chart.DataPen = new Pen( this.Chart.DataPen.Brush, 2 * thickness );
            }
            
            this.Chart.AxisPen = new Pen( this.Chart.AxisPen.Brush, 4 * thickness );
            this.Chart.Draw();
        }

        void OnFontsStyleChanged()
        {
            var cbBold = this.GetControl<CheckBox>( "CbBold" );
            var cbItalic = this.GetControl<CheckBox>( "CbItalic" );
            bool italic = cbItalic.IsChecked ?? false;
            bool bold = cbBold.IsChecked ?? false;
            FontStyle style = italic ? FontStyle.Italic : FontStyle.Normal;
            FontWeight weight = bold ? FontWeight.Bold : FontWeight.Normal;

            this.Chart.DataFont = new Chart.Font( this.Chart.DataFont.Size ) {
                Family = this.Chart.DataFont.Family,
                Style = style,
                Weight = weight
            };
            
            this.Chart.LabelFont = new Chart.Font( this.Chart.LabelFont.Size ) {
                Family = this.Chart.LabelFont.Family,
                Style = style,
                Weight = weight
            };
            
            this.Chart.LegendFont = new Chart.Font( this.Chart.LegendFont.Size ) {
                Family = this.Chart.LegendFont.Family,
                Style = style,
                Weight = weight
            };
            
            this.Chart.Draw();
        }

        void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        private Chart Chart { get; }
        private Chart.ChartType _chartType;
    }
}
