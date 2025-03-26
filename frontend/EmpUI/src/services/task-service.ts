import { User } from "@models/user-model";
import baseService from "@services/core/base-service";
import API_END_POINTS from "@utils/constants/api-end-points";

export const taskService = baseService.injectEndpoints({
  endpoints: (builder) => ({
    // userProfile: builder.query<User, string | void>({
    //   query: (employeeId) => ({
    //     // url: employeeId
    //     //   ? `${API_END_POINTS.employee}?Id=${employeeId}`
    //     //   : API_END_POINTS.employee,
    //     url: `${API_END_POINTS.employee}?Id=${employeeId}`,
    //     method: "GET",
    //   }),
    //   providesTags: ["user"],
    // }),
    tasks: builder.query<User[], string>({
      query: () => ({
        url: API_END_POINTS.tasks,
        method: "GET",
      }),
      providesTags: ["tasks"],
    }),
    task: builder.query<User, string>({
      query: (taskId) => ({
        url: API_END_POINTS.employee + `?Id=${taskId}`,
        method: "GET",
      }),
      providesTags: ["task"],
    }),
    // userSaved: builder.mutation<User, User>({
    //   query: (user) => {
    //     const requestUrl = user?.employeeId
    //       ? API_END_POINTS.employee + `/${user.employeeId}`
    //       : API_END_POINTS.employee;
    //     const requestMethod = user?.employeeId ? "PUT" : "POST";

    //     return {
    //       url: requestUrl,
    //       method: requestMethod,
    //       body: user,
    //     };
    //   },
    //   invalidatesTags: ["users", "user"],
    // }),
  }),
});

export const { useLazyTasksQuery, useTaskQuery } = taskService;
