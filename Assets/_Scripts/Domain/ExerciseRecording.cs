using HomeReval.Validator;
using System.Collections.Generic;
//using HomeReval.Validator.Map;

namespace HomeReval.Domain
{
    public class ExerciseRecording
    {
        //private List<ConvertedBody> convertedBodies = new List<ConvertedBody>();
        private List<Map.Mappings> jointMappings = new List<Map.Mappings>();
        public List<ConvertedBody> ConvertedBodies { get; set; }
        public List<Map.Mappings> JointMappings { get { return jointMappings; } }
        
    }
}
