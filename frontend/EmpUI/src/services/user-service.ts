import { User } from "@models/user-model";
// Keep original imports as comments
// import baseService from '@services/core/base-service';
// import API_END_POINTS from '@utils/constants/api-end-points';

// Import mock data functions
import {
  getCurrentUserProfile,
  getUserById,
  getUsersWithFilters,
  saveUser,
} from "@mock/users";
import { useState, useEffect } from "react";

// Original RTK Query implementation (commented out)
/*
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
*/

// Mock implementation for userProfileQuery
export const useUserProfileQuery = () => {
  const [isLoading, setIsLoading] = useState(true);
  const [data, setData] = useState<User | null>(null);
  const [isSuccess, setIsSuccess] = useState(false);
  const [error, setError] = useState<any>(null);

  useEffect(() => {
    // Simulate API call delay
    setTimeout(() => {
      try {
        const userProfile = getCurrentUserProfile();
        setData(userProfile);
        setIsSuccess(true);
      } catch (err) {
        setError(err);
      } finally {
        setIsLoading(false);
      }
    }, 500);
  }, []);

  return { isLoading, data, isSuccess, error };
};

// Mock implementation for useLazyUsersQuery
export const useLazyUsersQuery = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [data, setData] = useState<User[]>([]);
  const [error, setError] = useState<any>(null);

  const fetchUsers = (queryParams: string = "") => {
    setIsLoading(true);

    // Parse query params if needed
    const params = queryParams
      ? JSON.parse(
          `{"${queryParams.replace(/&/g, '","').replace(/=/g, '":"')}"}`
        )
      : {};

    setTimeout(() => {
      try {
        const users = getUsersWithFilters(params);
        setData(users);
      } catch (err) {
        setError(err);
      } finally {
        setIsLoading(false);
      }
    }, 300);
  };

  return [fetchUsers, { isFetching: isLoading, data, error }];
};

// Mock implementation for useUserQuery
export const useUserQuery = (userId: number) => {
  const [isLoading, setIsLoading] = useState(true);
  const [data, setData] = useState<User | null>(null);
  const [error, setError] = useState<any>(null);

  useEffect(() => {
    setTimeout(() => {
      try {
        const user = getUserById(userId);
        setData(user || null);
      } catch (err) {
        setError(err);
      } finally {
        setIsLoading(false);
      }
    }, 300);
  }, [userId]);

  return { isLoading, data, error };
};

// Mock implementation for useUserSavedMutation
export const useUserSavedMutation = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [isSuccess, setIsSuccess] = useState(false);
  const [isError, setIsError] = useState(false);
  const [error, setError] = useState<any>(null);
  const [data, setData] = useState<User | null>(null);

  const saveUserData = async (userData: User) => {
    setIsLoading(true);
    setIsSuccess(false);
    setIsError(false);
    setError(null);

    return new Promise<User>((resolve, reject) => {
      setTimeout(() => {
        try {
          const savedUser = saveUser(userData);
          setData(savedUser);
          setIsSuccess(true);
          resolve(savedUser);
        } catch (err) {
          setIsError(true);
          setError(err);
          reject(err);
        } finally {
          setIsLoading(false);
        }
      }, 500);
    });
  };

  return [saveUserData, { isLoading, isSuccess, isError, error, data }];
};
