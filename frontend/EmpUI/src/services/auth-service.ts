import { AuthRequest, AuthResponse } from '@models/auth-model';
import baseService from '@services/core/base-service';
import API_END_POINTS from '@utils/constants/api-end-points';

export const authService = baseService.injectEndpoints({
  endpoints: (builder) => ({
    login: builder.mutation<AuthResponse, AuthRequest>({
      query: (data) => ({
        url: API_END_POINTS.login,
        method: 'POST',
        body: data,
      }),
      invalidatesTags: ['auth', 'user']
    }),
  }),
});

export const { useLoginMutation } = authService;