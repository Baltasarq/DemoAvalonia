// DemoAvalonia (c) 2021/23 Baltasar MIT License <jbgarcia@uvigo.es>


namespace DemoAvalonia.UI;


using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;


internal class Node {
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
        var tvOptions = this.GetControl<TreeView>( "TvOptions" );
        var opExit = this.GetControl<MenuItem>( "OpExit" );
        var opAbout = this.GetControl<MenuItem>( "OpAbout" );
        var opSimplePanel = this.GetControl<MenuItem>( "OpSimplePanel" );
        var opStackPanel = this.GetControl<MenuItem>( "OpStackPanel" );
        var opForm = this.GetControl<MenuItem>( "OpForm" );
        var opChart = this.GetControl<MenuItem>( "OpChart" );
        var opMessageBox = this.GetControl<MenuItem>( "OpMessageBox" );
        var btFirst = this.GetControl<Button>( "BtFirst" );
        var btSecond = this.GetControl<Button>( "BtSecond" );
        var btThird = this.GetControl<Button>( "BtThird" );
        var btFourth = this.GetControl<Button>( "BtFourth" );
        var btFifth = this.GetControl<Button>( "BtFifth" );
        var cbOptions = this.GetControl<ComboBox>( "CbOptions" );
        
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
        cbOptions.ItemsSource = actionTitles;
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

        tvOptions.ItemsSource = listNodes;
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
