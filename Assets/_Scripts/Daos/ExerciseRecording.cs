using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeReval.Daos
{
    public class ExerciseRecording
    {
        private List<ExerciseFrame> exerciseFrames = new List<ExerciseFrame>();
        public List<ExerciseFrame> ExerciseFrames { get {return exerciseFrames ;} }
    }
}
