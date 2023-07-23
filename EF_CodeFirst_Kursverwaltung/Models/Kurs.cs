using System;
using System.ComponentModel.DataAnnotations;

namespace EF_CodeFirst_Kursverwaltung.Models
{
    public class Kurs
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime Starttermin { get; set; }

        public int Dauer { get; set; }

        public static bool Parse(string CSVDaten, out Kurs Kurs)
        {
            Kurs = CsvParse(CSVDaten);
            return Kurs != null;
        }

        public static Kurs? CsvParse(string csvZeile)
        {
            try
            {
                string[] felder = csvZeile.Split(';');

                return new Kurs
                {
                    Name = felder[1],
                    Starttermin = DateTime.Parse(felder[2]),
                    Dauer = int.Parse(felder[3])
                };
            }
            catch
            {
                return null;
            }
        }

        
    }
}