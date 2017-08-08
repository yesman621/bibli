using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Model;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Text;

namespace BibliApi.Controllers
{
    [RoutePrefix("api/Bibli")]
    public class BibliController : ApiController
    {
        private BibliContext db = new BibliContext();

        [HttpPut]
        [Route("modiflivre")]
        public IHttpActionResult ModifLivre([FromBody]JObject dd)          
        {
            
            string idd = dd.GetValue("id").ToString();
            string titre0 = dd.GetValue("titre").ToString();
            string descr0 = dd.GetValue("descr").ToString();
            //exemple: string date = "2000";
            int dt = Convert.ToInt32(dd.GetValue("date").ToString());
            string idauteur = dd.GetValue("idaut").ToString();
            string rv = dd.GetValue("rowVersion").ToString();

            int id=Convert.ToInt32(idd);
            byte[] rowVersion=Encoding.ASCII.GetBytes(rv);

            
            if (id <=0 ||id>=100000 ||MotValide(titre0)==false||MotValide(descr0)==false||
                MotValide(idauteur) == false || dt > DateTime.Now.Year)
            {
                return BadRequest();
            }

            Livre livreedit = db.Livres.Find(idd);
            if (livreedit == null)
            {
                
                Debug.WriteLine("  Unable to save changes. The department was deleted by another user.");
                return BadRequest();
            }

            livreedit.anneeCrea = dt;
            livreedit.idAut = idauteur;
            livreedit.titre = titre0;
            livreedit.descr = descr0;

                try
                {
                    db.Entry(livreedit).OriginalValues["RowVersion"] = rowVersion;
                     db.SaveChanges();

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Livre)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                       // ModelState.AddModelError(string.Empty,
                          //  "Unable to save changes. The department was deleted by another user.");
                        Debug.WriteLine("Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Livre)databaseEntry.ToObject();

                        if (databaseValues.titre != clientValues.titre)
                            Debug.WriteLine(" titre, Current value: "
                                + databaseValues.titre);
                        if (databaseValues.Auteur != clientValues.Auteur)
                            Debug.WriteLine("Auteur, Current value: "
                                +  databaseValues.idAut);
                        if (databaseValues.descr != clientValues.descr)
                            Debug.WriteLine("description, Current value: "
                                +  databaseValues.descr);
                        Debug.WriteLine("The record attempted to edit "
                            + "was modified by another user after  got the original value. The "
                            + "edit operation was canceled and the current values in the database "
                            + "have been displayed");
                        livreedit.RowVersion = databaseValues.RowVersion;
                    }
                }


             
             
            return Ok();
        }


        [Route("trouvelivre")]
        public IQueryable<Emprunt> GetLivre()
        {
           // DbSet<Livre> d =db.Livres;
            DbSet<Emprunt> d = db.Emprunts;
            if(d==null)
                Debug.WriteLine("  vide");
            else
                foreach (Emprunt a in d)
                Debug.WriteLine(a.Emprunteur + "/"+a.idEx);

            return d;
        }

        // GET: api/Livres/5

        [ResponseType(typeof(Livre))]
        [Route("livreparid")]
        public IHttpActionResult GetLivre(string id)
        {
            Debug.WriteLine("fonction getlivre id");
            Livre livre = db.Livres.Find(id);
            if (livre == null)
            {
                return NotFound();
            }
            Debug.WriteLine(livre.titre + "/");
            return Ok(livre);
        }

        //entree exemple: &titre=Pringles&descr=...&date=1974&idaut=1
        [HttpPost]
        [Route("nouveaulivre")]
        public IHttpActionResult NouveauLivre([FromBody]JObject dd)
        {

           
			//Debug.WriteLine("/n aaa");
            int id=db.Livres.Count()+1;

            string titre0 = dd.GetValue("titre").ToString();
            string descr0 = dd.GetValue("descr").ToString();
			//exemple: string date = "01/08/2000";
            int dt = Convert.ToInt32(dd.GetValue("date").ToString());
			string idauteur=dd.GetValue("idaut").ToString();


            if (MotValide(titre0) == false || MotValide(descr0) == false
                || MotValide(idauteur) == false||dt>DateTime.Now.Year)
                return BadRequest();

            Livre l = new Livre()
            {
                idliv = ""+id,
                titre = titre0,
                anneeCrea = dt,
                descr = descr0,
                idAut = idauteur,
               
            };

            db.Livres.Add(l);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                if (LivresExists(l.idliv))
                {
                    return Conflict();
                }
                else
                {
                    return InternalServerError(ex);
                }
            }

