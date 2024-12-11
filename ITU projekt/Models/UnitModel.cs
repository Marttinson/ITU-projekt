using ITU_projekt.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ITU_projekt.Models
{
    public class UnitModel
    {
        // Basic properties
        public string Name { get; set; }
        public int ID { get; set; }
        public string Description { get; set; }

        // Statistics: Array of error rates (e.g., float[])
        public List<float> ErrorRates { get; set; } = new List<float>();

        // User-generated questions
        public List<Question> UserQuestions { get; set; } = new List<Question>();
    }
}
