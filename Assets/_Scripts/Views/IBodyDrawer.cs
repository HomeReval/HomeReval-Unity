using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

namespace Views
{
    public interface IBodyDrawer
    {
        void DrawSkeleton(Body body);
        void Untracked();
        bool Tracked { get; }
    }
}
