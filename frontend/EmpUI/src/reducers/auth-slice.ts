import { createSlice, PayloadAction } from "@reduxjs/toolkit";

import { AuthState } from "@models/auth-model";

// Load initial state from localStorage if available
const loadInitialState = (): AuthState => {
  try {
    // Try to get from individual keys first
    const jwToken = localStorage.getItem("jwToken");
    const refreshToken = localStorage.getItem("refreshToken");

    if (jwToken && refreshToken) {
      return {
        jwToken,
        refreshToken,
      };
    }

    // Fall back to auth object
    const auth = localStorage.getItem("auth");
    if (auth) {
      const parsedAuth = JSON.parse(auth);
      return {
        jwToken: parsedAuth.jwToken || parsedAuth.accessToken || "",
        refreshToken: parsedAuth.refreshToken || "",
      };
    }
  } catch (error) {
    console.error("Error loading auth state from localStorage:", error);
  }

  return {
    jwToken: "",
    refreshToken: "",
  };
};

const initialState = loadInitialState();

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    setCredentials: (state, action: PayloadAction<AuthState>) => {
      const { jwToken, refreshToken } = action.payload;

      state.jwToken = jwToken;
      state.refreshToken = refreshToken;

      // Also update localStorage
      try {
        localStorage.setItem("jwToken", jwToken);
        localStorage.setItem("refreshToken", refreshToken);
      } catch (error) {
        console.error("Error saving auth state to localStorage:", error);
      }
    },
    logOut: (state) => {
      // Clear state
      state.jwToken = "";
      state.refreshToken = "";

      // Clear localStorage
      try {
        localStorage.removeItem("jwToken");
        localStorage.removeItem("accessToken"); // Remove old key as well
        localStorage.removeItem("refreshToken");
        localStorage.removeItem("auth");
        localStorage.removeItem("role");
        localStorage.removeItem("employeeId");
        localStorage.removeItem("userData");
      } catch (error) {
        console.error("Error clearing auth data from localStorage:", error);
      }

      return state;
    },
  },
});

export const { setCredentials, logOut } = authSlice.actions;

export default authSlice.reducer;
