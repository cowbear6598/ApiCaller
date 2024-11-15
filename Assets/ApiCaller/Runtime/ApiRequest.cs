using System;
using System.Text;
using ApiCaller.Common;
using ApiCaller.ScriptableObjects;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace ApiCaller
{
	internal class ApiRequest
	{
		private readonly UnityWebRequest _req;

		public ApiRequest(ApiRequestData data)
		{
			_req                 = new UnityWebRequest();
			_req.downloadHandler = new DownloadHandlerBuffer();

			_req.url     = data.Url;
			_req.method  = data.Method.ToString();
			_req.timeout = data.Timeout;

			foreach (var header in data.Headers)
				_req.SetRequestHeader(header.GetHeaderKey(), header.value);

			foreach (var query in data.Queries)
				AddQuery(query.key, query.value);
		}

		public async UniTask Execute()
		{
			await _req.SendWebRequest();

			if (_req.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
				throw new Exception(_req.error);
		}

		public async UniTask<TResponse> Execute<TResponse>() where TResponse : struct
		{
			await _req.SendWebRequest();

			if (_req.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
				throw new Exception(_req.error);

			return JsonUtility.FromJson<TResponse>(_req.downloadHandler.text);
		}

		public async UniTask<TResponse> Execute<TRequest, TResponse>(TRequest bodyData) where TRequest : struct where TResponse : struct
		{
			SetBody(bodyData);

			await _req.SendWebRequest();

			if (_req.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
				throw new Exception(_req.error);

			return JsonUtility.FromJson<TResponse>(_req.downloadHandler.text);
		}

		private void AddQuery(string key, string value)
		{
			if (_req.url.Contains("?"))
				_req.url += "&";
			else
				_req.url += "?";

			_req.url += $"{key}={value}";
		}

		private ApiRequest SetBody<TRequest>(TRequest data) where TRequest : struct
		{
			var jsonRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));

			_req.uploadHandler = new UploadHandlerRaw(jsonRaw);

			return this;
		}
	}
}