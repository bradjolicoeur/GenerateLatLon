using System;
using System.Collections.Generic;
using System.Text;
using Telematics.Simulator.Core.Interfaces;

namespace Telematics.Simulator.Core.Factories
{
    public class RandomFactory : IRandomFactory
    {
        private Random _random = new Random();

        public Random Create()
        {
            return _random;
        }
    }
}
