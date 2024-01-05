// DemoAvalonia (c) 2021/23 Baltasar MIT License <jbgarcia@uvigo.es>



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
            this._stations = new ObservableCollection<Station>( new Station[] {
                new (){ City = "Madrid", Name = "Chamartín" },
                new (){ City = "Madrid", Name = "Atocha" },
                new (){ City = "Ourense", Name = "Empalme" },
                new (){ City = "Ourense", Name = "San Francisco" },
            });

            var cbStart = this.GetControl<ComboBox>( "CbStart" );
            var cbEnd = this.GetControl<ComboBox>( "CbEnd" );
            var btOk = this.GetControl<Button>( "BtOk" );
            var btCancel = this.GetControl<Button>( "BtCancel" );

            // Modifications in the 'stations' list will be shown in the combos
            cbStart.ItemsSource = this._stations;
            cbStart.SelectedIndex = 0;
            cbEnd.ItemsSource = this._stations;
            cbEnd.SelectedIndex = 0;
            btOk.Click += (o, args) => this.OnClose( true );
            btCancel.Click += (o, args) => this.OnClose( false );
        }
        
        public int NumChildren {
            get {
                var edNumChildren = this.GetControl<NumericUpDown>( "EdNumChildren" );
                int? toret = (int?) edNumChildren.Value;
                
                return toret ?? 0;
            }
        }

        public Station Start {
            get {
                var cbStart = this.GetControl<ComboBox>( "CbStart" );
                return this._stations[ cbStart.SelectedIndex ];
            }
        }
        
        public Station End {
            get {
                var cbEnd = this.GetControl<ComboBox>( "CbEnd" );
                return this._stations[ cbEnd.SelectedIndex ];
            }
        }

        public string Kind {
            get {
                var cbKind = this.GetControl<ComboBox>( "CbKind" );
                return ( (ComboBoxItem?) cbKind?.SelectedItem )?.Content?.ToString() ??
                    "Nothing";
            }
        }

        public bool Accept => this._accept;

        void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        void OnClose(bool accept)
        {
            this._accept = accept;
            this.Close();
        }

        private ObservableCollection<Station> _stations;
        private bool _accept;
    }
}
