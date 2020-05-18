using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch
{
    class IndexController
    {
        private ElasticClient esClient;
        private Uri esHost;

        public IndexController(String esURL)
        {
            esHost = new Uri(esURL);
            var connectionPool = new SingleNodeConnectionPool(esHost);
            var settings = new ConnectionSettings(connectionPool);
            esClient = new ElasticClient(settings);
        }

        public async Task<bool> indexSingleUser(User newUser)
        {
            var indexName = "users";
            var indexResponse = await esClient.IndexAsync(newUser, f => f.Index(indexName));
            if (indexResponse.IsValid)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> indexManyUsers(List<User> newUsers)
        {
            var indexName = "users";
            var indexResponse = await esClient.BulkAsync(f => f.Index(indexName).IndexMany(newUsers, (descriptor, s) => descriptor.Index(indexName)));
            if (indexResponse.IsValid)
            {
                return true;
            }
            return false;
        }
    }
}
