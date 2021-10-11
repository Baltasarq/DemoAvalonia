// DemoAvalonia (c) 2021 Baltasar MIT License <jbgarcia@uvigo.es>


namespace DemoAvalonia.UI {
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;
    
    
    public partial class FormWindow : Window
    {
        public FormWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            var btOk = this.FindControl<Button>( "BtOk" );
            var btCancel = this.FindControl<Button>( "BtCancel" );

            btOk.Click += (o, args) => this.OnClose();
            btCancel.Click += (o, args) => this.OnClose();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnClose()
        {
            this.Close();
        }
    }
}
