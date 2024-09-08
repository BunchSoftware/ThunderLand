using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeNetwork.Server.Type.MapService
{
    public class World
    {
        private List<Location> locations = new List<Location>();
        public List<Location> Locations { get { return locations; } }

        public World() 
        {
            locations = new List<Location>()
            {
                new Location(){ IDLocation = 1},
                new Location(){ IDLocation = 2},
                new Location(){ IDLocation = 3},
                new Location(){ IDLocation = 4},
            };
        }

        public Location FindLocationByID(int IDLocation)
        {
            Location location = null;
            for (int i = 0; i < locations.Count; i++)
            {
                if (locations[i].IDLocation == IDLocation)
                    location = locations[i];
            }
            return location;
        }
    }
}
