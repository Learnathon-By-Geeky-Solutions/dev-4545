import { User } from "@models/user-model";
import baseService from "@services/core/base-service";
import API_END_POINTS from "@utils/constants/api-end-points";

export const userService = baseService.injectEndpoints({
  endpoints: (builder) => ({
    userProfile: builder.query<User, string | void>({
      query: (employeeId) => ({
        // url: employeeId
        //   ? `${API_END_POINTS.employee}?Id=${employeeId}`
        //   : API_END_POINTS.employee,
        url: `${API_END_POINTS.employee}?Id=${employeeId}`,
        method: "GET",
      }),
      providesTags: ["user"],
    }),
    users: builder.query<User[], string>({
      query: (queryParams) => ({
        // url: API_END_POINTS.users + `?${queryParams}`,
        url: API_END_POINTS.employees,
        method: "GET",
      }),
      providesTags: ["users"],
    }),
    user: builder.query<User, string>({
      query: (employeeId) => ({
        url: API_END_POINTS.employee + `?Id=${employeeId}`,
        method: "GET",
      }),
      providesTags: ["user"],
    }),
    userSaved: builder.mutation<User, User>({
      query: (user) => {
        return {
          url: API_END_POINTS.employees,
          method: "POST",
          body: user,
        };
      },
      invalidatesTags: ["users", "user"],
    }),
  }),
});

export const {
  useUserProfileQuery,
  useLazyUsersQuery,
  useUserSavedMutation,
  useUserQuery,
} = userService;
