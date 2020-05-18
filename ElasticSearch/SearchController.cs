using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch
{
    class SearchController
    {
        private ElasticClient esClient;
        private Uri esHost;

        public SearchController(String esURL)
        {
            esHost = new Uri(esURL);
            var connectionPool = new SingleNodeConnectionPool(esHost);
            var settings = new ConnectionSettings(connectionPool);
            esClient = new ElasticClient(settings);
        }

        public async void searchUsers(String searchText, bool isAutoComplete)
        {
            String indexName = "users";

            string query;
            searchText = searchText.Replace("%", "").Replace("/", " ").Replace("\"", "").Trim();
            if (isAutoComplete)
            {
                query = $"{searchText}*";
            }
            else
            {
                query = $"*{string.Join("* AND *", searchText.Split())}*";
            }
            QueryStringQuery qsq = new QueryStringQuery
            {
                Fields = Infer.Field<User>(d => d.firstName)
                    .And<User>(d => d.lastName),
                Query = query
            };


            var filters = new List<QueryContainer>
            {
                new NumericRangeQuery {Field = "age", GreaterThanOrEqualTo = 23}
            };

            var must = new List<QueryContainer>();
            must.Add(qsq);


            var sort = new List<ISort>();
            sort.Add(new FieldSort { Field = "age", Order = SortOrder.Descending });

            var searchRequest = new SearchRequest(indexName)
            {
                Query = new BoolQuery
                {
                    Must = must,
                    Filter = filters
                },
                Sort = sort
            };

            var json = esClient.RequestResponseSerializer.SerializeToString(searchRequest);
            var result = await esClient.SearchAsync<User>(searchRequest);
        }
    }
}
