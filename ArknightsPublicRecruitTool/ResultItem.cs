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
    class ResultGroup
    {
        private ListBoxItem m_item;
        private Grid m_base;
        private StackPanel m_panel;
        private Label m_title;
        private Label[] m_set;
        private string m_title_content;
        private string[] m_set_content;
        public string Title => m_title_content;
        public string[] Content => m_set_content;
        private SolidColorBrush FontColor => new SolidColorBrush(Colors.White);
        public UIElement Base => m_item;
        public ResultGroup(string Title, string[] Content,Brush BackgroundBrush)
        {
            m_title_content = Title;
            m_set_content = Content;
            m_base = new Grid()
            {
                Background = new SolidColorBrush(Colors.Transparent),
                Margin = new Thickness(0, 1, 0, 1)
            };
            m_base.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0, GridUnitType.Auto) });
            m_base.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0, GridUnitType.Auto) });
            m_panel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Background = new SolidColorBrush(Colors.Transparent)
            };
            m_panel.SetValue(Grid.RowProperty, 1);
            m_base.Children.Add(m_panel);

            m_title = new Label()
            {
                Content = Title,
                Background = new SolidColorBrush(Colors.Transparent),
                FontWeight = FontWeights.SemiBold,
                Foreground = FontColor,
                Height = 30,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            m_title.SetValue(Grid.RowProperty, 0);
            m_base.Children.Add(m_title);

            m_set = new Label[Content.Length];
            for (int i = 0; i < Content.Length; ++i)
            {
                m_set[i] = new Label()
                {
                    Content = Content[i],
                    Background = new SolidColorBrush(Colors.White) { Opacity = 0.3 },
                    Foreground = FontColor,
                    Height = 30,
                    Margin = new Thickness(0, 0, 0, 2),
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center
                };
                m_panel.Children.Add(m_set[i]);
            }
            m_item = new ListBoxItem()
            {
                Content = m_base,
                Background = BackgroundBrush,
                Foreground = FontColor,
                MinHeight = 30,
                Margin = new Thickness(0, 0, 0, 1),
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
