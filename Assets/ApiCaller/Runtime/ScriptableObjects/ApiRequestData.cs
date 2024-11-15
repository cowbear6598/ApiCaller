using ApiCaller.Common;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ApiCaller.ScriptableObjects
{
	[CreateAssetMenu(fileName = "ApiRequestData", menuName = "ApiCaller/ApiRequestData")]
	public class ApiRequestData : ScriptableObject
	{
		public Method          Method;
		public string          Domain;
		public string          Api;
		public int             Timeout = 30;
		public RequestHeader[] Headers;
		public Query[]         Queries;

		public string Url => Domain + Api;

		public UniTask Execute()
			=> new ApiRequest(this).Execute();

		public UniTask<TResponse> Execute<TResponse>() where TResponse : struct
			=> new ApiRequest(this).Execute<TResponse>();

		public UniTask<TResponse> Execute<TRequest, TResponse>(TRequest data) where TRequest : struct where TResponse : struct
			=> new ApiRequest(this).Execute<TRequest, TResponse>(data);
	}
}