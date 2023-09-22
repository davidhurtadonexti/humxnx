using System;
namespace pkg.Attributes
{
	public class General
	{
        public class ResponseData<T>
		{
			public string? message { get; set; }
			public T? data { get; set; }
        }

	}
}

