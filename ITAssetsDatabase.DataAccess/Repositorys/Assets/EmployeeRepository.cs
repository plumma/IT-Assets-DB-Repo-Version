using ITAssetsDatabase.BusinessDomain;
using System;
using System.Linq;


namespace ITAssetsDatabase.DataAccess.Repositorys.Assets
{
     public class EmployeeRepository : IEmployeeRepository, IDisposable
        {
        
        private ITAssetsDatabaseDBContext db = new ITAssetsDatabaseDBContext();

        // Does the Employee Already Exist in the Database

        public int EmployeeExistsLookup(string AssigneeSID)
        {
           return (db.Employees.Where(e => e.SID == AssigneeSID).Select(u => u.Id).FirstOrDefault());
         }
                

        // Add new Employee

        public int AddEmployee (Employee Employee)
        {
            db.Employees.Add(Employee);
            db.SaveChanges();

            // Returns the Employee Id of the newly added Employee record just added
            return Employee.Id;

        }


        // SID Lookup

        public int DoesEmployeeSIDExist(string AssigneeSID)
        { 
            return  (db.Employees.Where(e => e.SID == AssigneeSID).Select(u => u.Id).FirstOrDefault());
        }

        #region Tidy Up


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
