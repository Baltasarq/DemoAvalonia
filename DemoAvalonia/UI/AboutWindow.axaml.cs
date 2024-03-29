// DemoAvalonia (c) 2021/23 Baltasar MIT License <jbgarcia@uvigo.es>


namespace DemoAvalonia.UI {
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;
    
    using DemoAvalonia.Core;
    
    
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            var txtAbout = this.GetControl<TextBlock>( "TxtAbout" );
            var btOk = this.GetControl<Button>( "BtOk" );
            
            txtAbout.Text = AppInfo.Name + " v" + AppInfo.Version
                            + "\n" + AppInfo.Email;

            btOk.Click += (o, args) => this.OnExit();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void OnExit()
        {
            this.Close();
        }
    }
}
