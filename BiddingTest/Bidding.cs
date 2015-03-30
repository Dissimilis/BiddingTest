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
        public T[] Country { get; set; }
        public T[] Category { get; set; }
        public T[] Platform { get; set; }
    }

    public class Bidding<T>
    {

        public List<Campaign<T>> Campaigns = new List<Campaign<T>>(10000);

        private Dictionary<T, Campaign<T>[]> ByCountry;
        private Dictionary<T, Campaign<T>[]> ByCategory;
        private Dictionary<T, Campaign<T>[]> ByPlatform;

        private Campaign<T>[] CountryW = new Campaign<T>[] { };
        private Campaign<T>[] CategoryW = new Campaign<T>[] { };
        private Campaign<T>[] PlatformW = new Campaign<T>[] { };


        public Bidding()
        {
            var rand = new Random();
            for (int i = 0; i < 1000; i++)
            {
                var campaign = new Campaign<T>
                {
                    Bid = rand.Next(1, 10),
                    Platform = Enumerable.Range(0, rand.Next(0, 4)).Select(c => rand.Next(0, 3)).Select(c => (T) Convert.ChangeType(c, typeof (T))).ToArray(),
                    Country = Enumerable.Range(0, rand.Next(0, 4)).Select(c => rand.Next(0, 200)).Select(c => (T) Convert.ChangeType(c, typeof (T))).ToArray(),
                    Category = Enumerable.Range(0, rand.Next(0, 3)).Select(c => rand.Next(0, 50)).Select(c => (T) Convert.ChangeType(c, typeof (T))).ToArray(),
                };
               
                Campaigns.Add(campaign);
            }
        }


        private void AddByRef(Dictionary<T, Campaign<T>[]> dict, T key, Campaign<T> c)
        {
            Campaign<T>[] existing;
            if (dict.TryGetValue(key, out existing))
            {
                existing.Add(c);
            }
            else
            {
                existing = new[] { c };
                dict.Add(key, existing);
            }
        }


        public void InitDictionariesByRef(List<Campaign<T>> campaigns)
        {
            ByCategory = new Dictionary<T, Campaign<T>[]>();
            ByCountry = new Dictionary<T, Campaign<T>[]>();
            ByPlatform = new Dictionary<T, Campaign<T>[]>();

            var tempList = new List<Campaign<T>>(10000); 
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
            Campaign<T>[] co1;
            ByCountry.TryGetValue(country, out co1);

            Campaign<T>[] ca1;
            ByCategory.TryGetValue(category, out ca1);

            Campaign<T>[] p1;
            ByPlatform.TryGetValue(platform, out p1);

            if (p1 == null)
                p1 = new Campaign<T>[]{};

            var final = co1.Concat(CountryW).Intersect(ca1.Concat(CategoryW)).Intersect(p1.Concat(PlatformW));

            return final.ToArray();
        }
        public IEnumerable<Campaign<T>> LookupByRef2(T country, T category, T platform, T wildcard)
        {
            Campaign<T>[] co1;
            ByCountry.TryGetValue(country, out co1);

            Campaign<T>[] ca1;
            ByCategory.TryGetValue(category, out ca1);

            Campaign<T>[] p1;
            ByPlatform.TryGetValue(platform, out p1);
            if (p1 == null)
                p1 = new Campaign<T>[]{};

            var final = co1.Concat2(CountryW).Intersect(ca1.Concat2(CategoryW)).Intersect(p1.Concat2(PlatformW));
            return final.ToArray();
        }


    }

    public static class ArrayEx
    {
        public static T[] Concat2<T>(this T[] arr1, T[] arr2)
        {
            var a = new T[arr1.Length + arr2.Length];
            Array.Copy(arr1, a, arr1.Length);
            Array.Copy(arr2, 0, a, arr1.Length, arr2.Length);
            return a;
        }
        public static T[] Add<T>(this T[] dst, T itm)
        {
            Array.Resize(ref dst, dst.Length + 1);
            dst[dst.Length-1] = itm;
            return dst;
        }
    }
}