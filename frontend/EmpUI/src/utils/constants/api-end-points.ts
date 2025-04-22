const API_END_POINTS = {
  login: "/login",
  refreshToken: "/auth/refresh-token",
  user: "/auth/profile",
  users: "/users",
  employee: "/Employees/employee",
  employees: "/Employees",
  tasks: "/Tasks",
  task: "/Tasks/EmployeeId",
  projects: "/Projects",
  project: "/Projects/EmployeeId",
  features: "/Features",
  feature: "/Features/EmployeeId",
  leaves: "/Leave",
  performances: "/Performances",
  performance: "/Performances/EmployeeId",
  salaries: "/Salary",
  salary: "/Salary/GetSalaryByEmployeeId",
  empTasks: "/Tasks/EmployeeId",
  empFeatures:"/Features/EmployeeId"
};

export default API_END_POINTS;
