using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WEB_Assignment.Models;

namespace WEB_Assignment.DAL
{
    public class CovidDAL
    {
        public async Task<CovidModel> getCovidStatusAsync()
        {
            string url = "https://api.covid19api.com";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);

            HttpResponseMessage response = await client.GetAsync("/summary");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                CovidModel covidModels = JsonConvert.DeserializeObject<CovidModel>(data);
                return covidModels;
            }
            else
            {
                //Else error cause not shit
                return null;
            }
        }

        public async Task<int> GetPopulation(string countryName)
        {
            string url = "https://restcountries.eu";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);

            HttpResponseMessage response = await client.GetAsync("/rest/v2/name/" + countryName + "?fullText=true");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                List<CountryDataModel> countryDataList = JsonConvert.DeserializeObject<List<CountryDataModel>>(data);
                int countryPopulation;
                if (countryDataList.Count == 1)
                {
                    countryPopulation = countryDataList[0].population;
                }
                else
                {
                    countryPopulation = 0;
                }
                return countryPopulation;
            }
            else
            {
                return 0;
            }
        }

        //get country code from country name
        private async Task<string> getCountryCode(string countryName)
        {
            string url = "https://restcountries.eu";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);

            HttpResponseMessage response = await client.GetAsync("/rest/v2/name/" + countryName + "?fullText=true");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                List<CountryDataModel> countryDataList = JsonConvert.DeserializeObject<List<CountryDataModel>>(data);
                string countryCode;
                if (countryDataList.Count == 1)
                {
                    countryCode = countryDataList[0].alpha2Code;
                }
                else
                {
                    countryCode = null;
                }
                return countryCode;
            }
            else
            {
                return null;
            }
        }

        //get link for country flag image
        public async Task<string> getCountryFlagLink(string countryName)
        {
            string countryCode = await getCountryCode(countryName);
            string imageSize = "64"; //size can be either 16, 24, 32, 48 or 64
            string flagLink;
            if (countryCode != null)
            {
                flagLink = String.Format("https://www.countryflags.io/{0}/flat/{1}.png", countryCode, imageSize);
            }
            else
            {
                flagLink = "";
            }
            return flagLink;
        }
    }
}