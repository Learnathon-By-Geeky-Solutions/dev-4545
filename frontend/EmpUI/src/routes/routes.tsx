import Dashboard from "@pages/dashboard";
import Settings from "@pages/settings";
import Users from "@pages/users";
import UserCreate from "@pages/users/create";
import LeaveCreate from "@pages/leave/create";
import LeavesCreate from "@pages/leaves/create";
import UserEdit from "@pages/users/edit";
import UserDetails from "@pages/users/details";

import Tasks from "@pages/tasks";
import Projects from "@pages/projects";
import Features from "@pages/features";
import Leaves from "@pages/Leaves";
import Performances from "@pages/performances";
import Salary from "@pages/salary";
import EmployeeDetails from "@pages/users/emp-details";
import leave from "@pages/leave";
import TaskCreate from "@pages/tasks/create";
import ProjectCreate from "@pages/projects/create";
import FeatureCreate from "@pages/features/create";
import TaskEdit from "@pages/tasks/edit";
import FeatureEdit from "@pages/features/edit"
import ProjectEdit from "@pages/projects/edit"
import PerformanceCreate from "@pages/performances/create";

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
    roles: ["Admin"],
  },
  {
    path: "tasks/create",
    breadcrumb: "Tasks",
    component: TaskCreate,
    exact: true,
    children: [],
    roles: ["Admin"],
  },

  {
    path: "tasks/:id",
    breadcrumb: "Tasks",
    component: TaskEdit,
    exact: true,
    children: [],
    roles: ["Admin"],
  },
  {
    path: "performances/create",
    breadcrumb: "Performance",
    component: PerformanceCreate,
    exact: true,
    children: [],
    roles: ["Admin"],
  },

  {
    path: "projects/create",
    breadcrumb: "Projects",
    component: ProjectCreate,
    exact: true,
    children: [],
    roles: ["Admin"],
  },
  {
    path: "projects/:id",
    breadcrumb: "Projects",
    component: ProjectEdit,
    exact: true,
    children: [],
    roles: ["Admin"],
  },
  {
    path: "features/create",
    breadcrumb: "Features",
    component: FeatureCreate,
    exact: true,
    children: [],
    roles: ["Admin"],
  },
  {
    path: "features/:id",
    breadcrumb: "Features",
    component: FeatureEdit,
    exact: true,
    children: [],
    roles: ["Admin"],
  },
  {
    path: "leaves/create",
    breadcrumb: "Leaves",
    component: LeavesCreate,
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
  {
    path: "leave",
    breadcrumb: "Userleave",
    component: "",
    exact: true,
    roles: ["SE"],
    children: [
      {
        path: "",
        breadcrumb: "",
        component: leave,
        exact: true,
      },
      {
        path: "create",
        breadcrumb: "Create Leave",
        component: LeaveCreate,
        exact: true,
      },
    ],
  },
];

export default routes;
