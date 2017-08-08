using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
namespace Model.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void Setup()
        {
            Database.SetInitializer(new DbInitializer());
            using (BibliContext context = GetContext())
            {
                context.Database.Initialize(force: true);
            }
        }

        private static BibliContext GetContext()
        {
            return new BibliContext();
        }

        [TestMethod]
        public void CanGetLivres()
        {
            using (var context = GetContext())
            {
                Assert.AreEqual(3, context.Livres.ToList().Count);
                DbSet<Livre> d = context.Livres;

                if (d == null)
                    Debug.WriteLine("  vide");
                else
                    foreach (Livre a in d)
                        Debug.WriteLine(a.titre + ", année:"+a.anneeCrea+", auteur:"+a.idAut);
            }
        }
        /*
        [TestMethod]
        public void CanGetCustomers()
        {
            using (var context = GetContext())
            {
                Assert.AreEqual(2, context.Customers.ToList().Count);
            }
        }
        */
        [TestMethod]
        [ExpectedException(typeof(DbUpdateConcurrencyException))]
        public void DetecteLesEditionsConcurrentes()
        {
            using (BibliContext contexteDeJohn = GetContext())
            {
                using (BibliContext contexteDeSarah = GetContext())
                {
                    var Livre1 = contexteDeJohn.Livres.First();
                    var Livre2 = contexteDeSarah.Livres.First();

                    Livre1.titre = "gfhf";
                    contexteDeJohn.SaveChanges();

                    Livre2.titre = "Hfgh";

                    contexteDeSarah.SaveChanges();


                }
            }
        }
        

    }
}
