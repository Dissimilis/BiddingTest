using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace BiddingTest
{

    public class Query
    {
        public int Country { get; set; }
        public int Category { get; set; }
        public int Platform { get; set; }
    }
    public class Campaign<T>
    {
        public decimal Bid { get; set; }
        public List<T> Country { get; set; }
        public List<T> Category { get; set; }
        public List<T> Platform { get; set; }
    }

    public class Bidding<T>
    {

        public List<Campaign<T>> Campaigns = new List<Campaign<T>>(10000);

        private Dictionary<T, List<Campaign<T>>> ByCountry;
        private Dictionary<T, List<Campaign<T>>> ByCategory;
        private Dictionary<T, List<Campaign<T>>> ByPlatform;

        private List<Campaign<T>> CountryW = new List<Campaign<T>>();
        private List<Campaign<T>> CategoryW = new List<Campaign<T>>();
        private List<Campaign<T>> PlatformW = new List<Campaign<T>>();


        public Bidding()
        {
            var rand = new Random();
            for (int i = 0; i < 1000; i++)
            {
                var campaign = new Campaign<T>()
                {
                    Bid = rand.Next(1, 10),
                    
                };
                campaign.Platform = Enumerable.Range(0, rand.Next(0, 4)).Select(c => rand.Next(0, 3)).Select(c => (T)Convert.ChangeType(c, typeof(T))).ToList();
                campaign.Country = Enumerable.Range(0, rand.Next(0, 4)).Select(c => rand.Next(0, 200)).Select(c => (T)Convert.ChangeType(c, typeof(T))).ToList();
                campaign.Category = Enumerable.Range(0, rand.Next(0, 3)).Select(c => rand.Next(0, 50)).Select(c => (T)Convert.ChangeType(c, typeof(T))).ToList();
                Campaigns.Add(campaign);
            }
        }


        private void AddByRef(Dictionary<T, List<Campaign<T>>> dict, T key, Campaign<T> c)
        {
            List<Campaign<T>> existing;
            if (dict.TryGetValue(key, out existing))
            {
                existing.Add(c);
            }
            else
            {
                existing = new List<Campaign<T>>() { c };
                dict.Add(key, existing);
            }
        }


        public void InitDictionariesByRef(List<Campaign<T>> campaigns)
        {
            ByCategory = new Dictionary<T, List<Campaign<T>>>();
            ByCountry = new Dictionary<T, List<Campaign<T>>>();
            ByPlatform = new Dictionary<T, List<Campaign<T>>>();
            for (int i = 0; i < campaigns.Count; i++)
            {
                var c = campaigns[i];

                if (!c.Country.Any())
                {
                    CountryW.Add(c);
                }
                else
                {
                    foreach (var country in c.Country)
                    {
                        AddByRef(ByCountry, country, c);
                    }
                }

                if (!c.Category.Any())
                {
                    CategoryW.Add(c);
                }
                else
                {
                    foreach (var category in c.Category)
                    {
                        AddByRef(ByCategory, category, c);
                    }
                }
                if (!c.Platform.Any())
                {
                    PlatformW.Add(c);
                }
                else
                {
                    foreach (var platform in c.Platform)
                    {
                        AddByRef(ByPlatform, platform, c);
                    }
                }
            }
        }




        public IEnumerable<Campaign<T>> LookupByRef(T country, T category, T platform, T wildcard)
        {
            List<Campaign<T>> co1;
            ByCountry.TryGetValue(country, out co1);

            List<Campaign<T>> ca1;
            ByCategory.TryGetValue(category, out ca1);

            List<Campaign<T>> p1;
            ByPlatform.TryGetValue(platform, out p1);

            if (p1 == null)
                p1 = new List<Campaign<T>>();

            var final = co1.Concat(CountryW).Concat(ca1).Concat(CategoryW).Concat(p1).Concat(PlatformW);

            return final.ToArray();
        }
        public IEnumerable<Campaign<T>> LookupByRef2(T country, T category, T platform, T wildcard)
        {
            List<Campaign<T>> co1;
            ByCountry.TryGetValue(country, out co1);

            List<Campaign<T>> ca1;
            ByCategory.TryGetValue(category, out ca1);

            List<Campaign<T>> p1;
            ByPlatform.TryGetValue(platform, out p1);
            if (p1 == null)
                p1 = new List<Campaign<T>>();

            var final = co1.Concat(CountryW).Intersect(ca1.Concat(CategoryW)).Intersect(p1.Concat(PlatformW));

            return final.ToArray();
        }


    }
}