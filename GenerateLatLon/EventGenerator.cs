using GenerateLatLon.Interfaces;
using GenerateLatLon.Models;
using GenerateLatLon.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GenerateLatLon
{

    public class EventGenerator : IEventGenerator
    {
        private enum eventType : int
        {
            None,
            HardBraking,
            Diagnostics,
            Seatbelt,
            HardAcceleration,
            Idling
        }

        private IEnumerable<EventType> _eventList;

        private readonly Random _rand = new Random();

        public EventGenerator()
        {
            InitializeEventList();
        }

        private void InitializeEventList()
        {
            _eventList = new List<EventType>()
            {              
                new EventType { Name = "Seatbelt"},
                new EventType { Name = "HardAcceleration"},
                new EventType { Name = "HardBraking"},
                new EventType { Name = "Idling"},
                new EventType { Name = "Diagnostics"},
                new EventType { Name = "None"}
            };

            double totalWeight = _eventList.Count();

            int index = 1;
            foreach(var item in _eventList)
            {
                item.Weight = index;
                item.PercentOutcome = item.Weight / totalWeight;
                index++;
            }

            foreach(var item in _eventList)
            {
                Debug.WriteLine($"event {item.Name} weight {item.Weight} percent outcome {item.PercentOutcome}");
            }
        }


        public IEnumerable<IPosition> Generate(IPosition position)
        {
            var results = new List<IPosition>();

            eventType typeID = (eventType)_rand.Next(4);

            var type = _eventList.OrderBy(e => e.PercentOutcome).RandomElementByWeight(e => e.PercentOutcome);

            switch (type.Name)
            {
                case "HardBraking":
                   results.Add(GenerateHardBreaking(position));
                   break;

                case "HardAcceleration":
                    results.Add(GenerageHardAcceleration(position));
                    break;

                case "Seatbelt":
                    results.Add(GenerateSeatbelt(position));
                    break;

                case "Diagnostics":
                    results.Add(GenerateDiagnostics(position));
                    break;

                case "Idling":
                    results.AddRange(GenerateIdling(position));
                    break;

                default:
                    break;
            }

            return results;
        }


        private IEnumerable<Position> GenerateIdling(IPosition position)
        {
            var list = new List<Idling>();
            var instances = _rand.Next(10);
            for (int i = 0; i < instances-1; i++)
            {
                position.UtcPositionTime = position.UtcPositionTime.AddMinutes(_rand.Next(5));
                list.Add(new Idling(position));
            }

            return list;
        }

        private Position GenerateDiagnostics(IPosition position)
        {
            return new DiagnosticEvent(position);
        }

        private Position GenerateSeatbelt(IPosition position)
        {
            return new Seatbelt(position);
        }

        private Position GenerageHardAcceleration(IPosition position)
        {
            return new HardAcceleration(position);
        }

        private Position GenerateHardBreaking(IPosition position)
        {
            return new HardBraking(position);
        }

        
    }
}
