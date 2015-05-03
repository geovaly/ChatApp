using System.Collections.Generic;
using ChatApp.Application.Queries.Base;
using ChatApp.Utility;
using RestSharp;

namespace ChatApp.Application.RestSharp
{
    public class RestSharpQueryBus : RestSharpBase, IQueryBus
    {
        public RestSharpQueryBus(string serviceUrl)
            : base(serviceUrl)
        {
        }

        public List<TResult> List<TQuery, TResult>(TQuery query)
            where TQuery : IListQuery<TResult>
            where TResult : new()
        {
            return Execute<TQuery, List<TResult>>(query, Method.GET);
        }

        public List<TResult> List<TQuery, TResult>()
            where TQuery : IListQuery<TResult>, new()
            where TResult : new()
        {
            return List<TQuery, TResult>(new TQuery());
        }

        public TResult Get<TQuery, TResult>(TQuery query)
            where TQuery : IGetQuery<TResult>
            where TResult : new()
        {
            return Execute<TQuery, TResult>(query, Method.GET);
        }

        protected override void SetObject<TObject>(RestRequest request, TObject obj)
        {
            AddParametersFromFields(request, obj);
            AddParametersFromProperties(request, obj);
        }

        private static void AddParametersFromFields<TObject>(RestRequest request, TObject obj)
        {
            foreach (var f in typeof(TObject).GetFields())
            {
                var value = f.GetValue(obj);
                if (value != null)
                    request.AddParameter(f.Name, value);
            }
        }

        private static void AddParametersFromProperties<TObject>(RestRequest request, TObject obj)
        {
            foreach (var p in typeof(TObject).GetProperties())
            {
                var value = p.GetValue(obj);
                if (value != null)
                    request.AddParameter(p.Name, value);
            }
        }

        protected override string GetResource<TObject>()
        {
            var queryName = typeof(TObject).Name;

            return "api/ChatRoom/Get" +
                 (queryName.EndsWith("ListQuery")
                     ? queryName.RemoveSubstring("ListQuery")
                     : queryName.RemoveSubstring("GetQuery"));
        }
    }
}