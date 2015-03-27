using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BiddingTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var bidding = new Bidding<int>();

            bidding.InitDictionariesByRef(bidding.Campaigns);

            var biddingS = new Bidding<int>();

            biddingS.InitDictionariesByRef(biddingS.Campaigns);


            var query = new Query() { Category = 10, Country = 3, Platform = 1 };
            //bidding.LookupByInt(query);
            //bidding.LookupByRef(query);
            //var testGroup = new TestGroup("Uzpildymas");

            //var testResultSummary1 = testGroup.PlanAndExecute("By ref", () => bidding.InitDictionariesByRef(bidding.Campaigns), 1000);

            //var testResultSummary2 = testGroup.PlanAndExecute("by int", () => bidding.InitDictionariesByInt(bidding.Campaigns), 1000);

            //Console.WriteLine(testResultSummary1);
            //Console.WriteLine(testResultSummary2);





            for (int i = 0; i < 10000; i++)
            {
                bidding.LookupByRef(2, 5, 7, -1);
            } 
            for (int i = 0; i < 10000; i++)
            {
                bidding.LookupByRef2(2, 5, 7, -1);
            }



        }
    }





}




