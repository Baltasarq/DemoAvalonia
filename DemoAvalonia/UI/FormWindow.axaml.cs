// DemoAvalonia (c) 2021 Baltasar MIT License <jbgarcia@uvigo.es>



namespace DemoAvalonia.UI {
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;
    using System.Collections.ObjectModel;
    using DemoAvalonia.Core;

    
    public partial class FormWindow : Window
    {
        public FormWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.stations = new ObservableCollection<Station>( new Station[] {
                new (){ City = "Madrid", Name = "Chamartín" },
                new (){ City = "Madrid", Name = "Atocha" },
                new (){ City = "Ourense", Name = "Empalme" },
                new (){ City = "Ourense", Name = "San Francisco" },
            });

            var cbStart = this.FindControl<ComboBox>( "CbStart" );
            var cbEnd = this.FindControl<ComboBox>( "CbEnd" );
            var btOk = this.FindControl<Button>( "BtOk" );
            var btCancel = this.FindControl<Button>( "BtCancel" );

            // Modifications in the 'stations' list will be shown in the combos
            cbStart.Items = this.stations;
            cbStart.SelectedIndex = 0;
            cbEnd.Items = this.stations;
            cbEnd.SelectedIndex = 0;
            btOk.Click += (o, args) => this.OnClose( true );
            btCancel.Click += (o, args) => this.OnClose( false );
        }
        
        public int NumChildren {
            get {
                var edNumChildren = this.FindControl<NumericUpDown>( "EdNumChildren" );
                return (int) edNumChildren.Value;
            }
        }

        public Station Start {
            get {
                var cbStart = this.FindControl<ComboBox>( "CbStart" );
                return this.stations[ cbStart.SelectedIndex ];
            }
        }
        
        public Station End {
            get {
                var cbEnd = this.FindControl<ComboBox>( "CbEnd" );
                return this.stations[ cbEnd.SelectedIndex ];
            }
        }

        public string Kind {
            get {
                var cbKind = this.FindControl<ComboBox>( "CbKind" );
                return ( (ComboBoxItem?) cbKind?.SelectedItem )?.Content.ToString() ??
                    "Nothing";
            }
        }

        public bool Accept => this.accept;

        void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        void OnClose(bool accept)
        {
            this.accept = accept;
            this.Close();
        }

        ObservableCollection<Station> stations;
        bool accept;
    }
}
