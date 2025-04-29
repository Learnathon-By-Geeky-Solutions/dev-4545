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
        url: API_END_POINTS.task + `/${taskId}`,
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
        const requestUrl = task?.isEditMode
          ? API_END_POINTS.task + `/${task.taskId}`
          : API_END_POINTS.tasks;
        const requestMethod = task?.isEditMode ? "PUT" : "POST";

        console.log("from task service edit or create", task);

        return {
          url: requestUrl,
          method: requestMethod,
          body: task.task,
        };
      },
      invalidatesTags: ["tasks", "task"],
    }),
    deleteTask: builder.mutation<void, number>({
      query: (taskId) => ({
        url: API_END_POINTS.tasks + `/${taskId}`,
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
