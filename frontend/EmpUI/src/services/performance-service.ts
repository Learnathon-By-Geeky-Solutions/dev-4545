import { Performance } from "@models/performance-model";
import baseService from "@services/core/base-service";
import API_END_POINTS from "@utils/constants/api-end-points";

export const performanceService = baseService.injectEndpoints({
  endpoints: (builder) => ({
    performances: builder.query<Performance[], string>({
      query: () => ({
        url: API_END_POINTS.performances,
        method: "GET",
      }),
      providesTags: ["performances"],
    }),
    performance: builder.query<Performance, string>({
      query: (employeeId) => ({
        url: API_END_POINTS.performance + `?EmployeeId=${employeeId}`,
        method: "GET",
      }),
      providesTags: ["performance"],
    }),
    performanceSaved: builder.mutation<Performance, Performance>({
      query: (performance) => {
        const requestUrl = performance?.isEditMode
          ? API_END_POINTS.performances + `?Id=${performance.performance.id}`
          : API_END_POINTS.performances;
        const requestMethod = performance?.isEditMode ? "PUT" : "POST";

        console.log(
          "performance request method ",
          performance,
          requestUrl,
          requestMethod
        );

        return {
          url: requestUrl,
          method: requestMethod,
          body: performance.performance,
        };
      },
      invalidatesTags: ["performances", "performance"],
    }),
    deletePerformance: builder.mutation<void, number>({
      query: (performanceId) => ({
        url: `${API_END_POINTS.performance}?Id=${performanceId}`,
        method: "DELETE",
      }),
      invalidatesTags: ["performances"], // This will auto-refresh the performances list
    }),
  }),
});

export const {
  useLazyPerformancesQuery,
  usePerformanceSavedMutation,
  usePerformanceQuery,
  useDeletePerformanceMutation,
} = performanceService;
