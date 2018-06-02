using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeReval.Domain
{
    public class Exercise
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<ConvertedBody> ConvertedBodies { get; set; }

        public int Amount { get; set; }

    }
}
