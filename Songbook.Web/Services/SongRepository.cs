using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Options;
using Songbook.Core;

namespace Songbook.Web.Services
{
    public class DocumentDbOptions
    {
        public string ServiceEndpoint { get; set; }
        public string AuthKey { get; set; }
        public string DatabaseId { get; set; }
        public string CollectionId { get; set; }
    }

    public class SongRepository
    {
        private readonly DocumentDbOptions _options;

        public SongRepository(IOptions<DocumentDbOptions> options)
        {
            _options = options.Value;
        }

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

        private Uri DatabaseUri => UriFactory.CreateDatabaseUri(_options.DatabaseId);
        private Uri CollectionUri => UriFactory.CreateDocumentCollectionUri(_options.DatabaseId, _options.CollectionId);
        private Uri CreateSongDocumentUri(string songId) => UriFactory.CreateDocumentUri(_options.DatabaseId, _options.CollectionId, songId);

        private DocumentClient GetDocumentClient()
        {
            var client = new DocumentClient(
                new Uri(_options.ServiceEndpoint),
                _options.AuthKey,
                new ConnectionPolicy { EnableEndpointDiscovery = false });

            CreateDatabaseIfNotExists(client).Wait();
            CreateCollectionIfNotExists(client).Wait();

            return client;
        }

        private async Task CreateDatabaseIfNotExists(DocumentClient client)
        {
            try
            {
                await client.ReadDatabaseAsync(DatabaseUri);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode != System.Net.HttpStatusCode.NotFound)
                    throw;

                await client.CreateDatabaseAsync(new Database { Id = _options.DatabaseId });
            }
        }

        private async Task CreateCollectionIfNotExists(DocumentClient client)
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
                    DatabaseUri, 
                    new DocumentCollection { Id = _options.CollectionId },
                    new RequestOptions { OfferThroughput = 1000 });
            }
        }
    }
}
