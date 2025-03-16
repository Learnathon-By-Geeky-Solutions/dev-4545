// Import mock auth function
import { authenticateUser } from "@mock/auth";
// Mock implementation using React hooks
import { useState } from "react";
import { AuthRequest, AuthResponse } from "@models/auth-model";
// Keep original imports as comments
// import baseService from '@services/core/base-service';
// import API_END_POINTS from '@utils/constants/api-end-points';

// Original RTK Query implementation (commented out)
/*
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
*/

export const useLoginMutation = (): [
  (credentials: AuthRequest) => Promise<AuthResponse>,
  { isLoading: boolean; isError: boolean; error: any },
] => {
  const [isLoading, setIsLoading] = useState(false);
  const [isError, setIsError] = useState(false);
  const [error, setError] = useState<any>(null);

  const login = async (credentials: AuthRequest): Promise<AuthResponse> => {
    setIsLoading(true);
    setIsError(false);
    setError(null);

    try {
      const response = await authenticateUser(credentials);
      setIsLoading(false);
      return response as AuthResponse;
    } catch (err) {
      setIsLoading(false);
      setIsError(true);
      setError(err);
      throw err;
    }
  };

  return [login, { isLoading, isError, error }];
};
