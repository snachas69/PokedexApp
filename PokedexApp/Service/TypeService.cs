using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using PokedexApp.Data;
using System.Net.Http.Headers;

namespace PokedexApp.Service
{
    public class TypeService : IService<Models.Type>
    {
        private AppDbContext _context;
		private IConfiguration _configuration;
		private HttpClient _client;

        public TypeService(AppDbContext context, IConfiguration configuration)
        {
			_context = context;
			_configuration = configuration;

			_client = new HttpClient();
			_client.BaseAddress = new Uri(_configuration["ApiUrls:PokeApiBaseUrl"]);
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public List<Models.Type> GetDataFromApi()
		{
			List<Models.Type> typeList = new();
			HttpResponseMessage response = _client.GetAsync("type").Result;

			if (response.IsSuccessStatusCode)
			{
				string responseString = response.Content.ReadAsStringAsync().Result;
				JObject jsonResponse = JObject.Parse(responseString);
				JArray results = (JArray)jsonResponse["results"];

				foreach (JToken result in results)
				{
					string typeName = result["name"].ToString();
					HttpResponseMessage resultsResponse = _client.GetAsync("type/" + typeName).Result;

					if (response.IsSuccessStatusCode)
					{
						string detailData = resultsResponse.Content.ReadAsStringAsync().Result;
						JObject typeDetail = JObject.Parse(detailData);

						int id = (int)typeDetail["id"];
						string? name = (string?)typeDetail["name"];

						Models.Type type = new()
						{
							Id = id,
							Name = char.ToUpper(name[0]) + (name)[1..]
						};

						typeList.Add(type);
					}
				}
			}

			return typeList;
		}

		public IQueryable<Models.Type> GetDataFromDatabase()
		{
			throw new NotImplementedException();
		}
	}
}
