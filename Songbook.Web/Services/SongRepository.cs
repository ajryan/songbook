using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Songbook.Core;

namespace Songbook.Web.Services
{
    public class SongRepository
    {
        public async Task<IEnumerable<Song>> GetAll()
        {
            // TODO: appsettings.json

            using (var client = GetDocumentClient())
            {
                var query = client.CreateDocumentQuery<Song>(
                    CollectionUri,
                    new FeedOptions { MaxItemCount = -1 }).AsDocumentQuery();

                var songs = new List<Song>();

                while (query.HasMoreResults)
                    songs.AddRange(await query.ExecuteNextAsync<Song>());

                return songs;
            }
        }

        public async Task<Song> GetSong(string songId)
        {
            using (var client = GetDocumentClient())
            {
                try
                {
                    var doc = await client.ReadDocumentAsync(CreateSongDocumentUri(songId));

                    return (Song)(dynamic)doc.Resource;
                }
                catch (DocumentClientException e)
                {
                    if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                        return null;

                    throw;
                }
            }
        }

        public async Task<Document> CreateSong(Song song)
        {
            using (var client = GetDocumentClient())
            {
                return await client.CreateDocumentAsync(CollectionUri, song);
            }
        }

        public async Task<Document> UpdateSong(string songId, Song song)
        {
            using (var client = GetDocumentClient())
            {
                return await client.ReplaceDocumentAsync(CreateSongDocumentUri(songId), song);
            }
        }

        public async Task DeleteSong(string songId)
        {
            using (var client = GetDocumentClient())
            {
                await client.DeleteDocumentAsync(CreateSongDocumentUri(songId));
            }
        }

        private static Uri CollectionUri => UriFactory.CreateDocumentCollectionUri("songbook", "songs");
        private static Uri CreateSongDocumentUri(string songId) => UriFactory.CreateDocumentUri("songbook", "songs", songId);

        private static DocumentClient GetDocumentClient()
        {
            var client = new DocumentClient(
                new Uri("https://localhost:8081/"),
                "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                new ConnectionPolicy { EnableEndpointDiscovery = false });

            CreateDatabaseIfNotExists(client).Wait();
            CreateCollectionIfNotExists(client).Wait();

            return client;
        }

        private static async Task CreateDatabaseIfNotExists(DocumentClient client)
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri("songbook"));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode != System.Net.HttpStatusCode.NotFound)
                    throw;

                await client.CreateDatabaseAsync(new Database { Id = "songbook" });
            }
        }

        private static async Task CreateCollectionIfNotExists(DocumentClient client)
        {
            try
            {
                await client.ReadDocumentCollectionAsync(CollectionUri);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode != System.Net.HttpStatusCode.NotFound)
                    throw;

                await client.CreateDocumentCollectionAsync(
                    UriFactory.CreateDatabaseUri("songbook"), 
                    new DocumentCollection { Id = "songs" },
                    new RequestOptions { OfferThroughput = 1000 });
            }
        }
    }
}
