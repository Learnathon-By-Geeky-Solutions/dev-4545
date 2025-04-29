import { Leave } from "@models/leave-model";
import baseService from "@services/core/base-service";
import API_END_POINTS from "@utils/constants/api-end-points";

export const leaveService = baseService.injectEndpoints({
  endpoints: (builder) => ({
    leaves: builder.query<Leave[], string>({
      query: () => ({
        url: API_END_POINTS.leaves,
        method: "GET",
      }),
      providesTags: ["leaves"],
    }),
    leaveSaved: builder.mutation<Leave, Leave>({
      query: (leave) => {
        const requestUrl = leave?.employeeId
          ? API_END_POINTS.leaves + `?EmployeeId=${leave.employeeId}`
          : API_END_POINTS.leaves;
        const requestMethod = leave?.employeeId ? "PUT" : "POST";

        console.log("leave request method ", requestUrl, requestMethod);

        return {
          url: requestUrl,
          method: requestMethod,
          body: leave,
        };
      },
      invalidatesTags: ["leaves", "leave"],
    }),
    empLeave: builder.query<User, string>({
      query: (empId) => ({
        url:  API_END_POINTS.leave + `?EmployeeId=${empId}`,
        method: "GET",
      }),
      providesTags: ["employee-leave"],
    }),
    deleteLeave: builder.mutation<void, number>({
      query: (leaveId) => ({
        url: `${API_END_POINTS.leaves}?Id=${leaveId}`,
        method: "DELETE",
      }),
      invalidatesTags: ["leaves"], // This will auto-refresh the leaves list
    }),
  }),
});

export const {
  useLazyLeavesQuery,
  useLeaveSavedMutation,
  useLeaveQuery,
  useEmpLeaveQuery,
  useDeleteLeaveMutation,
} = leaveService;
