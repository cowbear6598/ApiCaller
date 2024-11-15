using System;

namespace ApiCaller.Common
{
	public enum Header
	{
		Accept,
		Authorization,
		ContentType,
		ContentLength,
	}

	[Serializable]
	public class RequestHeader
	{
		public Header header;
		public string value;

		public string GetHeaderKey()
		{
			return header switch
			{
				Header.Accept        => "accept",
				Header.Authorization => "authorization",
				Header.ContentType   => "content-type",
				Header.ContentLength => "content-length",
				_                    => "",
			};
		}
	}
}