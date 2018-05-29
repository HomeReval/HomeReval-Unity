using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeReval.Domain
{
    public class ExerciseRecording
    {
        private List<ConvertedBody> convertedBodies = new List<ConvertedBody>();
        public List<ConvertedBody> ConvertedBodies { get {return convertedBodies; } }
    }
}
