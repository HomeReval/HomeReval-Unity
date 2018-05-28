using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Kinect;

namespace HomeReval.Domain
{
    class ConvertedBody
    {
        public ConvertedBody(){
            /*float deltaX = x1 - x0;
            float deltaY = y1 - y0;
            float deltaZ = z1 - z0;

            float distance = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);*/
        }

        public List<Joint> CheckJoints{ get; set; }
        public List<JointResult> JointResults { get; set; }
        public DateTime Time { get; set; }
    }
}
