using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterStreamExample
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Enter the text you would like to track:");
                args = new[] { Console.ReadLine() };
            }
            var request = OAuth.CreateSignedRequest(new Uri("https://stream.twitter.com/1.1/statuses/filter.json?delimited=length&track=" + args[0]));
            using (var stream = request.GetResponse().GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                var tweetCount = 0;
                var failCount = 0;
                var start = DateTime.Now;
                while (stream.CanRead && !reader.EndOfStream)
                {
                    var data = reader.ReadLine();
                    if (!string.IsNullOrEmpty(data) && data[0] == '{')
                    {
                        try
                        {
                            var tweet = JsonConvert.DeserializeObject<Tweet>(data);
                            Console.Clear();
                            Console.WriteLine("Tweets handled:\t{0}\t({1:0.##} t/s)",(++tweetCount), tweetCount / (DateTime.Now - start).TotalSeconds);
                            Console.WriteLine("Tweets failed:\t" + (failCount));
                        }
                        catch
                        {
                            Console.WriteLine("Unhandled:" + data);
                            failCount++;
                        }
                    }
                }
            }
        }
    }
}
