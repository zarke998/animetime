using AnimeTime.Core.Domain;
using AnimeTime.Core.Domain.Enums;
using AnimeTimeDbUpdater.Core;
using AnimeTimeDbUpdater.Core.Domain;
using AnimeTimeDbUpdater.Utilities;
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
        public IEnumerable<CharacterInfo> Extract(string animeCharactersUrl)
        {
            ICollection<CharacterInfo> infos = new List<CharacterInfo>();

            var htmlExtractor = new HtmlWeb();

            HtmlDocument htmlDocument = null;
            CrawlDelayer.ApplyDelay(() => htmlDocument = htmlExtractor.Load(animeCharactersUrl));

            var mainChars = ExtractCharacterByRole(htmlDocument, CharacterRoleId.Main);
            var secondaryChars = ExtractCharacterByRole(htmlDocument, CharacterRoleId.Secondary);
            var minorChars = ExtractCharacterByRole(htmlDocument, CharacterRoleId.Minor);

            return mainChars.Union(secondaryChars).Union(minorChars);
        }
        private ICollection<CharacterInfo> ExtractCharacterByRole(HtmlDocument document, CharacterRoleId role)
        {
            ICollection<CharacterInfo> infos = new List<CharacterInfo>();

            string charsSelector = "";
            switch (role)
            {
                case CharacterRoleId.Main:
                    charsSelector = @"//h3[contains(text(),'Main Characters')]/following-sibling::table[1]//td[@class='tableCharInfo']/a[@class='name']";
                    break;
                case CharacterRoleId.Secondary:
                    charsSelector = @"//h3[contains(text(),'Secondary Characters')]/following-sibling::table[1]//td[@class='tableCharInfo']/a[@class='name']";
                    break;
                case CharacterRoleId.Minor:
                    charsSelector = @"//h3[contains(text(),'Minor Characters')]/following-sibling::table[1]//td[@class='tableCharInfo']/a[@class='name']";
                    break;
                default:
                    return infos;
            }

            var charNodes = document.DocumentNode.SelectNodes(charsSelector);
            if(charNodes != null)
            {
                foreach (var node in charNodes)
                {
                    var sourceUrl = node.GetAttributeValue("href", String.Empty);

                    var info = new CharacterInfo();
                    info.Character.SourceUrl = Constants.WebsiteUrls.AnimePlanet + sourceUrl;
                    info.Character.RoleId = role;

                    infos.Add(info);
                }
            }

            return infos;
        }

        public void Resolve(CharacterInfo characterInfo)
        {
            var sourceUrl = characterInfo.Character.SourceUrl;

            var htmlExtractor = new HtmlWeb();

            HtmlDocument document = null;
            CrawlDelayer.ApplyDelay(() => document = htmlExtractor.Load(sourceUrl));

            var character = characterInfo.Character;

            ResolveName(document, character);
            ResolveDescription(document, character);
            characterInfo.ImageUrl = GetImageUrl(document);
        }

        private void ResolveName(HtmlDocument document, Character character)
        {
            var nameNode = document.DocumentNode.SelectSingleNode("//h1[@itemprop='name']");
            character.Name = nameNode.InnerText;
        }
        private void ResolveDescription(HtmlDocument document, Character character)
        {
            var descriptionNodes = document.DocumentNode.SelectNodes("//div[contains(@class,'entrySynopsis')]//div[@itemprop='description']/p//text()");

            if(descriptionNodes != null)
            {
                string description = "";
                foreach (var node in descriptionNodes)
                {
                    description += node.InnerText;
                }

                character.Description = description;
            }
        }
        private string GetImageUrl(HtmlDocument document)
        {
            var imageNode = document.DocumentNode.SelectSingleNode("//div[contains(@class,'entrySynopsis')]//div[@class='mainEntry']/img[@itemprop='image']");

            var imageUrl = imageNode.GetAttributeValue("src", String.Empty);
            return Constants.WebsiteUrls.AnimePlanet + imageUrl;
        }
    }
}
