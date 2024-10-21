using ApiCaller.Domain;
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

		public string Url => Domain + Api;
	}
}