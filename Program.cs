using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace CCScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            var initHttpUrl = "http://paysites.mustbedestroyed.org/booty/ts4patreon/";
            HttpClient client = new HttpClient();
            
            var web = new HtmlWeb();
            var htmlDoc = new HtmlDocument();
            var pageDocument = web.Load(initHttpUrl);
            Dictionary<int, string> visitDictionary = new Dictionary<int, string>();
            var dictKey = 0;
            var linkNodes = pageDocument.DocumentNode.Descendants("pre").ToList();
            Console.WriteLine("Enter the number to visit corresponding section");
            foreach(var linkNode in linkNodes)
            {
                var htmlArr = linkNode.WriteContentTo().Split(" -");
                
                htmlDoc.LoadHtml(linkNode.WriteContentTo());
                var hrefNodes = htmlDoc.DocumentNode.SelectNodes("//a");
                foreach(var hrefNode in hrefNodes)
                {
                    Console.WriteLine(hrefNode.WriteContentTo());
                    var strHref = htmlArr.SingleOrDefault(htmlStr => htmlStr.Contains(hrefNode.InnerText));
                    var match = Regex.Match(strHref, @"(0[1-9]|1\d|2\d|3[01])-([a-zA-Z]{3})-(19|20)\d{2}\s+(0[1-9]|1[0-9]|2[0-3])\:(0[1-9]|[1-5][0-9])");

                    visitDictionary.Add(dictKey, hrefNode.GetAttributeValue("href", ""));
                    //Build visit dictionary
                    Console.WriteLine(String.Format("{0} : {1} Date Updated: {2}", dictKey, hrefNode.InnerText, match.Value));
                    dictKey++;
                }
            }
            Console.WriteLine("Which folder do you want to browse? : ");
            var input = int.Parse(Console.ReadLine());
            Console.Clear();
            string newUrl = string.Format("{0}{1}", initHttpUrl, visitDictionary.GetValueOrDefault<int, string>(input, ""));
            Console.WriteLine(String.Format("Trying to visit the URL: {0}", newUrl));
           
            pageDocument = web.Load(newUrl);
            linkNodes = pageDocument.DocumentNode.Descendants("pre").ToList();
            foreach (var linkNode in linkNodes)
            {
                foreach(var linkChild in linkNode.ChildNodes)
                {

                    Console.WriteLine(linkChild.WriteContentTo());
                }
            }
            Console.WriteLine("End of Links");
            Console.ReadLine();
        }
    }
}
