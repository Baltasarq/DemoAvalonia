// DemoAvalonia (c) 2021 Baltasar MIT License <jbgarcia@uvigo.es>


namespace DemoAvalonia.UI {
    using System;
    using System.Collections.Generic;

    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;


    class Node {
        public required string Title { init; get; }
        public required Action Action { init; get; }

        public override string ToString() => this.Title;
    }

    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            var tvOptions = this.FindControl<TreeView>( "tvOptions" );
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
            var cbOptions = this.FindControl<ComboBox>( "CbOptions" );
            
            opExit.Click += (_, _) => this.OnExit();
            opAbout.Click += (_, _) => this.OnAbout();

            // Actions
            var actionTitles = new [] {
                "Simple panel",
                "Stack panel",
                "Form",
                "Graph",
                "Message box"
            };
            
            var actions = new Action[] {
                this.OnViewSimplePanel,
                this.OnViewStackPanel,
                this.OnViewForm,
                this.OnViewChart,
                this.OnViewMessageBox
            };
            
            opSimplePanel.Click += (_, _) => actions[ 0 ]();
            btFirst.Click += (_, _) => actions[ 0 ]();
            
            opStackPanel.Click += (_, _) => actions[ 1 ]();
            btSecond.Click += (_, _) => actions[ 1 ]();
            
            opForm.Click += (_, _) => actions[ 2 ]();
            btThird.Click += (_, _) => actions[ 2 ]();

            opChart.Click += (_, _) => actions[ 3 ]();
            btFourth.Click += (_, _) => actions[ 3 ]();
            
            opMessageBox.Click += (_, _) => actions[ 4 ]();
            btFifth.Click += (_, _) => actions[ 4 ]();
            
            // Load combobox (can be done in the YAML as well)
            cbOptions.Items = actionTitles;
            cbOptions.SelectedIndex = 0;            
            cbOptions.SelectionChanged += (_, _) => {
                if ( cbOptions.SelectedIndex >= 0 ) {
                    actions[ cbOptions.SelectedIndex ]();
                }
            };
            
            // Prepare treeview
            var listNodes = new [] {
                new Node{ Title = actionTitles[ 0 ], Action = actions[ 0 ] },
                new Node{ Title = actionTitles[ 1 ], Action = actions[ 1 ] },
                new Node{ Title = actionTitles[ 2 ], Action = actions[ 2 ] },
                new Node{ Title = actionTitles[ 3 ], Action = actions[ 3 ] },
                new Node{ Title = actionTitles[ 4 ], Action = actions[ 4 ] }
            };

            tvOptions.Items = listNodes;
            tvOptions.SelectedItem = listNodes[ 0 ];
            tvOptions.SelectionChanged += (_, _) => {
                var node = (Node) tvOptions.SelectedItem;
                node?.Action();
            };
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

        public async void OnViewForm()
        {
            var form = new FormWindow();
            
            await form.ShowDialog( this );
            
            var dlg = new MessageBox {
                Message = $"Kind: {form.Kind}, "
                            + $" Start: {form.Start}, "
                            + $" End: {form.End}, "
                            + $" Num Children: {form.NumChildren}"
                            + $" Accepted: {form.Accept}"
            };
                
            await dlg.ShowDialog( this );
        }

        public void OnViewChart()
        {
            new ChartWindow().Show();
        }

        public void OnViewMessageBox()
        {
            var dlg = new MessageBox {
                Message = "This is a dialog in Avalonia, this is a dialog in Avalonia, "
                    + "this is a dialog in Avalonia, "
                    + "this is a dialog in Avalonia."
            };
                
            dlg.ShowDialog( this );
        }
    }
}
