// DemoAvalonia (c) 2021/23 Baltasar MIT License <jbgarcia@uvigo.es>


namespace DemoAvalonia.UI;


using System;
using System.Data;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

using Avalonia;
using Avalonia.Media;
using Avalonia.Controls;


/// <summary>Support for charts</summary>
public class Chart: Control {
    /// <summary>Select the type of chart.</summary>
    public enum ChartType { Lines, Bars }

    /// <summary>Support for fonts</summary>
    public struct Font {
        public Font(double fontSize)
        {
            this.Family = FontFamily.Default;
            this.Style = FontStyle.Normal;
            this.Weight = FontWeight.Normal;
            this.Size = fontSize;
        }
        
        public FontFamily Family { get; set; }
        public FontStyle Style { get; set; }
        public FontWeight Weight { get; set; }
        public double Size { get; set; }
    }

    /// <summary>Parameterless constructor. Change the properties.</summary>
    public Chart()
    {
        // Create the other properties
        this._values = new List<int>();
        this._labels = new List<string>();
        this.LegendX = "Value";
        this.LegendY = "Date";
        this._normalizedData = Array.Empty<int>();
        this.FrameWidth = 50;
        this.Type = ChartType.Lines;
        this.AxisPen = new Pen( Brushes.Black, 4 );
        this.DataPen = new Pen( Brushes.Red, 2 );
        this.DataFont = new Font( 12 ) { Family = FontFamily.Default };
        this.LabelFont = new Font( 12 ) { Family = FontFamily.Default };
        this.LegendFont = new Font( 12 ) { Family = FontFamily.Default };
        this.DrawGrid = true;
        this.GridPen = new Pen( Brushes.Gray );
        this.Background = Brushes.White;
        this.LegendBrush = Brushes.Black;
    }

    public override void Render(DrawingContext graphs)
    {
        base.Render( graphs );
        this.Draw( graphs );
    }

    public void Draw()
    {
        this.InvalidateVisual();
    }

    /// <summary>Redraws the chart</summary>
    private void Draw(DrawingContext graphs)
    {
        this._graphics = graphs;
        
        this.ClearCanvas();
        
        // Frame
        this.DrawRectangle(
            new Pen( this.Background, this.FrameWidth ),
            0, 0,
            this.Width,
            this.Height );
        
        // Chart components
        this.DrawAxis();
        this.DrawData();
        this.DrawLegends();
    }
    
    private void DrawLegends()
    {
        // Legend for X
        this.DrawString(
                this.LegendFont,
                this.LegendBrush,
                ( this.Width / 2 )
                  - ( (int) this.DataOrgPosition.X ) / 2,
                (int) ( this.FramedEndPosition.Y + 20 ),
                this.LegendX,
                vertical: false );

        // Legend for Y
        this.DrawString(
                this.LegendFont,
                this.LegendBrush,
                (int) - this.FramedEndPosition.Y,
                (int) this.FramedOrgPosition.X - 20,
                this.LegendY,
                vertical: true );
    }
    
    private void DrawData()
    {
        this.NormalizeData();

        int numValues = this._normalizedData.Length;
        int baseLine = (int) this.DataOrgPosition.Y;
        int xGap = (int) ( (double) this.GraphWidth / ( numValues + 1 ) );
        
        // Start drawing point
        var currentPoint = new Point(
                        x: this.DataOrgPosition.X + xGap,
                        y: baseLine - this._normalizedData[ 0 ] );

        // Next points
        for(int i = 0; i < numValues; ++i) {
            string tag = this._values[ i ].ToString();
            
            var nextPoint = new Point(
                x: this.DataOrgPosition.X + ( xGap * ( i + 1 ) ),
                y: baseLine - this._normalizedData[ i ]
            );

            if ( this.Type == ChartType.Bars ) {
                currentPoint = new Point( nextPoint.X, baseLine );
            }
            
            this.DrawLine( this.DataPen, currentPoint, nextPoint );
            this.DrawString(
                        font: this.DataFont,
                        this.DataPen.Brush ?? Brushes.Black,
                        x: (int) ( nextPoint.X - ( this.DataPen.Thickness / 2 ) ),
                        y: (int) ( nextPoint.Y - this.DataPen.Thickness ),
                        msg: tag );
            
            currentPoint = nextPoint;
        }
    }
    
