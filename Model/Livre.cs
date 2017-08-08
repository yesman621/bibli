using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Livre
    {
        public Livre()
        {
            Exemplaire = new List<Exemplaire>();
        }

        [Key]
        [StringLength(5)]
        public string idliv { get; set; }

        [Required]
        [StringLength(80)]
        public string titre { get; set; }


        public int anneeCrea { get; set; }

        [Required]
        [StringLength(100)]
        public string descr { get; set; }

        [Required]
        [StringLength(5)]
        public string idAut { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [ForeignKey("idAut")]
        public Auteur Auteur { get; set; }

        public IList<Exemplaire> Exemplaire { get; set; }
    }
}
