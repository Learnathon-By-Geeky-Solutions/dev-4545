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
    "salaries",
    "Salaries",
    "salary",
    "employee-tasks",
    "employee-features",
    "employee-projects",
    "employee-leave",
    "leave",
    "task",
    "project",
    "feature"
  ],
  endpoints: () => ({}),
});

export default baseService;
