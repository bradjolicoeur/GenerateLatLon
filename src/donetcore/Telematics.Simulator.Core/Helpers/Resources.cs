using Telematics.Simulator.Core.Models.StatesPoly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Telematics.Simulator.Core.Helpers
{
    public static class Resources
    {
        public static States GetStatePoly()
        {
            using (TextReader reader = new StreamReader(@"resources/states.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(States));
                return (States)serializer.Deserialize(reader);
            }
        }
    }
}
