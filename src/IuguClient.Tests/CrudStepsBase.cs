﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RestSharp;
using TechTalk.SpecFlow;
using static TechTalk.SpecFlow.ScenarioContext;

namespace IuguClientAPI.Tests
{
    [Binding]
    public class CrudStepsBase
    {
        public static IRestClient RestClient
        {
            get { return Current.Get<IRestClient>(nameof(RestClient)); }
            set { Current.Add(nameof(RestClient), value); }
        }

        public static Func<Expression<Predicate<IRestRequest>>, Task> Asserter
        {
            get { return Current.Get<Func<Expression<Predicate<IRestRequest>>, Task>>($"{Current.ScenarioInfo.Title}Asserter"); }
            set { Current.Add($"{Current.ScenarioInfo.Title}Asserter", value); }
        }

        protected static Task AssertRequestMatches(Expression<Predicate<IRestRequest>> expression)
            => Asserter(expression);

        [Then(@"the request should be a POST")]
        public Task ThenTheRequestShouldBeAPOST()
           => AssertRequestMatches(h => h.Method == Method.POST);

        [Then(@"the url should end with ""(.*)""")]
        public void ThenTheUrlShouldEndWith(string uri)
            => AssertRequestMatches(h => h.Resource == uri);

        [Then(@"should send Json object into the body")]
        public Task ThenShouldSendJsonObjectIntoTheBody()
            => AssertRequestMatches(h => h.RequestFormat == DataFormat.Json);

        [Then(@"the url should end with ""(.*)"" with id value equals to (.*)")]
        public void ThenTheUrlShouldEndWithWithIdValueEqualsTo(string uri, int id)
            => AssertRequestMatches(h => h.Resource == uri && h.Parameters.Any(p => p.Type == ParameterType.UrlSegment && (string)p.Value == id.ToString()));

        [Then(@"the request should be a PUT")]
        public void ThenTheRequestShouldBeAPUT() => AssertRequestMatches(h => h.Method == Method.PUT);

        [Then(@"the request should be a DELETE")]
        public void ThenTheRequestShouldBeADELETE()
            => AssertRequestMatches(h => h.Method == Method.DELETE);

        [Then(@"should send Api Token into the header")]
        public void ThenShouldSendApiTokenIntoTheHeader()
            => AssertRequestMatches(h => h.Parameters.Any(x => x.Type == ParameterType.HttpHeader && x.Name == "api_token"));
    }
}