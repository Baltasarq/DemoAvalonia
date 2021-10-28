// DemoAvalonia (c) 2021 Baltasar MIT License <jbgarcia@uvigo.es>


namespace DemoAvalonia.UI {
    using Avalonia;
    using Avalonia.Media;
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

            this.Chart = this.FindControl<Chart>( "ChGrf" );
            var rbBars = this.FindControl<RadioButton>( "RbBars" );
            var rbLine = this.FindControl<RadioButton>( "RbLine" );

            rbBars.Checked += (_, _) => this.OnChartFormatChanged();
            rbLine.Checked += (_, _) => this.OnChartFormatChanged();
            
            this.Chart.LegendY = "Sells (in thousands)";
            this.Chart.LegendX = "Months";
            this.Chart.Values = new []{ 10, 20, 30, 40, 25, 21, 11, 2, 28, 33, 18, 45 };
        }

        void OnChartFormatChanged()
        {
            if ( this.Chart.Type == Chart.ChartType.Bars ) {
                this.Chart.Type = Chart.ChartType.Lines;
            } else {
                this.Chart.Type = Chart.ChartType.Bars;
            }
            
            this.Chart.Draw();
        }
        
        public override void Render(DrawingContext context)
        {
            base.Render( context );

            this.Chart.Draw();
        }

        void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        Chart Chart { get; }
    }
}
