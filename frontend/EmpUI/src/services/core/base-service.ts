import { createApi } from "@reduxjs/toolkit/query/react";
import baseQueryWithReAuth from "./custom-fetch-service";

const baseService = createApi({
  reducerPath: "api",
  baseQuery: baseQueryWithReAuth,
  keepUnusedDataFor: 120,
  tagTypes: [
    "auth",
    "user",
    "users",
    "tasks",
    "projects",
    "features",
    "leaves",
    "performances",
    "performance",
  ],
  endpoints: () => ({}),
});

export default baseService;
