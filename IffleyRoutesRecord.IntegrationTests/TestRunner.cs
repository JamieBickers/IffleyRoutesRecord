﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.IntegrationTests
{
    public class TestRunner
    {
        private readonly ClientApi clientApi;

        public TestRunner(ClientApi clientApi)
        {
            this.clientApi = clientApi;
        }

        public async Task GetAndAssertResultEqualsExpectedAsync<T>(Uri uri, T expectedResult)
        {
            var result = await clientApi.GetAsync<T>(uri);
            AssertAreEqual(result, expectedResult);
        }

        public async Task GetAndAssertResultSatisfiesPredicateAsync<T>(Uri uri, Predicate<T> predicate)
        {
            var result = await clientApi.GetAsync<T>(uri);
            AssertIsSatisfied(result, predicate);
        }

        public async Task PostAndAssertResultEqualsAsync<TContent, TResponse>(Uri uri, TContent content, TResponse expectedResult)
        {
            var result = await clientApi.PostAsync<TContent, TResponse>(uri, content);
            AssertAreEqual(result, expectedResult);
        }

        public async Task PostAndAssertIsSuccessfulAsync<TContent>(Uri uri, TContent content)
        {
            var result = await clientApi.PostAsync(uri, content);
            AssertIsSuccessful(result);
        }

        public async Task PostWithModelErrorsAsync<TContent>(Uri uri, TContent content, string expectedErrorJson)
        {
            var (errors, statusCode) = await clientApi.PostWithStatusCodeAsync<TContent>(uri, content);
            AssertAreEqual(statusCode, HttpStatusCode.BadRequest);
            AssertAreEqual(errors, expectedErrorJson);
        }

        private static void AssertAreEqual<T>(T first, T second)
        {
            string firstSerialized = Serialize(first);
            string secondSerialized = Serialize(second);

            if (!firstSerialized.Equals(secondSerialized, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotEqualException();
            }
        }

        private static void AssertIsSuccessful(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new ResponseNotSuccessfulException();
            }
        }

        private static void AssertIsSatisfied<T>(T result, Predicate<T> predicate)
        {
            if (!predicate(result))
            {
                throw new ConditionNotSatisfiedException();
            }
        }

        private static string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data).ToUpperInvariant();
        }
    }
}
