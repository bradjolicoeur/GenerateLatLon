﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telematics.Simulator.Models.Interfaces
{
    public interface IBehaviorEvent : IDeviceEvent
    {
        string Label { get; }
    }
}
