using BookAPIDataCollection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Xunit;

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
            api.getBooksByTitle("Goodnight+Moon");
            api.compareAPIResponseData("Goodnight+Moon+123+Lap+Edition");
            
        }

        public string trimTitle(string title)
        {
            //removes any + signs from a string and replaces with a space
            string[] charsToRemove = new string[] { "+" };
            foreach (var c in charsToRemove)
            {
                title = title.Replace(c, " ");
            }
            return title;
        }

        public RestResponse getAPIResponse(string restRequest)
        {
            //establish new client and request for API
            var client = new RestClient(baseURL);
            var request = new RestRequest(restRequest);
            return client.ExecuteAsync(request).Result; //returns the entire response(json data) as string
        }

        public void getBooksByTitle(string title)
        {
            //gets the api response of the given request as paramter and conver to string
            string result = getAPIResponse("search.json?title=" + title + "").Content;

            //parse the json data and extract the first seed in the docs array that consists of books and their ids matching the title
            var myDetails = JsonConvert.DeserializeObject<Rootobject>(result);
            Console.WriteLine(myDetails);

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

        public void compareAPIResponseData(String title)
        {
            var o1 = JObject.Parse(File.ReadAllText(@"C:\Users\B\Desktop\BooksAPI\BooksAPI\CompareData.json"));

            RestResponse apiResponse = getAPIResponse("search.json?title=" + title + ""); //call the request and get the response to compare

            var o2 = JObject.Parse(apiResponse.Content); //parses the content of response to JObject so it can be compared with the .json file

            if (o1.Equals(o2))
            {
                Console.WriteLine("Response from the GET request matches");
            }
            else Console.WriteLine("Response from the GET request does not match");

        }

    }

    

}