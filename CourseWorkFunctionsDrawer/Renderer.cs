using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Shapes;

namespace CourseWorkFunctionsDrawer
{
    class Renderer
    {
        List<Polyline> polylines = new List<Polyline>();
        public void Render(Panel field, Point leftTopCorner, Point rightBottomCorner, Point zero, Graph graph, double scaleFactor)
        {
            foreach (var item in polylines)
            {
                field.Children.Remove(item);
            }

            Polyline polyline = new Polyline();
            polyline.Stroke = graph.Style.Brush;
            polyline.StrokeDashArray = graph.Style.DashPattern;
            polyline.StrokeThickness = graph.Style.Thickness;

            polylines.Add(polyline);
            foreach (var item in graph.Points)
            {
                if (item.X > leftTopCorner.X && item.X < rightBottomCorner.X &&
                    item.Y > leftTopCorner.Y && item.Y < rightBottomCorner.Y)
                {
                    polyline.Points.Add(new Point(item.X * scaleFactor + zero.X , item.Y * scaleFactor + zero.Y ));
                }
            }
            field.Children.Add(polyline);
        }
    }
}
