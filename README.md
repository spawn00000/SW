# SW

STAR WARS starships travel

I used the API available here: https://swapi.co/  
We want to know for all SW star ships, to cover a given distance, how many stops for resupply are required.

The input is a distance in mega lights (MGLT).
The output is a collection of all the star ships and the total amount of stops required to make the distance between the planets.

Assumption1 (about cargo)
We cannot know if the ships have a full cargo before they begin to travel that distance, because we do not have an attribute for cargo level. 
One solution would be - at the end - to add one additional stop to every ship so we can cover the worst case scenario (aka cargo empty).
For not over-complicating the problem, we will assume that at the starting point they are full.

Assumption2 (about passengers)
We cannot know if the ships have any non-essential passenger before they begin to travel that distance, because we do not have an attribute for passenger level.
The worst scenario would be that the ship is at full passenger capacity and nobody gets off at any stops - but we will need to adjust the consumables field (or create a new one for crew + passengers).
For not over-complicating the problem, we will assume that at the starting point, in the ship only the crew is present and nobody gets off/on during the stops (even if in star wars universe that would be very hard to achieve).

Assumption3 (about time) 
We will assume that all the times values refer to Earth calendar.
We also assume that the year, month, day of the starting point for the ships it is unknown (because the problem stated that only a distance is given as input). 
So we will have to make small approximations like: 1 year=8760hours; 1 month=730hours (1 Earth week always has 168 hours and 1 Earth day has 24 hours - no approximation here). It is not so relevant if it a leap year or if the month is August or February.

The application has 4 functions:

1. readInput - gets input from user for starships' distance (recursive until it gets a positive number)

2. getResponse - tries to get content from a URL

3. getAllStarships - gets data for all the starships is getResponse is valid. (recursive until next page of starships equals null). It parses the json string (for each page) returned by getResponse function until all the pages about starships are read. Adds starships to a list of starships.

4. findStopsNumber - contains the algorithm for getting the stops for each starship in the list of starships previously created.


Classes used: Starship class that inherits an abstract class entity. Dictionary class used for parsing the json.

Details about the application: Type = console application; IDE used = Microsoft Visual Studio 2010; DLLs used: Newtonsoft.Json

Instructions for usage: Download SW.exe and Newtonsoft.Json.dll from Debug folder. Run SW.exe. Write an input for the distance and press ENTER.

