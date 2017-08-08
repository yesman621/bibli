using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Auteur
    {
        public Auteur()
        {
            Livre = new List<Livre>();
        }

        [Key]
        [StringLength(5)]
        public string idAut { get; set; }

        [Required]
        [StringLength(60)]
        public string nomAuteur { get; set; }

        [Required]
        [StringLength(60)]
        public string prenomAut { get; set; }

        public IList<Livre> Livre { get; set; }
    }
}
