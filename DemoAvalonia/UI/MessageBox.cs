using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;


namespace DemoAvalonia.UI;

public class MessageBox: Window
{
    public MessageBox()
    {
        this._lblMessage = new Label {
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness( 5 )
        };
        this.Build();
    }
    
    void Build()
    {
        var pnlMain = new DockPanel {
            Margin = new Thickness( 5 )
        };
        var btOk = new Button {
            Content = "Ok",
            Margin = new Thickness(5 ),
            HorizontalAlignment = HorizontalAlignment.Right
        };

        btOk.Click += (_, _) => this.Close();

        pnlMain.Children.AddRange( new Control[]{ _lblMessage, btOk } );
        this.Content = pnlMain;
    }

    public string Message {
        get => ( (string?) this._lblMessage.Content ) ?? "";
        set {
            this._lblMessage.Content = value;
            this.SizeToContent = SizeToContent.WidthAndHeight;
        }
    }

    private readonly Label _lblMessage;
}
