import React, { useEffect } from "react";
import AppRoutes from "./routes";
import { useDispatch } from "react-redux";
import { setCredentials } from "@reducers/auth-slice";

const App = () => {
  const dispatch = useDispatch();

  // Initialize auth tokens from localStorage on app startup
  useEffect(() => {
    try {
      // Check for individual tokens first
      const jwToken =
        localStorage.getItem("jwToken") || localStorage.getItem("accessToken");
      const refreshToken = localStorage.getItem("refreshToken");

      if (jwToken && refreshToken) {
        dispatch(setCredentials({ jwToken, refreshToken }));
        console.log("Auth tokens initialized from localStorage");
        return;
      }

      // Fall back to auth object
      const auth = localStorage.getItem("auth");
      if (auth) {
        const authData = JSON.parse(auth);
        const token = authData.jwToken || authData.accessToken;
        if (token && authData.refreshToken) {
          dispatch(
            setCredentials({
              jwToken: token,
              refreshToken: authData.refreshToken,
            })
          );

          // Also set individual tokens for consistency
          localStorage.setItem("jwToken", token);
          localStorage.setItem("refreshToken", authData.refreshToken);

          console.log("Auth tokens initialized from auth object");
        }
      }
    } catch (error) {
      console.error("Error initializing auth tokens:", error);
    }
  }, [dispatch]);

  return (
    <React.Fragment>
      <AppRoutes />
    </React.Fragment>
  );
};

export default App;
