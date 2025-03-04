﻿using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CxRestClient_Tests")]
namespace CxRestClient
{

	public class CxRestContextBuilderCommon<T> : CxRestContextBuilderBase<T> where T : CxRestContextBuilderCommon<T>
	{
		internal override void Validate()
		{
            if (String.IsNullOrEmpty(User) )
                throw new InvalidOperationException("Username was not specified.");

            if (String.IsNullOrEmpty(Password))
                throw new InvalidOperationException("Password was not specified.");

            if (RetryLoop < 0)
                throw new InvalidOperationException("Retry loop can't be < 0.");

            if (Timeout < 0)
                throw new InvalidOperationException("Timeout can't be < 0.");

            if (String.IsNullOrEmpty(ApiUrl) )
                throw new InvalidOperationException("API URL was not specified.");

            if (!Uri.IsWellFormedUriString(ApiUrl, UriKind.Absolute))
                throw new InvalidOperationException("API URL is invalid.");
        }
    }
}
