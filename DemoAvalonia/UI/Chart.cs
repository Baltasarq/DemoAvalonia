// DemoAvalonia (c) 2021 Baltasar MIT License <jbgarcia@uvigo.es>


namespace DemoAvalonia.UI {
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Avalonia;
    using Avalonia.Media;
    using Avalonia.Layout;
    using Avalonia.Controls;
    using Avalonia.Controls.Shapes;
    
    
    public class Chart: Canvas {
        public enum ChartType { Lines, Bars }


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

        public Chart()
        {
            this.values = new List<int>();
            this.labels = new List<string>();
            this.LegendX = "Value";
            this.LegendY = "Date";
            this.normalizedData = Array.Empty<int>();
            this.FrameWidth = 50;
            this.Type = ChartType.Lines;
            this.AxisPen = new Pen( Brushes.Black, 4 );
            this.DataPen = new Pen( Brushes.Red, 2 );
            this.DataFont = new Font( 12 ) { Family = FontFamily.Default };
            this.LabelFont = new Font( 12 ) { Family = FontFamily.Default };
            this.LegendFont = new Font( 12 ) { Family = FontFamily.Default };
            this.Background = Brushes.White;
            this.DrawGrid = true;
            this.GridPen = new Pen( Brushes.Gray );
        }
        
        public override void Render(DrawingContext context)
        {
            base.Render( context );

            this.Draw();
        }
        
        /// <summary>
        /// Redraws the chart
        /// </summary>
        public void Draw()
        {
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
        
        void DrawLegends()
        {
            this.DrawString(
                    this.LegendFont,
                    (int) this.DataOrgPosition.X,
                    (int) ( this.FramedEndPosition.Y + 20 ),
                    this.LegendX,
                    vertical: false,
                    horizontalAlignment: HorizontalAlignment.Center,
                    width: this.GraphWidth );

            this.DrawString(
                    this.LegendFont,
                    -FrameWidth / 2,
                    (int) ( this.FramedOrgPosition.Y
                            + ( ( this.FramedEndPosition.Y
                              - this.FramedOrgPosition.Y ) / 2 ) ),
                    this.LegendY,
                    vertical: true );
        }
        
        void DrawData()
        {
            this.NormalizeData();

            int numValues = this.normalizedData.Length;
            int baseLine = (int) this.DataOrgPosition.Y;
            int xGap = (int) ( (double) this.GraphWidth / ( numValues + 1 ) );
            
            // Start drawing point
            var currentPoint = new Point(
                            x: this.DataOrgPosition.X + xGap,
                            y: baseLine - this.normalizedData[ 0 ] );

            // Next points
            for(int i = 0; i < numValues; ++i) {
                string tag = this.values[ i ].ToString();
                
                var nextPoint = new Point(
                    x: this.DataOrgPosition.X + ( xGap * ( i + 1 ) ),
                    y: baseLine - this.normalizedData[ i ]
                );

                if ( this.Type == ChartType.Bars ) {
                    currentPoint = new Point( nextPoint.X, baseLine );
                }
                
                this.DrawLine( this.DataPen, currentPoint, nextPoint );
                this.DrawString(
                            font: this.DataFont,
                            x: (int) ( nextPoint.X - ( this.DataPen.Thickness / 2 ) ),
                            y: (int) ( nextPoint.Y - this.DataPen.Thickness ),
                            msg: tag );
                
                currentPoint = nextPoint;
            }
        }
        
        void DrawAxis()
        {
            // Grid
            if ( this.DrawGrid ) {
                int numValues = this.values.Count;
                int xGap = (int) ( (double) this.GraphWidth / ( numValues + 1 ) );
                int yGap = this.GraphHeight / 10;
                
                // Labels available?
                if ( this.labels.Count == 0 ) {
                    this.labels.AddRange( 
                        Enumerable.Range( 1, this.values.Count )
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
                      && ( i - 1 ) < this.labels.Count )
                    {
                        this.DrawString(
                            font: this.LabelFont,
                            x: columnPos,
                            y: (int) ( this.FramedEndPosition.Y + 2 ),
                            msg: this.labels[ i - 1 ]
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
        
        void NormalizeData()
        {
            int numValues = this.values.Count;
            int maxValue = this.values.Max();
            this.normalizedData = this.values.ToArray();

            for(int i = 0; i < numValues; ++i) {
                this.normalizedData[ i ] =
                                    ( this.values[ i ] * this.GraphHeight ) / maxValue;
            }
            
            return;
        }
        
        /// <summary>
        /// Gets or sets the values used as data.
        /// </summary>
        /// <value>The values.</value>
        public IEnumerable<int> Values {
            get => this.values.ToArray();

            set {
                this.values.Clear();
                this.values.AddRange( value );
            }
        }
        
        /// <summary>
        /// Gets or sets the labels used for the data
        /// under the X axis.
        /// </summary>
        /// <value>The labels.</value>
        public IEnumerable<string> Labels {
            get => this.labels.ToArray();

            set {
                this.labels.Clear();
                this.labels.AddRange( value );
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
        
        /// <summary>
        /// Gets or sets the type of the chart.
        /// </summary>
        /// <value>The <see cref="ChartType"/>.</value>
        public ChartType Type {
            get; set;
        }

        new int Width => (int) this.Bounds.Width;

        new int Height => (int) this.Bounds.Height;

        void DrawRectangle(Pen pen, int x, int y, int width, int height)
        {
            var rect = new Rectangle {
                Width = width, Height = height,
                Stroke = pen.Brush,
                StrokeThickness = pen.Thickness,
            };
            
            SetLeft( rect, x );
            SetTop( rect, y );
            this.Children.Add( rect );
        }

        void DrawLine(Pen pen, int x1, int y1, int x2, int y2)
        {
            this.DrawLine( pen, new Point( x1, y1 ), new Point( x2, y2 ) );
        }
        
        void DrawLine(Pen pen, Point p1, Point p2)
        {
            var line = new Line {
                StartPoint = p1,
                EndPoint = p2,
                Stroke = pen.Brush,
                StrokeThickness = pen.Thickness
            };
    
            this.Children.Add( line );
        }
        
        void DrawString(Font font, int x, int y, string msg,
                        bool vertical = false,
                        HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left,
                        int width = Int32.MaxValue )
        {
            var lbl = new Label {
                Content = msg,
                HorizontalContentAlignment = horizontalAlignment,
                HorizontalAlignment = horizontalAlignment,
                FontFamily = font.Family,
                FontSize = font.Size,
                FontWeight = font.Weight,
                FontStyle = font.Style
            };
            
            if ( width < Int32.MaxValue ) {
                lbl.Width = width;
            }
            
            if ( vertical ) {
                lbl.RenderTransform = new RotateTransform( 270.0 );
            }
            
            SetLeft( lbl, x );
            SetTop( lbl, y );
            this.Children.Add( lbl );
        }

        void ClearCanvas()
        {
            this.Children.Clear();
            
            var rect = new Rectangle {
                Width = this.Width, Height = this.Height,
                Stroke = this.Background,
                StrokeThickness = 1,
                Fill = this.Background
            };
            
            SetLeft( rect, 0 );
            SetTop( rect, 0 );
            this.Children.Add( rect );
        }
        
        readonly List<int> values;
        readonly List<string> labels;
        int[] normalizedData;
    }
}
