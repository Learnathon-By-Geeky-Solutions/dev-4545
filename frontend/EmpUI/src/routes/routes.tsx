import Dashboard from "@pages/dashboard";
import Settings from "@pages/settings";
import Users from "@pages/users";
import UserCreate from "@pages/users/create";
import UserEdit from "@pages/users/edit";
import UserDetails from "@pages/users/details";

import Tasks from "@pages/tasks";
import Projects from "@pages/projects";
import Features from "@pages/features";
import Leaves from "@pages/Leaves";
import Performances from "@pages/performances";
import Salary from "@pages/salary";
import EmployeeDetails from "@pages/users/emp-details";

const routes = [
  {
    path: "",
    component: Dashboard,
    exact: true,
    children: [],
    roles: ["Admin", "SE"],
  },
  {
    path: "users",
    breadcrumb: "Users",
    component: "",
    exact: true,
    roles: ["Admin"],
    children: [
      {
        path: "",
        breadcrumb: "Users",
        component: Users,
        exact: true,
      },
      {
        path: "create",
        breadcrumb: "Create User",
        component: UserCreate,
        exact: true,
      },
      {
        path: ":id",
        // breadcrumb: DynamicUserBreadcrumb,
        component: UserEdit,
        exact: true,
      },
      {
        path: "details/:id",
        breadcrumb: "UserDetails",
        component: UserDetails,
        exact: true,
      },
    ],
  },
  {
    path: "tasks",
    breadcrumb: "Tasks",
    component: Tasks,
    exact: true,
    children: [],
    roles: ["Admin"],
  },
  {
    path: "projects",
    breadcrumb: "Projects",
    component: Projects,
    exact: true,
    children: [],
    roles: ["Admin"],
  },
  {
    path: "features",
    breadcrumb: "Features",
    component: Features,
    exact: true,
    children: [],
    roles: ["Admin"],
  },
  {
    path: "leaves",
    breadcrumb: "Leaves",
    component: Leaves,
    exact: true,
    children: [],
    roles: ["Admin"],
  },
  {
    path: "performances",
    breadcrumb: "Performances",
    component: Performances,
    exact: true,
    children: [],
    roles: ["Admin"],
  },
  {
    path: "salaries",
    breadcrumb: "Salaries",
    component: Salary,
    exact: true,
    children: [],
    roles: ["Admin"],
  },
  {
    path: "settings",
    breadcrumb: "Settings",
    component: Settings,
    exact: true,
    children: [],
    roles: ["Admin", "SE"], // Both roles can access
  },
  {
    path: "details",
    breadcrumb: "UserDetails",
    component: EmployeeDetails,
    exact: true,
    children: [],
    roles: ["SE"], // Both roles can access
  },
];

export default routes;
