using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace ArknightsPublicRecruitTool
{
    public class ButtonStyleProperty : DependencyObject
    {
        public static Color GetPointOverBrush(DependencyObject obj)
        {
            return (Color)obj.GetValue(PointOverBrush);
        }
        public static void SetPointOverBrush(DependencyObject obj, Color value)
        {
            obj.SetValue(PointOverBrush, value);
        }
        public static readonly DependencyProperty PointOverBrush =
            DependencyProperty.RegisterAttached("PointOverBrush", typeof(Color), typeof(ButtonStyleProperty),new PropertyMetadata(Colors.White));
    }
}
