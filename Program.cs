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
        //inits from our approximations (check README file)
        public const int hoursPerYear = 8760;
        public const int hoursPerMonth = 730;
        public const int hoursPerWeek = 168;
        public const int hoursPerDay = 24;


        static void Main(string[] args)
        {
            Console.WindowHeight = 60; //increase height of the console - to view all the starships without scroll
            Program p = new Program();


            Console.WriteLine("Please input the distance in MGLT....");
            //test input!! check for positive number >0
            string inputDistance = Console.ReadLine();
            Console.WriteLine("Star Wars starships are coming. They had to make several stops. Please wait");
            Console.WriteLine();                        

            
            List<starship> starships = new List<starship>();
            string urlPiece = "starships";
            string url = "https://swapi.co/api/" + urlPiece;

            //get information about starships
            p.getAllStarships(ref starships, url);

            //algorithm
            p.findStopsNumber(ref starships, inputDistance);
            


            //printing the results in the format required
            for (int i = 0; i < starships.Count; i++)
            {
                starship SS = starships[i];
                Console.WriteLine(SS.name + ": " + SS.noStopsPerDistance);
            }

            //keep console open.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }


        public void findStopsNumber(ref List<starship> starships, string inputDistance)
        {
            for (int i = 0; i < starships.Count; i++)
            {
                starship SS = starships[i];
                SS.distance = inputDistance;

                //parse consumables
                //transform into hours because MGLT is the distance per hour

                string[] c = SS.consumables.Split(' ');
                string consumables_units = c[0];
                if (consumables_units.Equals("unknown"))
                {
                    //check if we know the value of consumables and set accordingly
                    SS.consumables_hours = "unknown";
                    SS.noStopsPerDistance = "unknown";
                }
                else
                {
                    string consumables_UoM = c[1];

                    if (consumables_UoM.ToLower().Contains("year")) SS.consumables_hours = (Convert.ToInt32(consumables_units) * hoursPerYear).ToString();
                    if (consumables_UoM.ToLower().Contains("month")) SS.consumables_hours = (Convert.ToInt32(consumables_units) * hoursPerMonth).ToString();
                    if (consumables_UoM.ToLower().Contains("week")) SS.consumables_hours = (Convert.ToInt32(consumables_units) * hoursPerWeek).ToString();
                    if (consumables_UoM.ToLower().Contains("day")) SS.consumables_hours = (Convert.ToInt32(consumables_units) * hoursPerDay).ToString();

                    if (SS.MGLT.Equals("unknown"))
                    {
                        //check if we know the value of MGLT
                        SS.noStopsPerDistance = "unknown";
                    }
                    else
                    {

                        if (SS.MGLT.Equals("0"))
                        {
                            //protect against divide by 0
                            SS.noStopsPerDistance = "ship can teleport (has a blink drive) or cannot move from starting point";
                        }
                        else if (SS.consumables_hours.Equals("0"))
                        {
                            //protect against divide by 0
                            SS.noStopsPerDistance = "ship does not need living organisms and fuel to travel";
                        }
                        else
                        {
                            //truncate for getting the integer part of the double result 
                            SS.noStopsPerDistance = Math.Truncate(Convert.ToDouble(SS.distance) / (Convert.ToDouble(SS.MGLT) * Convert.ToDouble(SS.consumables_hours))).ToString();
                        }
                    }
                }

            }
        }


        public string getResponse(string url)
        {
            //to do tests
            WebRequest req = WebRequest.Create(url);

            Stream str = req.GetResponse().GetResponseStream();

            //read data
            StreamReader r = new StreamReader(str);
            string json = r.ReadToEnd();
            r.Dispose();

            return json;
        }


        public void getAllStarships(ref List<starship> starships, string url)
        {
            string json = getResponse(url);
            //parse json
            dictionary<starship> query = JsonConvert.DeserializeObject<dictionary<starship>>(json);

            starships.AddRange(query.results);
            if (query.next != null)
                getAllStarships(ref starships, query.next);

        }

        //public void getAllEntities(ref List<entity> entities, string url)
        //{
        //    string json = getResponse(url);
        //    //parse json
        //    dictionary<starship> query = JsonConvert.DeserializeObject<dictionary<starship>>(json);

        //    entities.AddRange(query.results);
        //    if (query.next != null)
        //        getAllEntities(ref entities, query.next);

        //    //dynamic jsond = JsonConvert.DeserializeObject(json);
        //    //string test = jsond.results[0].name;

        //}
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
