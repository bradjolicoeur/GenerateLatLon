﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateLatLon.Interfaces
{
    public interface IDeviceEvent
    {
        DateTime UtcPositionTime { get; set; }
    }
}
