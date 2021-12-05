using AnimeTime.Core;
using AnimeTime.Core.Domain;
using AnimeTime.Core.Exceptions;
using AnimeTime.WebsiteProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTimeAnimeSourceUpdater
{
    public class MainApplication : IApplication
    {
        public void Run()
        {
            var unitOfWork = ClassFactory.CreateUnitOfWork();

            var animeIdsWithNoSources = unitOfWork.Animes.GetIdsWithNoSources();
            if(animeIdsWithNoSources.Count() == 0)
            {
                Environment.Exit(0);
            }
            var allSourceUrls = unitOfWork.AnimeSources.GetAllUrls();            

            var websites = unitOfWork.Websites.GetAll();
            var websiteProcessorPairs = new List<(Website website, IWebsiteProcessor processor)>();
            foreach (var website in websites)
            {
                var processor = WebsiteProcessorFactory.CreateWebsiteProcessor(website.Name, website.Url, website.QuerySuffix);
                processor.CrawlDelayer = ClassFactory.CreateCrawlDelayer();

                websiteProcessorPairs.Add((website, processor));
            }

            foreach (var animeId in animeIdsWithNoSources)
            {
                var anime = unitOfWork.Animes.GetWithAltTitles(animeId);

                var sourceFetchTasks = new List<Task<(string animeUrl, string animeDubUrl)>>();
                foreach (var pair in websiteProcessorPairs)
                {
                    sourceFetchTasks.Add(pair.processor.GetAnimeUrlAsync(anime.Title, anime.ReleaseYear, anime.AltTitles.Select(t => t.Title)));
                }
                var sources = Task.WhenAll(sourceFetchTasks).Result;

                for (int i = 0; i < sources.Length; i++)
                {
                    var website = websiteProcessorPairs[i].website;
                    if (sources[i].animeUrl == null && sources[i].animeDubUrl == null)
                    {
                        var source = new AnimeSource()
                        {
                            WebsiteId = website.Id,
                            AnimeId = animeId,
                            Status_Id = AnimeTime.Core.Domain.Enums.AnimeSourceStatusIds.CouldNotResolve
                        };
                        anime.AnimeSources.Add(source);
                    }

                    if (sources[i].animeUrl != null)
                    {
                        var source = new AnimeSource()
                        {
                            WebsiteId = website.Id,
                            AnimeId = animeId,
                            Url = sources[i].animeUrl,
                            AnimeVersion_Id = AnimeTime.Core.Domain.Enums.AnimeVersionIds.Sub,
                        };

                        if (!allSourceUrls.Contains(sources[i].animeUrl))
                            source.Status_Id = AnimeTime.Core.Domain.Enums.AnimeSourceStatusIds.Resolved;
                        else
                            source.Status_Id = AnimeTime.Core.Domain.Enums.AnimeSourceStatusIds.Conflict;

                        allSourceUrls.Add(sources[i].animeUrl);
                        anime.AnimeSources.Add(source);
                    }

                    if (sources[i].animeDubUrl != null)
                    {
                        var source = new AnimeSource()
                        {
                            WebsiteId = website.Id,
                            AnimeId = animeId,
                            Url = sources[i].animeDubUrl,
                            AnimeVersion_Id = AnimeTime.Core.Domain.Enums.AnimeVersionIds.Dub,
                        };

                        if (!allSourceUrls.Contains(sources[i].animeDubUrl))
                            source.Status_Id = AnimeTime.Core.Domain.Enums.AnimeSourceStatusIds.Resolved;
                        else
                            source.Status_Id = AnimeTime.Core.Domain.Enums.AnimeSourceStatusIds.Conflict;

                        allSourceUrls.Add(sources[i].animeDubUrl);
                        anime.AnimeSources.Add(source);
                    }
                }

                try
                {
                    unitOfWork.Complete();
                }
                catch (EntityInsertException e)
                {
                    // Log exception to db                    

                    // Remove added sources in cache (unneeded?)
                    foreach (var source in sources)
                    {
                        if (source.animeUrl != null)
                            allSourceUrls.Remove(source.animeUrl);
                        if (source.animeDubUrl != null)
                            allSourceUrls.Remove(source.animeDubUrl);
                    }

                    Environment.Exit(0);
                }
            }

            MarkSourcesAsInitialized(unitOfWork);
        }

        private void MarkSourcesAsInitialized(IUnitOfWork unitOfWork)
        {
            var metadata = unitOfWork.Metadata.GetAll().FirstOrDefault();

            if (metadata != null)
            {
                metadata.AnimeSourcesInitialized = true;
                unitOfWork.Complete();
            }
            else
            {
                metadata = new Metadata();
                metadata.AnimeSourcesInitialized = true;

                unitOfWork.Metadata.Add(metadata);
                unitOfWork.Complete();
            }
        }
    }
}
