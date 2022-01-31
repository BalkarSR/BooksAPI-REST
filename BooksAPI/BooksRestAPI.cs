using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Threading;

namespace BooksRestAPI
{
    public class BooksAPI
    {
        int numOfBooks { get; set; }

        static void Main(String[] args)
        {
            BooksAPI api = new BooksAPI();
            api.getBooksByExactTitle("Goodnight+Moon");
            Console.WriteLine(api.numOfBooks);
        }

        public string trimTitle(string title)
        {
            string[] charsToRemove = new string[] { "+" };
            foreach (var c in charsToRemove)
            {
                title = title.Replace(c, " ");
            }
            return title;
        }
        public void getBooksByExactTitle(string title)
        {
            numOfBooks = 0;
            //set base url
            var restClient = new RestClient("https://openlibrary.org/");
            //create the GET request and record it in the response variable
            var request = new RestRequest("search.json?title=" + title + "");
            var response = restClient.ExecuteGetAsync(request).Result;

            //checks for status code 200
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string result = response.Content;

                //parse the json data and extract the first seed in the docs array that consists of books and their ids matching the title
                var myDetails = JsonConvert.DeserializeObject<Rootobject>(result);

                for(int i = 0; i < myDetails.docs.Length; i++)
                {
                    if (myDetails.docs[i].title.Equals(trimTitle(title)))
                    {
                        numOfBooks++;
                    }
                }
                
            }

        }

    }

    public class Rootobject
    {
        public Doc[] docs { get; set; }
    }

    public class Doc
    {
        public string key { get; set; }
        public string title { get; set; }
        public int[] publish_year { get; set; }
    }

}