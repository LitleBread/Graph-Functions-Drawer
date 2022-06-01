using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkFunctionsDrawer
{
    class Graph
    {
        public string Function { get; private set; }
        public string CoordinateSystem { get; private set; }
        public string Interval { get; private set; }
        public Point Zero { get; set; }
        public bool ToShow
        {
            get
            { return toShow; }
            set
            {
                if (value)
                {
                    PlaneToDraw.Children.Add(Line);
                }
                else
                    PlaneToDraw.Children.Remove(Line);
                toShow = value;
            }
        }

        public Panel PlaneToDraw { get; set; }
        public Polyline Line { get; private set; }

        public Style Style { get; set; }
        public List<Point> Points { get; private set; } = new List<Point>();
        Func<double, double> func;
        private bool toShow = true;

        public double ScaleFactor { get; set; } = 1;
        bool isDecart;
        bool isDrawn = false;
        double min, max, step;



        public Graph(string funcString, bool isDecart, Style style, Panel grid) : this(funcString, isDecart, style)
        {
            PlaneToDraw = grid;
        }

        public Graph(string funcString, bool isDecart, Style style)
        {
            this.Function = funcString;
            func = Compiler.GetDelegate(StupidParser.Parse(funcString));
            Points = new List<Point>();
            this.isDecart = isDecart;
            CoordinateSystem = isDecart ? "Декартовая" : "Полярная";
            this.Style = style;
        }


        public async void Draw(Point zero, double min, double max, double step, double scaleFactor)
        {
            this.Zero = zero;
            this.ScaleFactor *= scaleFactor;
            this.min = min;
            this.max = max;
            this.step = step;

            if (Function.ToLower().Contains("log") && min <= 0)
                min = 1e-6;
            if (!isDrawn)
            {
                await Task.Run(() =>
                {
                    if (!isDecart)
                    {
                        double x, y;
                        min *= Math.PI / 180;
                        max *= Math.PI / 180;
                        step *= Math.PI / 180;

                        for (double f = min; f <= max + step; f += step)
                        {
                            ToDekart(func(f), f, out x, out y);
                            Points.Add(new Point(-x * scaleFactor, y * scaleFactor));
                        }
                    }
                    else
                    {
                        for (double x = min; x <= max + step; x += step)
                        {
                            Points.Add(new Point(x * scaleFactor, func(x) * scaleFactor));
                        }
                    }
                });

                Line = new Polyline();
                Line.Stroke = Style.Brush;
                Line.StrokeThickness = Style.Thickness;
                Line.StrokeDashArray = Style.DashPattern;
                for (int i = 0; i < Points.Count; i++)
                {
                    Line.Points.Add(new Point(Points[i].X * 10 + zero.X, Points[i].Y * 10 + zero.Y));
                }

                PlaneToDraw.Children.Add(Line);
                isDrawn = true;
            }
            else
            {

                for (int i = 0; i < Line.Points.Count; i++)
                {
                    Line.Points[i] = new Point(Points[i].X * 10 + zero.X, Points[i].Y * 10 + zero.Y);
                }

            }
            //MessageBox.Show("Done Drawing");
        }

        private void ToDekart(double r, double f, out double x, out double y)
        {
            x = r * Math.Cos(f);
            y = r * Math.Sin(f);
        }

        public void Scale(double sf, Point p)
        {

            this.ScaleFactor += 1 - sf;


            for (int i = 0; i < Points.Count; i++)
            {
                Points[i] = new Point((Points[i].X - p.X) * sf + p.X * sf, (Points[i].Y - p.Y) * sf + p.Y * sf);
            }


        }


        public void RemoveGraph()
        {
            PlaneToDraw.Children.Remove(Line);
        }

    }
}
