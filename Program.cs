using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

using Newtonsoft.Json;


namespace SW
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();


            Console.WriteLine("Please input the distance in MGLT....");
            string inputDistance = Console.ReadLine();
            Console.WriteLine("Star Wars starships are coming. They had to make several stops. Please wait");
            Console.WriteLine();

            string test = p.getResponse("https://swapi.co/api/starships");

            //keep console open.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }


        public string getResponse(string url)
        {
            WebRequest req = WebRequest.Create(url);

            Stream str = req.GetResponse().GetResponseStream();

            //read data
            StreamReader r = new StreamReader(str);
            string json = r.ReadToEnd();
            r.Dispose();

            return json;
        }
    }



    public class starship : entity
    {
        public string name { get; set; }
        public string MGLT { get; set; }
        public string consumables { get; set; }

        public string consumables_hours { get; set; }
        public string distance { get; set; }
        public string noStopsPerDistance { get; set; }
    }

    public class dictionary<T> : entity where T : entity
    {
        public string next { get; set; }
        public List<T> results { get; set; }
    }

    public abstract class entity
    {
    }

}
