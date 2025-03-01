import { User } from '@models/user-model';
import baseService from '@services/core/base-service';
import API_END_POINTS from '@utils/constants/api-end-points';

export const userService = baseService.injectEndpoints({
  endpoints: (builder) => ({
    userProfile: builder.query<User, void>({
      query: () => ({
        url: API_END_POINTS.user,
        method: 'GET'
      }),
      providesTags: ['user']
    }),
    users: builder.query<User[], string>({
      query: (queryParams) => ({
        url: API_END_POINTS.users + `?${queryParams}`,
        method: 'GET'
      }),
      providesTags: ['users']
    }),
    user: builder.query<User, number>({
      query: (userId) => ({
        url: API_END_POINTS.users + `/${userId}`,
        method: 'GET'
      }),
      providesTags: ['user']
    }),
    userSaved: builder.mutation<User, User>({
      query: (user) => {
        const requestUrl = user?.id ? API_END_POINTS.users + `/${user.id}` : API_END_POINTS.users;
        const requestMethod = user?.id ? 'PUT' : 'POST';
        
        return {
          url: requestUrl,
          method: requestMethod,
          body: user
        };
      },
      invalidatesTags: ['users', 'user']
    })
  }),
});

export const { useUserProfileQuery, useLazyUsersQuery, useUserSavedMutation, useUserQuery } = userService;