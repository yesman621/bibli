using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BibliContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Livre> Livres { get; set; }
        public DbSet<Auteur> Auteurs { get; set; }
        public DbSet<Emprunt> Emprunts { get; set; }
        public DbSet<MaisonEdition> MaisonsEdition { get; set; }
        public DbSet<Emprunteur> Emprunteurs { get; set; }
        public DbSet<Exemplaire> Exemplaires { get; set; }

        public BibliContext()
            : base(@"Data Source=(localdb)\MSSQLLocalDb;Initial Catalog=RestApiDemo;")
        {

        }
    }
}
