using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Arknights;

namespace ArknightsPublicRecruitTool
{
    class ResultItem
    {
        public static Brush[] RankBrushes = new Brush[]
        {
            Application.Current.Resources["LowRank"] as Brush,
            Application.Current.Resources["LowRank"] as Brush,
            Application.Current.Resources["LowRank"] as Brush,
            Application.Current.Resources["RankFour"] as Brush,
            Application.Current.Resources["RankFive"] as Brush,
            Application.Current.Resources["RankSix"] as Brush
        };
        private ListBoxItem m_item;
        private string[] m_tags;
        private Operator m_target;
        private SolidColorBrush FontColor => new SolidColorBrush(Colors.White);
        public ListBoxItem Base => m_item;
        public IEnumerable<string> Tags => m_tags;
        public Operator Target => m_target;
        public ResultItem(string[] tags, Operator target)
        {
            m_tags = tags;
            m_target = target;
            StringBuilder content = new StringBuilder();
            content.AppendFormat("级别:{0} 代号: {1} ->", target.Rank, target.CodeName);
            foreach (var tag in tags)
                content.AppendFormat(" {0}", tag);
            m_item = new ListBoxItem()
            {
                Content = content.ToString(),
                Background = RankBrushes[target.Rank - 1],
                Foreground = FontColor,
                MinHeight = 30,
                Margin = new Thickness(0, 1, 0, 1),
                BorderThickness = new Thickness(2),
                Style = Application.Current.Resources["ItemStyle"] as Style,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            m_item.MouseEnter += (sender, e) =>
            {
                VisualStateManager.GoToState(sender as FrameworkElement, "MouseEnter", true);
            };
            m_item.MouseLeave += (sender, e) =>
            {
                VisualStateManager.GoToState(sender as FrameworkElement, "MouseLeave", true);
            };
        }
    }
}
