using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Kinect;

namespace HomeReval.Helpers
{
    class Map
    {
        private static readonly Dictionary<JointType, JointType> JointMappings 
            = new Dictionary<JointType, JointType>{
                { JointType.Head, JointType.Neck },
                { JointType.Neck, JointType.Head }
            };
    }
}
