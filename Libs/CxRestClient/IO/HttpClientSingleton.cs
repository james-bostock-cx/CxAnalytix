﻿using log4net;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Linq;

[assembly: InternalsVisibleTo("CxRestClient_Tests")]
namespace CxRestClient.IO
{
    internal class HttpClientSingleton
    {
        private static HttpClient _client = null;
        private static readonly Object _lock = new object();
        private static ProductInfoHeaderValue _userAgent;

        private static ILog _log = LogManager.GetLogger(typeof(HttpClientSingleton));

		static HttpClientSingleton()
		{
            var assembly = System.Reflection.Assembly.GetEntryAssembly();

            String companyName = "Checkmarx";
            String productName = "CxAnalytix";
            String productVersion = "0.0.0";

            _userAgent = new ProductInfoHeaderValue($"{companyName}-{productName}", productVersion);

            if (assembly != null)
            {
				var companyAttrib = assembly.CustomAttributes.FirstOrDefault((x) => x.AttributeType == typeof(System.Reflection.AssemblyCompanyAttribute));
				if (companyAttrib != null)
					companyName = companyAttrib.ConstructorArguments[0].ToString().Replace("\"", "");

				var productAttrib = assembly.CustomAttributes.FirstOrDefault((x) => x.AttributeType == typeof(System.Reflection.AssemblyProductAttribute));
				if (productAttrib != null)
					productName = productAttrib.ConstructorArguments[0].ToString().Replace("\"", "");

				var versionAttrib = assembly.CustomAttributes.FirstOrDefault((x) => x.AttributeType == typeof(System.Reflection.AssemblyInformationalVersionAttribute));
				if (versionAttrib != null)
					productVersion = versionAttrib.ConstructorArguments[0].ToString().Replace("\"", "");
			}


            try
            {
                _userAgent = new ProductInfoHeaderValue($"{companyName}-{productName}", productVersion);
                _log.Debug($"User Agent: {_userAgent}");
            }
            catch (Exception)
			{
                // Attempting to assign values such as "Microsoft Corporation" causes the
                // user agent class to throw an exception.
			}
        }


        private HttpClientSingleton()
        { }

        public static void Clear()
        {
            lock (_lock)
                if (_client != null)
                {
                    _client.Dispose();
                    _client = null;
                }
        }


        public static void Initialize(bool doSSLValidate, TimeSpan opTimeout)
        {
            lock (_lock)
            {
                if (_client != null)
                    return;

                HttpClientHandler h = GetClientHandler();
                if (!doSSLValidate)
                    h.ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) => true;


                _client = new HttpClient(h, true);
                _client.Timeout = opTimeout;
                _client.DefaultRequestHeaders.UserAgent.Add(_userAgent);
            }
        }

        private static HttpClientHandler GetClientHandler ()
        {
            if (!_log.IsNetworkTrace())
                return new HttpClientHandler();
            else
                return new LoggingClientHandler();



        }

        public static HttpClient GetClient()
        {
            lock(_lock)
				if (_client == null)
					throw new InvalidOperationException("HttpClient has not been initialized.");

			return _client;
        }

    }
}
