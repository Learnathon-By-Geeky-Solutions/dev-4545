﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Core.Entities;

namespace Employee.Core.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeEntity>> GetEmployees();
        Task<EmployeeEntity> GetEmployeeById(Guid id);
        Task<EmployeeEntity> AddEmployee(EmployeeEntity employee);
        Task<EmployeeEntity> UpdateEmployee(Guid id, EmployeeEntity updatedentity);
        Task<bool> DeleteEmployee(Guid id);
    }
}
