import { User } from "@models/user-model";
import baseService from "@services/core/base-service";
import API_END_POINTS from "@utils/constants/api-end-points";

export const taskService = baseService.injectEndpoints({
  endpoints: (builder) => ({
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
    empTasks: builder.query<User, string>({
      query: (empId) => ({
        url: API_END_POINTS.empTasks + `/${empId}`,
        method: "GET",
      }),
      providesTags: ["employee-tasks"],
    }),
    taskSaved: builder.mutation<User, User>({
      query: (task) => {
        const requestUrl = task?.taskId
          ? API_END_POINTS.employee + `/${user.taskId}`
          : API_END_POINTS.employee;
        const requestMethod = task?.taskId ? "PUT" : "POST";

        console.log("task request method ", requestUrl, requestMethod);
        return {
          url: requestUrl,
          method: requestMethod,
          body: user,
        };
      },
      invalidatesTags: ["tasks", "task"],
    }),
    deleteTask: builder.mutation<void, number>({
      query: (taskId) => ({
        url: `${API_END_POINTS.tasks}?Id=${taskId}`,
        method: "DELETE",
      }),
      invalidatesTags: ["tasks"], // This will auto-refresh the tasks list
    }),
  }),
});

export const {
  useLazyTasksQuery,
  useEmpTasksQuery,
  useTaskSavedMutation,
  useTaskQuery,
  useDeleteTaskMutation,
} = taskService;
