using BooksRestAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksAPI_Inti
{
    class BooksAPI_Inti
    {
        static void Main(String[] args)
        {
            BooksAPI api = new BooksAPI();
            api.getBooksByTitle();
            api.printBookDetails();
            api.compareAPIResponseData();
        }
    }
    
}
