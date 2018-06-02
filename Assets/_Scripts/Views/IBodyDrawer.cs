using System.Collections.Generic;
using Windows.Kinect;

namespace Views
{
    public interface IBodyDrawer
    {
        void DrawSkeleton(Dictionary<JointType, Joint> joints);
        void Untracked();
        bool Tracked { get; }
    }
}
