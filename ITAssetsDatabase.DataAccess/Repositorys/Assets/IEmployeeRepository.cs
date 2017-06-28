using ITAssetsDatabase.BusinessDomain;
using System;


namespace ITAssetsDatabase.DataAccess.Repositorys.Assets
{
    public interface IEmployeeRepository: IDisposable
    {
        int EmployeeExistsLookup(string AssigneeSID);
        int AddEmployee(Employee Employee);
        int DoesEmployeeSIDExist(string AssigneeSID);
    }
}
