using System;
using System.Net;
using RestSharp;

namespace ChatApp.Application.RestSharp
{
    public abstract class RestSharpBase
    {
        private readonly string _serviceUrl;

        protected RestSharpBase(string serviceUrl)
        {
            _serviceUrl = serviceUrl;
        }

        protected TResponse Execute<TObject, TResponse>(TObject obj, Method method)
            where TResponse : new()
        {
            var response = RestClient().Execute<TResponse>(Request(obj, method));
            CheckResponseIsOk(response);
            return response.Data;
        }

        protected void Execute<TObject>(TObject obj, Method method)
        {
            var response = RestClient().Execute(Request(obj, method));
            CheckResponseIsOk(response);
        }

        protected abstract void SetObject<TObject>(RestRequest request, TObject obj);

        protected abstract string GetResource<TObject>();

        private RestRequest Request<TObject>(TObject obj, Method method)
        {
            var request = new RestRequest(GetResource<TObject>(), method);
            request.RequestFormat = DataFormat.Json;
            SetObject(request, obj);
            return request;
        }

        private RestClient RestClient()
        {
            return new RestClient(_serviceUrl);
        }

        private static void CheckResponseIsOk(IRestResponse response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException();
        }

    }
}