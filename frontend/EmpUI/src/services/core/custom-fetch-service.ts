import {
  BaseQueryFn,
  FetchArgs,
  fetchBaseQuery,
  FetchBaseQueryError,
} from "@reduxjs/toolkit/query/react";
import { Mutex } from "async-mutex";
import { AuthResponse } from "@models/auth-model";
import { setCredentials, logOut } from "@reducers/auth-slice";
import API_END_POINTS from "@utils/constants/api-end-points";
import { RootState } from "@/store";

const url = process.env.API_BASE_URL as string;
const baseUrl = url;
const mutex = new Mutex();

const baseApi = fetchBaseQuery({
  baseUrl: baseUrl,
  prepareHeaders: (headers: Headers, { getState }) => {
    // Try to get token from Redux state first
    const state = getState() as RootState;
    let token = state.auth.jwToken as string;

    // If not in Redux state, try to get from localStorage directly
    if (!token) {
      try {
        token = localStorage.getItem("jwToken") || "";
        // If jwToken not found, try legacy accessToken
        if (!token) {
          token = localStorage.getItem("accessToken") || "";
        }
      } catch (error) {
        console.error("Error reading token from localStorage:", error);
      }
    }

    if (token) {
      headers.set("Authorization", `Bearer ${token}`);
    }

    return headers;
  },
});

const baseQueryWithReAuth: BaseQueryFn<
  string | FetchArgs,
  any,
  FetchBaseQueryError
> = async (args, api, extraOptions) => {
  const isFormData = args && (args as FetchArgs).body instanceof FormData;
  const headers = new Headers();

  if (!isFormData) {
    headers.set("Content-Type", "application/json");
  }

  const requestArgs: FetchArgs =
    typeof args === "string" ? { url: args } : { ...args };
  requestArgs.headers = headers;

  await mutex.waitForUnlock();

  console.log("request args", requestArgs);

  let response = await baseApi(requestArgs, api, extraOptions);

  if (response.error && response.error.status === 403) {
    if (!mutex.isLocked()) {
      const release = await mutex.acquire();

      try {
        const state = api.getState() as RootState;
        // Get refresh token from state or localStorage
        let refreshToken = state.auth.refreshToken;

        if (!refreshToken) {
          try {
            refreshToken = localStorage.getItem("refreshToken") || "";
          } catch (error) {
            console.error(
              "Error reading refresh token from localStorage:",
              error
            );
          }
        }

        if (refreshToken) {
          const formData = new FormData();
          formData.append("refreshToken", refreshToken);

          const refreshResponse = await baseApi(
            {
              url: API_END_POINTS.refreshToken,
              method: "POST",
              body: formData,
              ...headers,
            },
            api,
            extraOptions
          );

          if (refreshResponse.error) {
            api.dispatch(logOut());
            localStorage.removeItem("jwToken");
            localStorage.removeItem("refreshToken");
          } else {
            const refreshResponseData = refreshResponse.data as AuthResponse;

            const authData = {
              jwToken: refreshResponseData.jwToken,
              refreshToken: refreshResponseData.refreshToken,
            };

            api.dispatch(setCredentials(authData));

            // Store tokens in both formats
            localStorage.setItem("jwToken", refreshResponseData.jwToken);
            localStorage.setItem(
              "refreshToken",
              refreshResponseData.refreshToken
            );

            headers.set("Authorization", `Bearer ${authData.jwToken}`);
            requestArgs.headers = headers;

            response = await baseApi(requestArgs, api, extraOptions);
          }
        } else {
          api.dispatch(logOut());
          localStorage.removeItem("jwToken");
          localStorage.removeItem("refreshToken");
          localStorage.removeItem("role");
          localStorage.removeItem("employeeId");
        }
      } finally {
        release();
      }
    } else {
      await mutex.waitForUnlock();
      const state = api.getState() as RootState;
      const token = state.auth.jwToken;

      headers.set("Authorization", `Bearer ${token}`);
      requestArgs.headers = headers;

      response = await baseApi(requestArgs, api, extraOptions);
    }
  }

  return response;
};

export default baseQueryWithReAuth;
