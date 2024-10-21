using System;
using System.Text;
using ApiCaller.ScriptableObjects;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace ApiCaller.Domain
{
	public class ApiRequest
	{
		private readonly UnityWebRequest _req;

		public ApiRequest()
		{
			_req                 = new UnityWebRequest();
			_req.downloadHandler = new DownloadHandlerBuffer();
		}

		public async UniTask<TResponse> Execute<TResponse>()
		{
			await _req.SendWebRequest();

			if (_req.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
				throw new Exception(_req.error);

			return JsonUtility.FromJson<TResponse>(_req.downloadHandler.text);
		}

		public async UniTask<TResponse> SimpleRequest<TResponse>(ApiRequestData data)
		{
			UseApiRequestData(data);

			return await Execute<TResponse>();
		}

		#region Builder

		public ApiRequest UseApiRequestData(ApiRequestData data)
		{
			_req.url     = data.Url;
			_req.method  = data.Method.ToString();
			_req.timeout = data.Timeout;

			foreach (var header in data.Headers)
				_req.SetRequestHeader(header.GetHeaderKey(), header.value);

			return this;
		}

		public ApiRequest SetUrl(string url)
		{
			_req.url = url;

			return this;
		}

		public ApiRequest SetMethod(Method method)
		{
			_req.method = method.ToString();

			return this;
		}

		public ApiRequest SetTimeout(int timeout)
		{
			_req.timeout = timeout;

			return this;
		}

		public ApiRequest AddHeader(string key, string value)
		{
			_req.SetRequestHeader(key, value);

			return this;
		}

		public ApiRequest AddQuery(string key, string value)
		{
			if (_req.url.Contains("?"))
				_req.url += "&";
			else
				_req.url += "?";

			_req.url += $"{key}={value}";

			return this;
		}

		public ApiRequest SetBody<TRequest>(TRequest data)
		{
			var jsonRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));

			_req.uploadHandler = new UploadHandlerRaw(jsonRaw);

			return this;
		}

		public UnityWebRequest Build()
		{
			return _req;
		}

		#endregion
	}
}