    private void DrawAxis()
    {
        // Grid
        if ( this.DrawGrid ) {
            int numValues = this._values.Count;
            int xGap = (int) ( (double) this.GraphWidth / ( numValues + 1 ) );
            int yGap = this.GraphHeight / 10;
            
            // Labels available?
            if ( this._labels.Count == 0 ) {
                this._labels.AddRange( 
                    Enumerable.Range( 1, this._values.Count )
                        .Select( x => Convert.ToString( x ) ) );
            }
            
            // Vertical lines going right
            for (int i = 0; i < ( numValues + 1 ); ++i) {
                int columnPos = (int) this.DataOrgPosition.X + ( xGap * i );

                // Y axis (1 per value)
                this.DrawLine(
                    pen: this.GridPen,
                    x1: columnPos,
                    y1: (int) this.DataOrgPosition.Y,
                    x2: columnPos,
                    y2: (int) this.FramedOrgPosition.Y );
                
                // The label
                if ( ( i - 1 ) >= 0
                  && ( i - 1 ) < this._labels.Count )
                {
                    this.DrawString(
                        font: this.LabelFont,
                        this.LegendBrush,
                        x: columnPos,
                        y: (int) ( this.FramedEndPosition.Y + 2 ),
                        msg: this._labels[ i - 1 ]
                    );
                }
            }

            // Horizontal lines going up
            for (int i = 0; i < 10; ++i) {
                int rowPos = (int) this.DataOrgPosition.Y - ( yGap * i );
                
                // X axis (tenth line)
                this.DrawLine(
                    this.GridPen,
                    (int) this.DataOrgPosition.X,
                    rowPos,
                    (int) this.FramedEndPosition.X,
                    rowPos );
            }
        }
        
        // Y axis
        this.DrawLine( this.AxisPen,
                       (int) this.FramedOrgPosition.X,
                       (int) this.FramedOrgPosition.Y,
                       (int) this.FramedOrgPosition.X,
                       (int) this.FramedEndPosition.Y );
                                 
        // X axis
        this.DrawLine( this.AxisPen,
                       (int) this.FramedOrgPosition.X,
                       (int) this.FramedEndPosition.Y,
                       (int) this.FramedEndPosition.X,
                       (int) this.FramedEndPosition.Y );
    }
    
    private void NormalizeData()
    {
        int numValues = this._values.Count;
        int maxValue = this._values.Max();
        this._normalizedData = this._values.ToArray();

        for(int i = 0; i < numValues; ++i) {
            this._normalizedData[ i ] =
                                ( this._values[ i ] * this.GraphHeight ) / maxValue;
        }
        
        return;
    }
    
    /// <summary>
    /// Gets or sets the values used as data.
    /// </summary>
    /// <value>The values.</value>
    public IEnumerable<int> Values {
        get => this._values.ToArray();
        set {
            this._values.Clear();
            this._values.AddRange( value );
        }
    }
    
    /// <summary>
    /// Gets or sets the labels used for the data
    /// under the X axis.
    /// </summary>
    /// <value>The labels.</value>
    public IEnumerable<string> Labels {
        get => this._labels.ToArray();

        set {
            this._labels.Clear();
            this._labels.AddRange( value );
        }
    }
    
    /// <summary>
    /// Gets the framed origin.
    /// </summary>
    /// <value>The origin <see cref="Point"/>.</value>
    public Point DataOrgPosition {
        get {
            int margin = (int) ( this.AxisPen.Thickness * 2 );
            
            return new Point(
                this.FramedOrgPosition.X + margin,
                this.FramedEndPosition.Y - margin );
        }
    }
    
    /// <summary>
    /// Gets or sets the width of the frame around the chart.
    /// </summary>
    /// <value>The width of the frame.</value>
    public int FrameWidth {
        get; set;
    }
    
    /// <summary>
    /// Gets the framed origin.
    /// </summary>
    /// <value>The origin <see cref="Point"/>.</value>
    public Point FramedOrgPosition => new ( this.FrameWidth, this.FrameWidth );
    
    /// <summary>
    /// Gets the framed end.
    /// </summary>
    /// <value>The end <see cref="Point"/>.</value>
    public Point FramedEndPosition => new ( this.Width - this.FrameWidth,
                                            this.Height - this.FrameWidth );
    
    /// <summary>
    /// Gets the width of the graph.
    /// </summary>
    /// <value>The width of the graph.</value>
    public int GraphWidth => this.Width - ( this.FrameWidth * 2 );
    
