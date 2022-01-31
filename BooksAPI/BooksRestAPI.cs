using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Threading;

namespace BooksRestAPI
{
    public class BooksAPI
    {
        int numOfBooks = 0;
        string baseURL = "https://openlibrary.org/";
        List<string> bookIDs = new List<string>();

        static void Main(String[] args)
        {
            BooksAPI api = new BooksAPI();
            api.getBooksByExactTitle("Goodnight+Moon");
            
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

        public string getAPIResponse(string restRequest)
        {
            var client = new RestClient(baseURL);
            var request = new RestRequest(restRequest);
            return client.ExecuteAsync(request).Result.Content; //returns the json as string
        }

        public void getBooksByExactTitle(string title)
        {
            //gets the api response of the given request as paramter
            string result = getAPIResponse("search.json?title=" + title + "");

            //parse the json data and extract the first seed in the docs array that consists of books and their ids matching the title
            var myDetails = JsonConvert.DeserializeObject<Rootobject>(result);

            //loop through each set of books in the docs array
            for (int i = 0; i < myDetails.docs.Length; i++)
            {
                //check if the title matches exactly and inc the counter
                if (myDetails.docs[i].title.Equals(trimTitle(title)))
                {
                    numOfBooks++;
                }

                //check if the book's publish year 2000+ and grab its key value
                if(myDetails.docs[i].first_publish_year >= 2000)
                {
                    bookIDs.Add(myDetails.docs[i].key);
                }
            }


            Console.WriteLine("Total number of books matching the title '" + trimTitle(title) + "': " + numOfBooks);
            Console.WriteLine("Keys of books published since 2000:");
            foreach (var key in bookIDs)
            {
                Console.WriteLine(key);
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
        public int first_publish_year { get; set; }
    }

}