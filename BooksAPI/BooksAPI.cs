using BookAPIDataCollection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Configuration;
using System.Collections.Specialized;
using NUnit.Framework;
using FluentAssertions;
using FluentAssertions.Json;

namespace BooksRestAPI
{
    public class BooksAPI
    {
        int numOfBooks = 0;
        string baseURL = ConfigurationManager.AppSettings.Get("baseURL");
        string bookTitle = ConfigurationManager.AppSettings.Get("bookTitle");
        string bookTitleCompare = ConfigurationManager.AppSettings.Get("bookTitleCompare");
        List<string> bookIDs = new List<string>();

        //takes the GET request as func param
        public string getAPIResponse(string restRequest)
        {
            //establish new client and request for API
            var client = new RestClient(baseURL);
            var request = new RestRequest(restRequest);
            return client.ExecuteAsync(request).Result.Content; //returns the entire response(json data) as string
        }

        public void getBooksByTitle()
        {
            //gets the api response of the given request as paramter and conver to string
            string result = getAPIResponse("search.json?title=" + bookTitle + "");

            //parse the json data and extract the first seed in the docs array that consists of books and their ids matching the title
            var myDetails = JsonConvert.DeserializeObject<Rootobject>(result);

            //loop through each set of books in the docs array
            for (int i = 0; i < myDetails.docs.Length; i++)
            {
                //check if the title matches exactly and inc the counter
                if (myDetails.docs[i].title.Equals(bookTitle))
                {
                    numOfBooks++;
                }

                //check if the book's publish year 2000+ and grab its key value
                if(myDetails.docs[i].first_publish_year >= 2000)
                {
                    bookIDs.Add(myDetails.docs[i].key);
                }
            }

            Console.WriteLine("Total number of books matching the title '" + bookTitleCompare + "': " + numOfBooks);
            Console.WriteLine("Keys of books published since 2000:");
            foreach (var key in bookIDs)
            {
                Console.WriteLine(key);
            }

        }

        [Test]
        public void compareAPIResponseData()
        {
            var expectedData = JToken.Parse(File.ReadAllText(@"C:\Users\B\Desktop\BooksAPI\BooksAPI\CompareData.json"));

            string apiResponse = getAPIResponse("search.json?title=" + bookTitleCompare + ""); //call the request and get the response to 
            var actualData = JToken.Parse(apiResponse); //parses the content of response to JObject so it can be compared with the .json file

            Console.WriteLine("Actual data: \n" + actualData);
            Console.WriteLine("Expected data: \n" + expectedData);

            actualData.Should().BeEquivalentTo(expectedData);

        }

    } 

}