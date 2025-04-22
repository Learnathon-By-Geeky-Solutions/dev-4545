import {
  BrowserRouter as Router,
  Routes,
  Route,
  Outlet,
} from "react-router-dom";
import useAuthentication from "@hooks/use-authentication";
import DefaultLayout from "@layouts/default-layout";
import LoginLayout from "@layouts/login-layout";
import ForgetPassword from "@pages/auth/forget-password";
import Login from "@pages/auth/login";
import ResetPassword from "@pages/auth/reset-password";
import NotFound from "@pages/not-found";
import PrivateRoute from "./private-route";
import PublicRoute from "./public-route";
import routes from "./routes";
import { getAuthorizedRoutes } from "./route-utils";

const AppRoutes = () => {
  const { isAuthenticated } = useAuthentication();
  const authorizedRoutes = getAuthorizedRoutes();
  console.log("authorized routes ", authorizedRoutes);

  return (
    <Router>
      <Routes>
        <Route
          path="/login"
          element={
            <PublicRoute isAuthenticated={isAuthenticated()}>
              <LoginLayout />
            </PublicRoute>
          }
        >
          <Route index element={<Login />} />
        </Route>
        <Route
          path="/forget-password"
          element={
            <PublicRoute isAuthenticated={isAuthenticated()}>
              <LoginLayout />
            </PublicRoute>
          }
        >
          <Route index element={<ForgetPassword />} />
        </Route>
        <Route
          path="/reset-password"
          element={
            <PublicRoute isAuthenticated={isAuthenticated()}>
              <LoginLayout />
            </PublicRoute>
          }
        >
          <Route index element={<ResetPassword />} />
        </Route>

        <Route
          path="/"
          element={
            <PrivateRoute isAuthenticated={isAuthenticated()}>
              <DefaultLayout />
            </PrivateRoute>
          }
        >
          {/* {routes.map(({ component: Component, path, children }) => (
            <Route
              path={`/${path}`}
              element={children.length > 0 ? <Outlet /> : <Component />}
              key={path}
            >
              {children &&
                children.map(
                  ({ component: ChildComponent, path: childPath }) => (
                    <Route
                      path={`/${path}/${childPath}`}
                      element={<ChildComponent />}
                      key={childPath}
                    />
                  )
                )}
            </Route>
          ))} */}
          {/* Change this line to use authorizedRoutes instead of routes */}
          {authorizedRoutes.map(({ component: Component, path, children }) => (
            <Route
              path={`/${path}`}
              element={
                children && children.length > 0 ? <Outlet /> : <Component />
              }
              key={path}
            >
              {children &&
                children.map(
                  ({ component: ChildComponent, path: childPath }) => (
                    <Route
                      path={`/${path}/${childPath}`}
                      element={<ChildComponent />}
                      key={childPath}
                    />
                  )
                )}
            </Route>
          ))}
        </Route>
        <Route path="*" element={<NotFound />} />
      </Routes>
    </Router>
  );
};

export default AppRoutes;
