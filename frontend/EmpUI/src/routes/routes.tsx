import Dashboard from "@pages/dashboard";
import Settings from "@pages/settings";
import Users from "@pages/users";
import UserCreate from "@pages/users/create";
import UserEdit from "@pages/users/edit";
import UserDetails from "@pages/users/details";
import {
  DashboardBreadcrumb,
  DynamicUserBreadcrumb,
  UserDetailsBreadcrumb,
} from "@/routes/route-utils";
import Tasks from "@pages/tasks";
import Projects from "@pages/projects";
import Features from "@pages/features";

const routes = [
  {
    path: "",
    breadcrumb: DashboardBreadcrumb,
    component: Dashboard,
    exact: true,
    children: [],
  },
  {
    path: "users",
    breadcrumb: "Users",
    component: "",
    exact: true,
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
  },
  {
    path: "projects",
    breadcrumb: "Projects",
    component: Projects,
    exact: true,
    children: [],
  },
  {
    path: "features",
    breadcrumb: "Features",
    component: Features,
    exact: true,
    children: [],
  },
  {
    path: "settings",
    breadcrumb: "Settings",
    component: Settings,
    exact: true,
    children: [],
  },
];

export default routes;