    /// <summary>
    /// Gets the height of the graph.
    /// </summary>
    /// <value>The height of the graph.</value>
    public int GraphHeight => this.Height - ( this.FrameWidth * 2 );

    /// <summary>
    /// Gets or sets the pen used to draw the axis.
    /// </summary>
    /// <value>The axis <see cref="Pen"/>.</value>
    public Pen AxisPen {
        get; set;
    }
    
    /// <summary>
    /// Gets or sets the pen used to draw the data.
    /// </summary>
    /// <value>The data <see cref="Pen"/>.</value>
    public Pen DataPen {
        get; set;
    }
    
    /// <summary>
    /// Gets or sets the pen used to draw the grid.
    /// </summary>
    /// <value>The grid <see cref="Pen"/>.</value>
    public Pen GridPen {
        get; set;
    }
    
    /// <summary>
    /// Gets or sets the font for data.
    /// </summary>
    /// <value>The data <see cref="Font"/>.</value>
    public Font DataFont {
        get; set;
    }
    
    /// <summary>
    /// Gets or sets the font for labels.
    /// </summary>
    /// <value>The label <see cref="Font"/>.</value>
    public Font LabelFont {
        get; set;
    }
    
    /// <summary>
    /// Gets or sets the legend for the x axis.
    /// </summary>
    /// <value>The legend for axis x.</value>
    public string LegendX {
        get; set;
    }
    
    /// <summary>
    /// Gets or sets the legend for the y axis.
    /// </summary>
    /// <value>The legend for axis y.</value>
    public string LegendY {
        get; set;
    }
    
    /// <summary>
    /// Gets or sets the font for legends.
    /// </summary>
    /// <value>The <see cref="Font"/> for legends.</value>
    public Font LegendFont {
        get; set;
    }

    /// <summary>Get or sets whether to show a grid or not.</summary>
    public bool DrawGrid {
        get; set;
    }

    /// <summary>The color for the text in the legend.</summary>
    public IBrush LegendBrush {
        get; set;
    }
    
    /// <summary>
    /// Gets or sets the type of the chart.
    /// </summary>
    /// <value>The <see cref="ChartType"/>.</value>
    public ChartType Type {
        get; set;
    }

    public IBrush Background { get; set; }

    private new int Width => (int) this.Bounds.Width;

    private new int Height => (int) this.Bounds.Height;

    private void DrawRectangle(IPen pen, int x, int y, int width, int height)
    {
        this.Graphics.DrawRectangle(
                            pen.Brush,
                            pen,
                            new Rect( x, y, width, height ) );
    }

    private void DrawLine(IPen pen, int x1, int y1, int x2, int y2)
    {
        this.DrawLine( pen, new Point( x1, y1 ), new Point( x2, y2 ) );
    }
    
    private void DrawLine(IPen pen, Point p1, Point p2)
    {
        this.Graphics.DrawLine( pen, p1, p2 );
    }
    
    private void DrawString(
                    Font font, IBrush color, int x, int y, string msg,
                    bool vertical = false)
    {
        // Left to right, or viceversa
        var flow = FlowDirection.LeftToRight;

        if ( CultureInfo.CurrentCulture.TextInfo.IsRightToLeft ) {
            flow = FlowDirection.RightToLeft;
        }
        
        // Font
        var typeface = new Typeface( font.Family, font.Style, font.Weight );
        
        // Formatted text
        var text = new FormattedText(
                            msg,
                            CultureInfo.CurrentCulture,
                            flow,
                            typeface, 
                            font.Size,
                            color );
        
        // Vertical
        if ( vertical ) {
            this.Graphics.PushTransform(
                            new RotateTransform( 270 ).Value );
        }

        this.Graphics.DrawText( text, new Point( x, y ) );
    }

    private void ClearCanvas()
    {
        this.Graphics.DrawRectangle(
            this.Background,
            null,
            new Rect( 0, 0, this.Width, this.Height ) );
    }
    
    private DrawingContext Graphics {
        get {
            if ( this._graphics is null ) {
                throw new NoNullAllowedException( "graphics (drawingcontext) is null" );
            }

            return this._graphics;
        }
    }
    
    private readonly List<int> _values;
    private readonly List<string> _labels;
    private DrawingContext? _graphics;
    private int[] _normalizedData;
}