            return Ok();
        }
		
		private bool LivresExists(string id)
        {
            return db.Livres.Count(e => e.idliv == id) > 0;
        }

        // entree exemple: &nom=Litchie&prenom=Lionel
        [HttpPost]
        [Route("nouvelauteur")]
        public IHttpActionResult NouvelAuteur([FromBody]JObject dd)
        {

            Debug.WriteLine("fct nouvel auteur");
            int id=db.Auteurs.Count()+1;

            string nom = dd.GetValue("nom").ToString();
            string prenom = dd.GetValue("prenom").ToString();

            if (MotValide(nom) == false || MotValide(prenom) == false)
                return BadRequest();

            Auteur a = new Auteur()
            {
                idAut = ""+id,
                nomAuteur = nom,
                prenomAut = prenom
            };

            db.Auteurs.Add(a);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                if (AuteurExists(a.idAut))
                {
                    return Conflict();
                }
                else
                {
                    return InternalServerError(ex);
                }
            }

            return Ok();
        }

        private bool AuteurExists(string id)
        {
            return db.Auteurs.Count(e => e.idAut == id) > 0;
        }

        // entree exemple: &numCarte=1&idExemp=1
        [HttpPost]
        [Route("nouvelemprunt")]
        public IHttpActionResult NouvelEmprunt([FromBody]JObject dd)
        {
			

            string numCarte = dd.GetValue("numCarte").ToString();
            string idLivre = dd.GetValue("idExemp").ToString();

            if (MotValide(numCarte) == false || MotValide(idLivre) == false)
                return BadRequest();
			
			//si l'exemplaire est deja preté : test
            var emprunts = db.Emprunts.Where(a => a.idEx == idLivre).ToList();
            Debug.WriteLine("new emp. count " + emprunts.Count);
            
            if (emprunts.Count != 0)
            {
                //deja emprunté
                return BadRequest();
            }
			
			int id=db.Emprunts.Count()+1;
			
			Emprunt e = new Emprunt()
            {
                idEmprunt = ""+id,
                numCarte = numCarte,
                idEx = idLivre,
				dateEmprnt = DateTime.Now.Date.ToLocalTime(),
                dateMax=DateTime.Now.Date.ToLocalTime().AddDays(15)
            };

            db.Emprunts.Add(e);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
				
                return InternalServerError(ex);
                
			}

            return Ok();
		
		}

        //en entree: p e &annee=1993&idliv=1&ref=h564415&idMaison=1
		[HttpPost]
        [Route("nouvelexemplaire")]
		public IHttpActionResult NouvelExemplaire([FromBody]JObject dd)
        {


			int anne0=0;

			try {
				anne0 = System.Convert.ToInt32(dd.GetValue("annee").ToString());

			} 
			catch (System.FormatException) {
				System.Console.WriteLine(
					"The string is not formatted as an int.");
			}
			catch (System.ArgumentNullException) {
				System.Console.WriteLine(
					"The string is null.");
			}

			string ref0 = dd.GetValue("ref").ToString();

			string idliv0= dd.GetValue("idliv").ToString();
			string idMaison= dd.GetValue("idMaison").ToString();
           
            if (MotValide(ref0) == false || MotValide(idliv0) == false
                || MotValide(idMaison) == false)
                return BadRequest();
			
			int id=db.Exemplaires.Count()+1;
			
			
			
			Exemplaire Ex = new Exemplaire()
            {
                idEx=""+id,
				anneEdition=anne0,
				reference=ref0,
				idliv=idliv0,
				idME=idMaison
            };
		
		
            db.Exemplaires.Add(Ex);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                if (ExemplaireExists(Ex.idEx))
                {
                    return Conflict();
                }
                else
                {
                    return InternalServerError(ex);
                }
            }

            return Ok();
        }

        private bool ExemplaireExists(string id)
        {
            return db.Exemplaires.Count(e => e.idEx == id) > 0;
        }

        //en entree : pe &nom=Poche&ville=Grenoble&pays=France
        [HttpPost]
        [Route("nouvellemaisonedition")]
        public IHttpActionResult NouvelleMaisonEdition([FromBody]JObject dd)
        {
            
			int id=db.MaisonsEdition.Count()+1;
			
			string nom0 = dd.GetValue("nom").ToString();
			string ville0=dd.GetValue("ville").ToString();
			string pays0= dd.GetValue("pays").ToString();

            if (MotValide(nom0) == false || MotValide(ville0) == false
                || MotValide(pays0) == false)
                return BadRequest();
			
			MaisonEdition maisonEdition = new MaisonEdition()
            {
                idME = ""+id,
                nom = nom0,
                ville = ville0,
				pays=pays0
            };


            db.MaisonsEdition.Add(maisonEdition);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                if (MaisonEditionExists(maisonEdition.idME))
                {
                    return Conflict();
                }
                else
                {
                    return InternalServerError(ex);
                }
            }
            return Ok();
        }

        private bool MaisonEditionExists(string id)
        {
            return db.MaisonsEdition.Count(e => e.idME == id) > 0;
        }
		
		//enlever emprunt d'un lecteur
        //entrée exemple: &numCarte=1&idEx=1
        [HttpDelete]
        [Route("retouremprunt")]
        public IHttpActionResult RetourEmprunt([FromBody]JObject dd)
        {
			
			string numCarte = dd.GetValue("numCarte").ToString();
			//exemplaire et livre est confondu
            string idExemp = dd.GetValue("idEx").ToString();

            if (MotValide(numCarte) == false || MotValide(idExemp) == false)
                return BadRequest();
			
			/*
            Emprunt emprunt =  (from ep in db.Emprunts
                                
                                            join e in db.Emprunteurs on ep.numCarte equals e.numCarte
                                            join ex in db.Exemplaires on ep.idEx equals ex.idEx
                                            where e.numCarte == idEmprunteur
                                            where ex.idEx== idExemplaire
                                            select new Emprunt
                      {
                          
                      }).First();
            */

            Emprunt emprunt = db.Emprunts.Where(a => a.numCarte == numCarte).Where(a => a.idEx == idExemp).First();      
            if (emprunt == null)
            {
                return NotFound();
            }

            db.Emprunts.Remove(emprunt);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {

                return InternalServerError(ex);

            }

            return Ok();
        }
		
		private bool MotValide(string mot)
		{
			bool b=false;

            if (String.IsNullOrEmpty(mot))
                b = false;
            else
                b = true;
			
			return b;
		
		}

    }
}
