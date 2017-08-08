using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Exemplaire
    {
        public Exemplaire()
        {
            Emprunt = new List<Emprunt>();
        }

        [Key]
        [StringLength(5)]
        public string idEx { get; set; }

        [Required]
        [StringLength(10)]
        public string reference { get; set; }

       
        public int anneEdition { get; set; }

        [Required]
        [StringLength(5)]
        public string idliv { get; set; }

        [Required]
        [StringLength(5)]
        public string idME { get; set; }

        public IList<Emprunt> Emprunt { get; set; }

         [ForeignKey("idME")]
        public MaisonEdition MaisonEdition { get; set; }

         [ForeignKey("idliv")]
        public Livre Livre { get; set; }
    }
}
