// DemoAvalonia (c) 2021 Baltasar MIT License <jbgarcia@uvigo.es>


namespace DemoAvalonia.UI {
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;


    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            var opExit = this.FindControl<MenuItem>( "OpExit" );
            var opAbout = this.FindControl<MenuItem>( "OpAbout" );
            var opSimplePanel = this.FindControl<MenuItem>( "OpSimplePanel" );
            var opStackPanel = this.FindControl<MenuItem>( "OpStackPanel" );
            var opForm = this.FindControl<MenuItem>( "OpForm" );
            var opChart = this.FindControl<MenuItem>( "OpChart" );
            var opMessageBox = this.FindControl<MenuItem>( "OpMessageBox" );
            var btFirst = this.FindControl<Button>( "BtFirst" );
            var btSecond = this.FindControl<Button>( "BtSecond" );
            var btThird = this.FindControl<Button>( "BtThird" );
            var btFourth = this.FindControl<Button>( "BtFourth" );
            var btFifth = this.FindControl<Button>( "BtFifth" );
            
            opExit.Click += (_, _) => this.OnExit();
            opAbout.Click += (_, _) => this.OnAbout();
            
            opSimplePanel.Click += (_, _) => this.OnViewSimplePanel();
            btFirst.Click += (_, _) => this.OnViewSimplePanel();
            
            opStackPanel.Click += (_, _) => this.OnViewStackPanel();
            btSecond.Click += (_, _) => this.OnViewStackPanel();
            
            opForm.Click += (_, _) => this.OnViewForm();
            btThird.Click += (_, _) => this.OnViewForm();

            opChart.Click += (_, _) => this.OnViewChart();
            btFourth.Click += (_, _) => this.OnViewChart();
            
            opMessageBox.Click += (_, _) => this.OnViewMessageBox();
            btFifth.Click += (_, _) => this.OnViewMessageBox();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void OnExit()
        {
            this.Close();
        }

        public void OnAbout()
        {
            new AboutWindow().ShowDialog( this );
        }

        public void OnViewSimplePanel()
        {
            new SimplePanelWindow().Show();
        }

        public void OnViewStackPanel()
        {
            new StackPanelWindow().Show();
        }

        public void OnViewForm()
        {
            new FormWindow().ShowDialog( this );
        }

        public void OnViewChart()
        {
            new ChartWindow().Show();
        }

        public void OnViewMessageBox()
        {
            var dlg = new MessageBox {
                Message = "This is a dialog in Avalonia, this is a dialog in Avalonia"
                    + "this is a dialog in Avalonia, "
                    + "this is a dialog in Avalonia."
            };
                
            dlg.ShowDialog( this );
        }
    }
}
