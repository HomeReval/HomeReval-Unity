using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeReval.Domain
{
    public class Exercise
    {
        private List<ExerciseRecording> exerciseRecordings = new List<ExerciseRecording>();

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<ExerciseRecording> ExerciseRecordings { get { return exerciseRecordings; } }

        public int Amount { get; set; }

    }
}
