import { User } from "@models/user-model";
import baseService from "@services/core/base-service";
import API_END_POINTS from "@utils/constants/api-end-points";
import Salaries from "../pages/salaries";

export const salaryService = baseService.injectEndpoints({
  endpoints: (builder) => ({
    salaries: builder.query<User[], string>({
      query: () => ({
        url: API_END_POINTS.salaries,
        method: "GET",
      }),
      providesTags: ["salaries"],
    }),
    salaryByEmployeeId: builder.query<User, string>({
      query: (employeeId) => ({
        url: `${API_END_POINTS.salaries}/${employeeId}`,
        method: "GET",
      }),
      // tag this single entry for granular cache updates
      providesTags: (result, error, employeeId) => [
        { type: "Salaries" as const, id: employeeId },
      ],
    }),
    salarySaved: builder.mutation<User, User>({
      query: (salary) => {
        const requestUrl = salaries?.salaryId
          ? API_END_POINTS.employee + `/${user.salaryId}`
          : API_END_POINTS.employee;
        const requestMethod = salary?.salaryId ? "PUT" : "POST";

        console.log("salary request method ", requestUrl, requestMethod);

        return {
          url: requestUrl,
          method: requestMethod,
          body: user,
        };
      },
      invalidatesTags: ["salaries", "salary"],
    }),
    deleteSalary: builder.mutation<void, number>({
      query: (employeeId) => ({
        url: `${API_END_POINTS.salaries}/${employeeId}`,
        method: "DELETE",
      }),
      invalidatesTags: ["salaries"], // This will auto-refresh the tasks list
    }),
  }),
});

export const {
  useLazySalariesQuery,
  useSalaryByEmployeeIdQuery,
  useSalarySavedMutation,
  useSalaryQuery,
  useDeleteSalaryMutation,
} = salaryService;
