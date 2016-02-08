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
            var request = OAuth.CreateSignedRequest(new Uri("https://stream.twitter.com/1.1/statuses/filter.json?delimited=length&track=twitter"));
            using (var stream = request.GetResponse().GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                var tweetCount = 0;
                var failCount = 0;
                while (stream.CanRead && !reader.EndOfStream)
                {
                    var data = reader.ReadLine();
                    if (data[0] == '{')
                    {
                        try
                        {
                            var tweet = JsonConvert.DeserializeObject<Tweet>(data);
                            Console.Clear();
                            Console.WriteLine("Tweets handled:\t" + (++tweetCount));
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
