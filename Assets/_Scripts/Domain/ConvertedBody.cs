using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Kinect;

namespace HomeReval.Domain
{
    class ConvertedBody
    {
        public List<Joint> CheckJoints{ get; set; }
        public List<JointResult> JointResults { get; set; }
        public DateTime Time { get; set; }
    }
}
