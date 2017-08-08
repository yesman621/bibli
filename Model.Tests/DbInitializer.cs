using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tests
{
    class DbInitializer : DropCreateDatabaseAlways<BibliContext>
    {

        protected override void Seed(BibliContext context)
        {
           
            Auteur a = new Auteur()
            {
                idAut="1",
                nomAuteur="JP",
                prenomAut="Zorro"
            };

            Livre l = new Livre()
            {
                idliv = "1",
                titre = "socrate",
                anneeCrea = 1984,
                descr = "...",
                idAut = "1",
                Auteur=a
            };

            Livre l2 = new Livre()
            {
                idliv = "2",
                titre = "pluton",
                anneeCrea = 1986,
                descr = "...",
                idAut = "1",
                Auteur = a
            };

            Emprunteur e = new Emprunteur()
            {
                numCarte = "1",
                nom = "Judor",
                prenom = "Eric",
                adresse = "... 5000 namur",
                email = "ericj@dd.com"
            };

            MaisonEdition m = new MaisonEdition()
            {

                idME = "1",
                nom = "Fayard",
                ville = "Lille",
                pays = "France"
            };

            Exemplaire ex = new Exemplaire()
            {

                idEx = "1",
                reference = "df11567",
                anneEdition = 1992,
                idliv = "1",
                idME = "1"
            };

            Exemplaire ex2 = new Exemplaire()
            {

                idEx = "2",
                reference = "g56457",
                anneEdition = 1982,
                idliv = "2",
                idME = "1"
            };

            context.Auteurs.Add(a);
            context.Livres.Add(l);
            context.Livres.Add(l2);
            context.Emprunteurs.Add(e);
            context.MaisonsEdition.Add(m);
            context.Exemplaires.Add(ex);
            context.Exemplaires.Add(ex2);
            
            context.SaveChanges();
        }
    }
}
