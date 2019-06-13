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
        private KeyValuePair<string[], Operator[]>[] QueryResult = null;
        private object SyncRoot = new object();
        private object QuerySync = new object();
        private bool View = false;
        private void Initialize()
        {
            AppBase.Background = Application.Current.Resources["BackgroundBrush"] as SolidColorBrush;
            ExitButton.Click += (sender, e) => { Close(); };
            ConvertButton.Click += (sender, e) =>
            {
                View = !View;
                RefreshView();       
            };
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
        static string ContactWith(IEnumerable<string> Sequence, string Seperator)
        {
            var builder = new StringBuilder();
            string first = Sequence.FirstOrDefault();
            if (first == default)
                return default;
            Sequence = Sequence.Skip(1);
            builder.Append(first);
            foreach (var each in Sequence)
            {
                builder.Append(Seperator);
                builder.Append(each);
            }
            return builder.ToString();
        }
        private void UpdateNotify(KeyValuePair<string[], Operator[]>[] Update)
        {
            lock (QuerySync) QueryResult = Update;
            RefreshView();
        }
        private void RefreshView()
        {
            if (View)
                GenerateTagView();
            else
                GenerateOperatorView();
        }
        private void GenerateOperatorView()
        {
            var table = new Dictionary<Operator, List<string[]>>();
            lock (QuerySync)
            {
                if (QueryResult == null || QueryResult.Length == 0)
                    return;
                foreach (var query in QueryResult)
                {
                    foreach (var op in query.Value)
                    {
                        if (!table.ContainsKey(op))
                            table.Add(op, new List<string[]>());
                        table[op].Add(query.Key);
                    }
                }
            }
            var items = from pair in table
                        select new ResultGroup(
                        string.Format("星级: {0} 代号: {1}", pair.Key.Rank, pair.Key.CodeName),
                        (from set in pair.Value select ContactWith(set, " ")).ToArray(),
                        ResultItem.RankBrushes[pair.Key.Rank - 1]);
            ResultList.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                lock(SyncRoot)
                {
                    ResultList.Items.Clear();
                    foreach (var item in items)
                        ResultList.Items.Add(item.Base);
                }
            }));
        }
        private void GenerateTagView()
        {
            var items = new List<ResultGroup>();
            lock (QuerySync)
            {
                if (QueryResult == null || QueryResult.Length == 0)
                    return;
                foreach (var query in QueryResult)
                    items.Add(new ResultGroup(
                        ContactWith(query.Key, " "),
                        (from op in query.Value select string.Format("星级: {0} 代号: {1}", op.Rank, op.CodeName)).ToArray(),
                        ResultItem.RankBrushes[query.Value.FirstOrDefault() == null ? 0 : query.Value.First().Rank - 1]
                        ));
            }
            ResultList.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                lock (SyncRoot)
                {
                    ResultList.Items.Clear();
                    foreach (var item in items)
                        ResultList.Items.Add(item.Base);
                }
            }));
        }
    }
}
