using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }

        private RecruitAPI API;
        private TagCategory[] Categories;
        void Initialize()
        {
            AppBase.Background = Application.Current.Resources["BackgroundBrush"] as SolidColorBrush;
            ExitButton.Click += (sender, e) => { Close(); };
            MouseMove += (sender, e) => { if (e.LeftButton == MouseButtonState.Pressed) DragMove(); };
            API = new RecruitAPI()
            {
                API = new Recruit(File.ReadAllText(@"ConfigJson/OperatorSet.json", Encoding.UTF8),
                                  File.ReadAllText(@"ConfigJson/TagCategories.json", Encoding.UTF8),
                                  one => one.Way.Contains("公开招募"))
            };
            API.Notify += UpdateNotify;

            Categories = (from category in API.Categories
                      select new TagCategory(API.Tags, category.Key, category.Value)).ToArray();
            foreach (var category in Categories)
                TagPanel.Children.Add(category.Base);
        }
        private void UpdateNotify(KeyValuePair<string[], Operator[]>[] Update)
        {
            var query = new List<KeyValuePair<string[], Operator>>();
            foreach (var pair in Update)
                query.AddRange(from item in pair.Value select new KeyValuePair<string[], Operator>(pair.Key, item));
            var items = from item in query select new ResultItem(item.Key, item.Value);
            ResultList.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                ResultList.Items.Clear();
                foreach (var item in items)
                {
                    ResultList.Items.Add(item.Base);
                }
            }));
        }
    }
}
