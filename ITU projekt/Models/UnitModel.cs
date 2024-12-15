/** UnitModel
 * M
 * Vojtěch Hrabovský (xhrabo18)
 * 
 * Uchovává všechno nutné info o lekcích.
 */


using System.Collections.Generic;


namespace ITU_projekt.Models
{
    /// <summary>
    /// Class for handling Units
    /// </summary>
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
