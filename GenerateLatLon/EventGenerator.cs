using GenerateLatLon.Interfaces;
using GenerateLatLon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private readonly Random _rand = new Random();

        public IEnumerable<IPosition> Generate(IPosition position)
        {
            var results = new List<IPosition>();

            eventType typeID = (eventType)_rand.Next(4);

            switch (typeID)
            {
                case eventType.HardBraking:
                   results.Add(GenerateHardBreaking(position));
                   break;

                case eventType.HardAcceleration:
                    results.Add(GenerageHardAcceleration(position));
                    break;

                case eventType.Seatbelt:
                    results.Add(GenerateSeatbelt(position));
                    break;

                case eventType.Diagnostics:
                    results.Add(GenerateDiagnostics(position));
                    break;

                case eventType.Idling:
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
