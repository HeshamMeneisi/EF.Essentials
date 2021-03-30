using System.Collections.Generic;
using System.Text;
using System.Web;

namespace GenericCompany.Common.Helpers
{
    public class UrlBuilder
    {
        private string _baseUrl;
        private string _protocol;
        private string _domain;
        private string _subdomain;
        private List<string> _pathParts;
        private Dictionary<string, string> _attributes;

        public UrlBuilder()
        {
            _pathParts = new List<string>();
            _attributes = new Dictionary<string, string>();
        }

        public UrlBuilder WithBase(string baseUrl)
        {
            _baseUrl = baseUrl;
            return this;
        }

        public UrlBuilder WithProtocol(string protocol)
        {
            _protocol = protocol;
            return this;
        }

        public UrlBuilder WithDomain(string domain)
        {
            _domain = domain;
            return this;
        }

        public UrlBuilder WithSubdomain(string subdomain)
        {
            _subdomain = subdomain;
            return this;
        }

        public UrlBuilder WithPath(string path)
        {
            _pathParts.Add(path);
            return this;
        }

        public UrlBuilder WithAttribute(string key, string value)
        {
            _attributes[key] = value;
            return this;
        }

        public string BaseUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_baseUrl)) return _baseUrl;
                var p = string.IsNullOrEmpty(_protocol) ? "https" : _protocol;
                var s = string.IsNullOrEmpty(_subdomain) ? "" : _subdomain + ".";
                return $"{p}://{s}{_domain}";
            }
        }

        public string Build()
        {
            var s = new StringBuilder();

            s.Append(BaseUrl);

            foreach (var part in _pathParts)
            {
                s.Append($"/{part}");
            }

            if (_attributes.Count > 0)
            {
                s.Append("?");
                foreach (var kv in _attributes)
                {
                    s.Append($"{kv.Key}={HttpUtility.UrlEncode(kv.Value)}&");
                }
            }

            return s.ToString().TrimEnd('&');
        }
    }
}
