using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace ArknightsPublicRecruitTool
{
    class TagCategory
    {
        private Button[] m_buttons;
        private Dictionary<Button, bool> m_select_map;
        private StackPanel m_panel;
        private Grid m_grid;
        private Label m_lable;
        private string m_category;
        private string[] m_tags;
        private SolidColorBrush FontColor => new SolidColorBrush(Colors.White);

        private IList<string> m_reference_to_tags;
        public string Category => m_category;
        public string[] Tags => m_tags;
        public UIElement Base => m_grid;
        public TagCategory(IList<string> TagsReference, string Cat, string[] TagSeq)
        {
            m_tags = TagSeq;
            m_category = Cat;
            m_select_map = new Dictionary<Button, bool>(TagSeq.Length);
            m_reference_to_tags = TagsReference;
            m_grid = new Grid()
            {
                Background = new SolidColorBrush(Colors.Transparent)
            };
            m_grid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(0, GridUnitType.Auto)
            });
            m_grid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(0, GridUnitType.Auto)
            });

            m_lable = new Label()
            {
                Content = Cat,
                MinWidth = 60,
                MinHeight = 25,
                Foreground = FontColor,
                Background = new SolidColorBrush(Colors.Transparent),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            m_lable.SetValue(Grid.ColumnProperty, 0);
            m_grid.Children.Add(m_lable);

            m_panel = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            m_panel.SetValue(Grid.ColumnProperty, 1);
            m_grid.Children.Add(m_panel);

            m_buttons = new Button[m_tags.Length];
            for (int i = 0; i < TagSeq.Length; ++i)
            {
                m_buttons[i] = new Button()
                {
                    Margin = new Thickness(5),
                    MinWidth = 60,
                    MinHeight = 25,
                    Content = TagSeq[i],
                    Foreground = FontColor,
                    Style = Application.Current.Resources["ButtonStyle"] as Style
                };
                m_buttons[i].MouseEnter += MouseEnter;
                m_buttons[i].MouseLeave += MouseLeave;
                m_buttons[i].LostFocus += LostFocus;
                m_buttons[i].GotFocus += GotFocus;
                m_buttons[i].Click += TagClick;
                VisualStateManager.GoToState(m_buttons[i], "Normal", false);
                m_select_map.Add(m_buttons[i], false);
                m_panel.Children.Add(m_buttons[i]);
            }
        }
        private void LostFocus(object sender, RoutedEventArgs e)
        {
            if (!m_select_map[sender as Button])
                VisualStateManager.GoToState(sender as FrameworkElement, "Normal", false);
            else
                VisualStateManager.GoToState(sender as FrameworkElement, "Selected", false);
        }
        private void GotFocus(object sender, RoutedEventArgs e)
        {
            if (!m_select_map[sender as Button])
                VisualStateManager.GoToState(sender as FrameworkElement, "PointerOver", true);
            else
                VisualStateManager.GoToState(sender as FrameworkElement, "SelectedAndPointerOver", true);
        }
        private void MouseLeave(object sender, MouseEventArgs e)
        {
            if (!m_select_map[sender as Button])
                VisualStateManager.GoToState(sender as FrameworkElement, "Normal", true);
            else
                VisualStateManager.GoToState(sender as FrameworkElement, "Selected", true);

        }
        private void MouseEnter(object sender, MouseEventArgs e)
        {
            if (!m_select_map[sender as Button])
                VisualStateManager.GoToState(sender as FrameworkElement, "PointerOver", true);
            else
                VisualStateManager.GoToState(sender as FrameworkElement, "SelectedAndPointerOver", true);
        }
        private void TagClick(object sender, RoutedEventArgs e)
        {
            var target = e.Source as Button;
            if (m_select_map[target])
            {
                m_reference_to_tags.Remove(target.Content as string);
                m_select_map[target] = false;
            }
            else
            {
                m_reference_to_tags.Add(target.Content as string);
                m_select_map[target] = true;
            }
        }
    }
}
