using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourseWorkFunctionsDrawer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point zero;
        Line xAxis;
        Line yAxis;
        ObservableCollection<Graph> graphs;
        Rectangle back;
        double scaleFactor = 1;
        Point oldPos;
        Regex thickness = new Regex(@"[^0a-zA-Z][0-9,]+");

        void setAxis()
        {
            xAxis.X1 = zero.X - 100000;
            xAxis.X2 = zero.X + 100000;
            xAxis.Y1 = zero.Y;
            xAxis.Y2 = zero.Y;
            yAxis.Y1 = zero.Y - 100000;
            yAxis.Y2 = zero.Y + 100000;
            yAxis.X1 = zero.X;
            yAxis.X2 = zero.X;

        }
        public MainWindow()
        {
            InitializeComponent();
            DashStyleComboBox.ItemsSource = new List<string>() { "None", "Dashed", "Dotted", "Dash-Dotted" };
            DashStyleComboBox.SelectedIndex = 0;

            ThicknessTextBox.Text = "2";
            
            graphs = new ObservableCollection<Graph>();
            back = new Rectangle();
            back.Fill = Brushes.LightGray;

            GrapghGrid.Children.Add(back);

            zero = new Point((MWindow.Width - 200) / 2, MWindow.Height / 2);
            if (xAxis == null)
            {
                xAxis = new Line();

                GrapghGrid.Children.Add(xAxis);
            }
            if (yAxis == null)
            {
                yAxis = new Line();
                GrapghGrid.Children.Add(yAxis);
            }

            xAxis.Stroke = Brushes.Black;
            xAxis.StrokeThickness = 2;

            yAxis.Stroke = Brushes.Gray;
            yAxis.StrokeThickness = 2;


            setAxis();
        }
        private Point GetPointRelatively(MouseEventArgs e, IInputElement relElem, Point relativePoint)
        {
            return new Point(e.GetPosition(relElem).X - relativePoint.X, e.GetPosition(relElem).Y - relativePoint.Y);
        }
        private void GrapghGrid_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = GetPointRelatively(e, sender as IInputElement, zero);
            point.X /= 10 * scaleFactor;
            point.Y /= -10 * scaleFactor;
            ErrorMessageTBLock.Text = string.Format("{0,2:N3} ; {1,2:N3}", point.X, point.Y);
            
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point p = GetPointRelatively(e, sender as IInputElement, oldPos);

                oldPos = e.GetPosition(sender as IInputElement);

                
                double min = 0, max = 0, dstep = 0;
                zero = new Point(zero.X + p.X, zero.Y + p.Y);
                setAxis();
                foreach (var item in graphs)
                {
                    item.Zero = zero;
                    
                    double.TryParse(minVal.Text.Replace(".", ","), out min);
                    double.TryParse(maxVal.Text.Replace(".", ","), out max);
                    double.TryParse(step.Text.Replace(".", ","), out dstep); 
                    (item as Graph).Draw(zero, min, max, dstep, scaleFactor);
                }

            }
        }

        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            double min = 0, max = 0, dstep = 0;

            double.TryParse(minVal.Text.Replace(".", ","), out min);
            double.TryParse(maxVal.Text.Replace(".", ","), out max);
            double.TryParse(step.Text.Replace(".", ","), out dstep);
            double thickness = -1;



            double.TryParse(ThicknessTextBox.Text.Replace(".", ","), out thickness);

            if (thickness <= 0)
                thickness = 2;
            Color color;
            if (ColorPicker.SelectedColor != null)
                color = (Color)ColorPicker.SelectedColor;
            else
                color = Colors.LightCoral;
            Style style = new Style(thickness, DashStyleComboBox.Text, color);

            string func = GraphEnterTBox.Text;
            Graph graph = null;
            bool isDec = (bool)Decart.IsChecked;

            graph = new Graph(func, isDec, style);


            graph.PlaneToDraw = GrapghGrid;
            graph.Draw(zero, min, max, dstep, scaleFactor);
            graphs.Add(graph);
            GraphsList.ItemsSource = graphs;

        }

        private void GrapghGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            oldPos = e.GetPosition(sender as IInputElement);
            Point point = GetPointRelatively(e, sender as IInputElement, zero);
            point.X /= 10 * scaleFactor;
            point.Y /= -10 * scaleFactor;
            ErrorMessageTBLock.Text = string.Format("{0,2:N3} ; {1,2:N3}", point.X, point.Y);
        }
        
        private void GrapghGrid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double sf = 1 + e.Delta / 720.0;
            scaleFactor *= sf;
            Point mousePos = e.GetPosition(GrapghGrid);

            Point p = GetPointRelatively(e, sender as IInputElement, zero);
            Point p1 = e.GetPosition(GrapghGrid);


            zero = new Point((zero.X - p1.X) * sf + p1.X, (zero.Y - p1.Y) * sf + p1.Y);




            setAxis();
            double min = 0, max = 0, dstep = 0;
            foreach (var item in graphs)
            {
               
                item.Scale(sf, p);

                double.TryParse(minVal.Text.Replace(".", ","), out min);
                double.TryParse(maxVal.Text.Replace(".", ","), out max);
                double.TryParse(step.Text.Replace(".", ","), out dstep);
                item.Draw(zero, min, max, dstep, sf);
            }


        }

        private void deleteGraphBtn_Click(object sender, RoutedEventArgs e)
        {
            if (GraphsList.SelectedItem == null)
                return;
            (GraphsList.SelectedItem as Graph).RemoveGraph();
            graphs.Remove(GraphsList.SelectedItem as Graph);

        }

        private void GraphsList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (GraphsList.SelectedItem == null)
                    return;
                (GraphsList.SelectedItem as Graph).RemoveGraph();
                graphs.Remove(GraphsList.SelectedItem as Graph);
            }
        }

    }
}
