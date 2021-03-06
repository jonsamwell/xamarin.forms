﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Redbridge.SDK
{
	public class JsonWebRequestFunc<TResult, TInput1> : JsonWebRequest
	{
		public JsonWebRequestFunc(string requestUri, HttpVerb httpVerb)	: base(requestUri, httpVerb) { }
		
		public async Task<TResult> ExecuteAsync(TInput1 input1)
		{
			OnExtractParameters(input1);
			using (var response = await OnExecuteRequestAsync(input1))
			{
				if (!response.IsSuccessStatusCode)
					await response.ThrowResponseException();

				return await OnReadResultBody(response);
			}
		}

        protected virtual string OnCreatePayload (TInput1 input)
        {
            return base.OnCreatePayload(input);
        }

        protected override string OnCreatePayload(object body)
        {
            return OnCreatePayload((TInput1)body);
        }

		protected virtual void OnExtractParameters(TInput1 input1) { }

		protected virtual async Task<TResult> OnReadResultBody(HttpResponseMessage responseMessage)
		{
			var json = await responseMessage.Content.ReadAsStringAsync();
			if (json == null) throw new NotSupportedException("The resultant stream is null.");

			using (var reader = new StringReader(json))
			{
				var serializer = new JsonSerializer();
				OnAddConverters(serializer);

				using (var jsonReader = new JsonTextReader(reader))
				{
					return serializer.Deserialize<TResult>(jsonReader);
				}
			}
		}
	}

	public class JsonWebRequestFunc<TResult> : JsonWebRequest
	{
		public JsonWebRequestFunc(Uri baseUri, string requestUri, HttpVerb httpVerb)
			: this(requestUri, httpVerb)
		{
			if (baseUri == null) throw new ArgumentNullException(nameof(baseUri));
			RootUri = baseUri;
		}

		public JsonWebRequestFunc(string requestUri, HttpVerb httpVerb)
			: base(requestUri, httpVerb) { }

		public async Task<TResult> ExecuteAsync()
		{
			using (var response = await OnExecuteRequestAsync())
			{
				if (!response.IsSuccessStatusCode)
					await response.ThrowResponseException();
				
				return await OnReadResultBody(response);
			}
		}

		protected virtual async Task<TResult> OnReadResultBody(HttpResponseMessage responseMessage)
		{
			var json = await responseMessage.Content.ReadAsStringAsync();
			if (json == null) throw new NotSupportedException("The resultant stream is null.");

			using (var reader = new StringReader(json))
			{
				var serializer = new JsonSerializer();
				OnAddConverters(serializer);

				using (var jsonReader = new JsonTextReader(reader))
				{
					return serializer.Deserialize<TResult>(jsonReader);
				}
			}
		}
	}
}
