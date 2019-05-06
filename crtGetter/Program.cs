using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace crtGetter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, please enter a domain");
            var input = Console.ReadLine();
            var certList = GetCerts(input);

            foreach (var cert in certList)
            {
                DateTime.TryParse(cert.not_after, out var expDate);
                if (expDate > DateTime.Now)
                {
                    Console.WriteLine($"[+] {cert.name_value}, expires on {cert.not_after}");
                }
            }
        }


        static List<CertInfo> GetCerts(string site)
        {
            var requestUrl = $"https://crt​.sh/?q=%.{site}&output=json";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("cert bot");
            var response = httpClient.GetAsync(requestUrl);

            var data = response.Result.Content.ReadAsStringAsync().Result;
            var certsList = JsonConvert.DeserializeObject<List<CertInfo>>(data);

            return certsList;
        }
    }


    class CertInfo
    {
        public string issuer_ca_id { get; set; }
        public string issuer_name { get; set; }
        public string name_value { get; set; }
        public string min_cert_id { get; set; }
        public string min_entry_timestamp { get; set; }
        public string not_before { get; set; }
        public string not_after { get; set; }
    }
}
