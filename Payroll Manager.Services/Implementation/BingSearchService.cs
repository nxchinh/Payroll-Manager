using System;
using System.Net.Http;
using System.Threading.Tasks;
using Payroll_Manager.Entity;

namespace Payroll_Manager.Services.Implementation
{
    public class BingSearchService : StatisticSearchService, IBingSearchService
    {
        public const string _searchEngines = @"https://bing.com";
        private const string _tagSearch = "//li[contains(@class, 'b_algo')]";

        public BingSearchService(HttpClient httpClient) : base(httpClient, _tagSearch)
        {
        }
        public string SearchName => _searchEngines;
        public async Task<SearchResult> Search(string keywords, string tagUrl)
        {
            var returnResult = new SearchResult { Keywords = keywords, Url = tagUrl };
            var searchUrl = $"{_searchEngines}/search?q={keywords}&count=100";
            returnResult.Results = await base.SearchByUrl(searchUrl, tagUrl); ;
            returnResult.Date = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            returnResult.SearchEngine = _searchEngines;
            return returnResult;
        }
    }
}
