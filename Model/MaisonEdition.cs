namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public  class MaisonEdition
    {
        public MaisonEdition()
        {
            Exemplaire = new List<Exemplaire>();
        }

  

        [Key]
        [StringLength(5)]
        public string idME { get; set; }

        [Required]
        [StringLength(30)]
        public string nom { get; set; }

        [Required]
        [StringLength(25)]
        public string ville { get; set; }

        [Required]
        [StringLength(30)]
        public string pays { get; set; }

        public  IList<Exemplaire> Exemplaire { get; set; }
    }
}
