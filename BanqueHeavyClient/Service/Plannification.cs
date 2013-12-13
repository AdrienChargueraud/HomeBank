using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BanqueLogicLayer.Modele;
using BanqueHeavyClient.Global;

namespace BanqueHeavyClient.Service
{
    public class Plannification
    {
        public Plannification() { }

        public void UpdateProgrammation()
        {
            var context = SessionUtilisateur.Instance.banqueContexte;
            var transaction = context.Database.BeginTransaction();
            try
            {
                var listeOperation = context.OperationPlanning.ToList();

                foreach (var _uneInstance in listeOperation)
                {
                    DateTime lastProg = Convert.ToDateTime(_uneInstance.opeplan_dateOperation);
                    lastProg = lastProg.AddDays(Convert.ToDouble(_uneInstance.opeplan_frequence));
                    if (lastProg <= DateTime.Now)
                    {
                        Operation opTemp = new Operation
                        {
                            operation_date = lastProg,
                            operation_montant = Convert.ToDouble(_uneInstance.opeplan_montant),
                            compte_compte_id = Convert.ToInt32(_uneInstance.compte_compte_id),
                            categorie_categorie_id = 1
                        };

                        SessionUtilisateur.Instance.banqueContexte.Operation.Add(opTemp);
                        context.SaveChanges();
                        transaction.Commit();

                        this.ActualiserLaProgrammation(Convert.ToInt32(_uneInstance.opeplan_id), lastProg);
                    }
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                new Erreur("Problème lors du traitement des opérations pannifiées", "Opérations Planifiées", Enums.TypeErreurMessage.ERREUR).DisplayErreur();
            }
            finally
            {
                transaction.Dispose();
            }
        }

        private void ActualiserLaProgrammation(int id, DateTime newProg)
        {
            var context = SessionUtilisateur.Instance.banqueContexte;
            var transaction = context.Database.BeginTransaction();

            try
            {
                var opUpdate = context.OperationPlanning.Where(m => m.opeplan_id == id).SingleOrDefault();
                opUpdate.opeplan_dateOperation = newProg;
                context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                new Erreur("Problème lors du traitement des opérations pannifiées", "Opérations Planifiées", Enums.TypeErreurMessage.ERREUR).DisplayErreur();
            }
            finally
            {
                transaction.Dispose();
            }
        }
    }
}
