using AnimeTime.Utilities.Web;
using AnimeTime.WebsiteProcessors.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace AnimeTime.WebsiteProcessors.AjaxProcessors
{
    public class AnimeOwlAjaxProcessor
    {
        private string episodeUrlPrefix = "https://portablegaming.co/watch/";
        private string ajaxSearchUrl = "https://animeowl.net/api/advance-search";
        private string ajaxEpisodesUrl = "https://animeowl.net/api/anime/{0}/episodes";

        public async Task<IEnumerable<AnimeSearchResult>> GetSearchResultsAsync(string searchString)
        {
            using (var client = new HttpClient())
            {
                string json = @"{
                        ""clicked"": false,
                        ""limit"": 30,
                        ""page"": 0,
                        ""pageCount"": 1,
                        ""value"": "" " + $"{searchString}" + @" "",
                        ""sort"": 4,
                        ""selected"": {
                            ""type"": [],
                            ""genre"": [],
                            ""year"": [],
                            ""country"": [],
                            ""season"": [],
                            ""status"": [],
                            ""sort"": [],
                            ""language"": []
                        },
                        ""results"": [],
                        ""label"": ""searching ....""
                    }
                ";

                var searchResults = new List<AnimeSearchResult>();

                var response = await client.PostAsync(ajaxSearchUrl, new StringContent(json, Encoding.UTF8, "application/json"));
                dynamic result = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);

                foreach (var item in result.results)
                {
                    searchResults.Add(new AnimeSearchResult()
                    {
                        Title = item.anime_name,
                        Url = "anime/" + item.anime_slug
                    });
                }
                return searchResults;
            }
        }

        public async Task<IEnumerable<EpisodeSource>> GetEpisodesAsync(string animeUrl)
        {
            var web = new HtmlWeb();
            var doc = web.Load(animeUrl);

            var animeId = doc.DocumentNode.SelectSingleNode(".//div[@id='unq-anime-id']").GetAttributeValue("animeid", 0);

            var result = new List<EpisodeSource>();
            using (var client = new HttpClient())
            {
                var responseContent = await client.GetStringAsync(String.Format(ajaxEpisodesUrl, animeId));
                var responseJson = JsonConvert.DeserializeObject<dynamic>(responseContent);

                dynamic episodes;
                string slug;
                if (IsAnimeUrlSub(animeUrl))
                {
                    episodes = responseJson.sub;
                    slug = responseJson.sub_slug;
                }
                else
                {
                    episodes = responseJson.dub;
                    slug = responseJson.dub_slug;
                }
                foreach (var episode in episodes)
                {
                    result.Add(new EpisodeSource()
                    {
                        EpisodeNumber = Convert.ToInt32(episode.name),
                        Url = WebUtils.CombineUrls(episodeUrlPrefix, slug, Convert.ToString(episode.episode_index))
                    });
                }

                return result;
            }
        }

        public async Task<IEnumerable<string>> GetVideoSourcesAsync(string episodeUrl)
        {
            var result = new List<string>();
            using (var browserFetcher = new BrowserFetcher())
            {
                await browserFetcher.DownloadAsync();

                var browser = await Puppeteer.LaunchAsync(
                    new LaunchOptions { Headless = true }
                );

                var page = await browser.NewPageAsync();
                

                await page.GoToAsync(episodeUrl);

                await page.WaitForXPathAsync(".//div[contains(@class,'choose-no-ads-server')]/div");
                var videoSources = await page.XPathAsync(".//div[contains(@class,'choose-no-ads-server')]/div");
                foreach (var videoSource in videoSources)
                {
                    var link = await page.EvaluateFunctionAsync<string>("e => e.getAttribute('data-source')", videoSource);
                    result.Add(link);
                }

                await browser.CloseAsync();
            }
            return result;
        }

        private bool IsAnimeUrlSub(string animeUrl)
        {
            return !animeUrl.Contains("-dub");
        }
    }
}
