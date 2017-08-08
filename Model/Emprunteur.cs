namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public  class Emprunteur
    {
        public Emprunteur()
        {
            Emprunt = new List<Emprunt>();
        }

        [Key]
        [StringLength(5)]
        public string numCarte { get; set; }

        [Required]
        [StringLength(20)]
        public string nom { get; set; }

        [Required]
        [StringLength(25)]
        public string prenom { get; set; }

        [Required]
        [StringLength(200)]
        public string adresse { get; set; }

        [Required]
        [StringLength(30)]
        public string email { get; set; }

        public  IList<Emprunt> Emprunt { get; set; }
    }
}
