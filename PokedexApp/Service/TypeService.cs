using Newtonsoft.Json.Linq;
using PokedexApp.Data;
using System.Net.Http.Headers;

namespace PokedexApp.Service
{
    public class TypeService : IService<Models.Type>
    {
        private AppDbContext _context;
        private HttpClient _client;

        public TypeService(AppDbContext context)
        {
            _context = context;
            _client = new HttpClient();
			_client.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
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

						Models.Type type = new()
						{
							Id = (int)typeDetail["id"],
							Name = char.ToUpper(((string)typeDetail["name"])[0]) + ((string)typeDetail["name"])[1..]
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
