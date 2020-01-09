using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parser.Models;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Parser.Controllers
{
    public class ParserController : Controller
    {
        private readonly ParserContext _context;
        public ParserController(ParserContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Scan()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Scan(string url, int depth)
        {
            var urls = ParseAll(url, depth);
            var newRecords = urls.Select(u => new Record(u, GetBody(u)));

            var host = new Uri(urls.First()).Host;
            var domain = _context.Domains.Find(host);
            if (domain != null)
            {
                domain.Records ??= _context.Records.Where(r => r.Domain == domain).ToHashSet();
                domain.Records.UnionWith(newRecords);
            }
            else
            {
                _context.Domains.Add(new Domain { DomainName = host, Records = new HashSet<Record>(newRecords) });
            }
            _context.SaveChanges();

            return View("ScanResult", urls);
        }

        public IActionResult Save()
        {
            return View("Index");
        }
        [HttpPost]
        public IActionResult Save(List<string> urls)
        {
            var newRecords = urls.Select(u => new Record(u, GetBody(u)));

            var host = new Uri(urls.First()).Host;
            var domain = _context.Domains.Find(host);
            if (domain != null)
            {
                domain.Records.UnionWith(newRecords);
            }
            else
            {
                _context.Domains.Add(new Domain { DomainName = host, Records = new HashSet<Record>(newRecords) });
            }
            return View("Index");
        }


        [HttpGet]
        public IActionResult Domain()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Domain(string host)
        {
            var records = _context.Records.Where(r => r.Domain.DomainName == host).ToList();
            records ??= new List<Record>();
            return View("Result", records);
        }

        private List<string> ParseAll(string url, int depth)
        {
            var res = new HashSet<string>();
            var current = new List<string>()
            {
                url
            };
            var founded = new List<string>();
            for (int i = 0; i < depth; i++)
            {
                foreach (var u in current)
                {
                    founded.AddRange(Parse(new Uri(u)));
                }
                res.UnionWith(founded);
                current = founded.Except(res).ToList();
                founded.Clear();
            }
            return res.ToList();
        }

        /// <summary>
        /// для данной ссылки ищет все ссылки на этот же сайт
        /// </summary>
        private IEnumerable<string> Parse(Uri uri)
        {
            var web = new HtmlWeb();
            var doc = web.Load(uri);
            return doc.DocumentNode
                .SelectNodes("//a")
                .Where(n => n.InnerHtml.Contains(uri.Host))
                .Select(n => n.InnerHtml)
                .Distinct();
        }

        /// <summary>
        /// вырезает теги из html-разметки
        /// </summary>
        private string GetBody(string url)
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);
            //var replacement = doc.CreateTextNode(" ");
            // doc.DocumentNode.Descendants()
            //    .Select(n => n.ParentNode.ReplaceChild(replacement, n))
            //    .Select(n => n.InnerHtml);
            var body = Regex.Replace(doc.ParsedText, "<[^>]+>", string.Empty);
            if (body.Length > 500)
                return body.Substring(0, 500);
            return body;
        }
    }
}