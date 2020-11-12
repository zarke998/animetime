using AnimeTime.Core.Domain;
using AnimeTime.Core.Domain.Enums;
using AnimeTime.Utilities;
using AnimeTimeDbUpdater.Core;
using AnimeTimeDbUpdater.Core.Domain;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTimeDbUpdater.Persistence
{
    class AnimePlanetCharacterRepository : ICharacterInfoRepository
    {
        private static string _blankCharacterImage = "blank_char.gif";

        public ICrawlDelayer CrawlDelayer { get; set; }

        public IEnumerable<CharacterBasicInfo> Extract(string animeCharactersUrl)
        {
            var htmlExtractor = new HtmlWeb();

            HtmlDocument htmlDocument = null;
            if(CrawlDelayer != null)
            {
                CrawlDelayer.ApplyDelay(() => htmlDocument = htmlExtractor.Load(animeCharactersUrl));
            }
            else
            {
                htmlDocument = htmlExtractor.Load(animeCharactersUrl);
            }

            var mainChars = ExtractCharacterByRole(htmlDocument, "Main");
            var secondaryChars = ExtractCharacterByRole(htmlDocument, "Secondary");
            var minorChars = ExtractCharacterByRole(htmlDocument, "Minor");

            return mainChars.Union(secondaryChars).Union(minorChars);
        }
        private ICollection<CharacterBasicInfo> ExtractCharacterByRole(HtmlDocument document, string role)
        {
            var basicInfos = new List<CharacterBasicInfo>();

            string charsSelector = "";
            switch (role)
            {
                case "Main":
                    charsSelector = @"//h3[contains(text(),'Main Characters')]/following-sibling::table[1]//td[@class='tableCharInfo']/a[@class='name']";
                    break;
                case "Secondary":
                    charsSelector = @"//h3[contains(text(),'Secondary Characters')]/following-sibling::table[1]//td[@class='tableCharInfo']/a[@class='name']";
                    break;
                case "Minor":
                    charsSelector = @"//h3[contains(text(),'Minor Characters')]/following-sibling::table[1]//td[@class='tableCharInfo']/a[@class='name']";
                    break;
                default:
                    return basicInfos;
            }

            var charNodes = document.DocumentNode.SelectNodes(charsSelector);
            if (charNodes != null)
            {
                foreach (var node in charNodes)
                {
                    var sourceUrl = node.GetAttributeValue("href", String.Empty);

                    var info = new CharacterBasicInfo();
                    info.DetailsUrl = Constants.WebsiteUrls.AnimePlanet + sourceUrl;
                    info.Role = role;

                    basicInfos.Add(info);
                }
            }

            return basicInfos;
        }

        public CharacterDetailedInfo Resolve(CharacterBasicInfo basicInfo)
        {
            var detailedInfo = new CharacterDetailedInfo();

            var htmlExtractor = new HtmlWeb();

            HtmlDocument document = null;
            if(CrawlDelayer != null)
            {
                CrawlDelayer.ApplyDelay(() => document = htmlExtractor.Load(basicInfo.DetailsUrl));
            }
            else
            {
                document = htmlExtractor.Load(basicInfo.DetailsUrl);
            }

            detailedInfo.BasicInfo = basicInfo;
            detailedInfo.Name = ResolveName(document);
            detailedInfo.Description = ResolveDescription(document);
            detailedInfo.ImageUrl = GetImageUrl(document);

            return detailedInfo;
        }

        private string ResolveName(HtmlDocument document)
        {
            var nameNode = document.DocumentNode.SelectSingleNode("//h1[@itemprop='name']");
            return nameNode.InnerText;
        }
        private string ResolveDescription(HtmlDocument document)
        {
            var descriptionNodes = document.DocumentNode.SelectNodes("//div[contains(@class,'entrySynopsis')]//div[@itemprop='description']/p//text()");
            if (descriptionNodes == null) return null;

            string description = "";
            foreach (var node in descriptionNodes)
            {
                description += node.InnerText;
            }

            return description;
        }
        private string GetImageUrl(HtmlDocument document)
        {
            var imageNode = document.DocumentNode.SelectSingleNode("//div[contains(@class,'entrySynopsis')]//div[@class='mainEntry']/img[@itemprop='image']");

            var imageUrl = imageNode.GetAttributeValue("src", String.Empty);

            if (imageUrl.Contains(_blankCharacterImage))
            {
                return null;
            }

            return Constants.WebsiteUrls.AnimePlanet + imageUrl;
        }
    }
}
