using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Arknights;

namespace ArknightsPublicRecruitTool
{
    public class RecruitAPI
    {
        private ObservableCollection<string> m_tags;
        public IList<string> Tags => m_tags;
        public Recruit API { get; set; }
        public Dictionary<string, string[]> Categories => API.TagCategories;

        public delegate void ResultDoneNotify(KeyValuePair<string[], Operator[]>[] Update);

        public event ResultDoneNotify Notify;
        public RecruitAPI()
        {
            m_tags = new ObservableCollection<string>();
            m_tags.CollectionChanged += TagsChanged;
        }
        private void TagsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Notify(API.BestOf(m_tags.ToArray(), target => target.Rank >= 3).ToArray());
        }
    }
}
