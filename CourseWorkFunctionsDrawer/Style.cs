using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CourseWorkFunctionsDrawer
{
    class Style
    {
        public static Dictionary<string, DoubleCollection> dashPatterns = new Dictionary<string, DoubleCollection>();
        static Style()
        {
            dashPatterns.Add("None", null);
            dashPatterns.Add("Dashed", new DoubleCollection(new double[]{5, 5 }));
            dashPatterns.Add("Dotted", new DoubleCollection(new double[]{2, 4 }));
            dashPatterns.Add("Dash-Dotted", new DoubleCollection(new double[] { 5, 1, 1, 5 }));
        }

        public Style(double thickness, string dashPattern, Color color)
        {
            Thickness = thickness;
            DashPattern = dashPatterns[dashPattern];
            Brush = new SolidColorBrush(color);
        }

        public double Thickness { get; set; }
        public DoubleCollection DashPattern { get; set; }
        public Brush Brush { get; set; }
    }
}
