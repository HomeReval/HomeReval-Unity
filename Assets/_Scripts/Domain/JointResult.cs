using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Kinect;

namespace HomeReval.Domain
{
    class JointResult
    {
        public Joint CurrentJoint { get; set; }
        public Joint TargetJoint { get; set; }
        public float Distance { get; set; }
        public float Angle { get; set; }
    }
}
