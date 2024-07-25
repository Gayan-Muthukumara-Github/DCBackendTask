using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DCBackend.DataAccessLayer.SQLHelper
{
    public static class SQlQueryHelper
    {
        private static readonly Dictionary<string, string> SqlQueries = new Dictionary<string, string>();

        public static void LoadQueries(string filePath)
        {
            var document = XDocument.Load(filePath);
            var queries = document.Descendants("query");
            foreach (var query in queries)
            {
                var name = query.Attribute("name").Value;
                var sql = query.Value.Trim();
                SqlQueries[name] = sql;
            }
        }

        public static string GetQuery(string name)
        {
            if (SqlQueries.TryGetValue(name, out var query))
            {
                return query;
            }
            throw new InvalidOperationException($"Query '{name}' not found.");
        }
    }
}
