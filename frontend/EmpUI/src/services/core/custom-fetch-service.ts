import {
  BaseQueryFn,
  FetchArgs,
  fetchBaseQuery,
  FetchBaseQueryError
} from '@reduxjs/toolkit/query/react';
import { Mutex } from 'async-mutex';
import { AuthResponse } from '@models/auth-model';
import { setCredentials, logOut} from '@reducers/auth-slice';
import API_END_POINTS from '@utils/constants/api-end-points';
import { RootState } from '@/store';

const baseUrl = process.env.API_BASE_URL as string;
const mutex = new Mutex();

const baseApi = fetchBaseQuery({
  baseUrl: baseUrl,
  prepareHeaders: (headers: Headers, { getState }) => {
    const state = getState() as RootState;
    const token = state.auth.accessToken as string;

    if (token) {
      headers.set('Authorization', `Bearer ${token}`);
    }

    return headers;
  }
});

const baseQueryWithReAuth: BaseQueryFn<
  string | FetchArgs,
  any,
  FetchBaseQueryError> = async (args, api, extraOptions) => {
  
  const isFormData = args && (args as FetchArgs).body instanceof FormData;
  const headers = new Headers();
  
  if (!isFormData) {
    headers.set('Content-Type', 'application/json');
  }
  
  const requestArgs: FetchArgs = typeof args === 'string' ? { url: args } : { ...args };
  requestArgs.headers = headers;
  
  await mutex.waitForUnlock();
  
  let response = await baseApi(requestArgs, api, extraOptions);

  if (response.error && response.error.status === 403) {
    
    if (!mutex.isLocked()) {
      const release = await mutex.acquire();
    
      try {
        const state = api.getState() as RootState;
        const refreshToken = state.auth.refreshToken;
        
        if (refreshToken) {
          const formData = new FormData();
          formData.append('refresh_token', state.auth.refreshToken as string);
          
          const refreshResponse = await baseApi({
              url: API_END_POINTS.refreshToken,
              method: 'POST',
              body: formData,
              ...headers
            },
            api,
            extraOptions,
          );
          
          if (refreshResponse.error) {
            api.dispatch(logOut());
            localStorage.removeItem('auth');
          } else {
            const refreshResponseData = refreshResponse.data as AuthResponse;
            
            const authData = {
              accessToken: refreshResponseData.access_token,
              refreshToken: refreshResponseData.refresh_token
            };
            
            api.dispatch(setCredentials(authData));
            localStorage.setItem('auth', JSON.stringify(authData));
            
            headers.set('Authorization', `Bearer ${authData.accessToken}`);
            requestArgs.headers = headers;
            
            response = await baseApi(requestArgs, api, extraOptions);
          }
        } else {
          api.dispatch(logOut());
          localStorage.removeItem('auth');
        }
        
      } finally {
        release();
      }
    } else {
      await mutex.waitForUnlock();
      const state = api.getState() as RootState;
      const token = state.auth.accessToken;
      
      headers.set('Authorization', `Bearer ${token}`);
      requestArgs.headers = headers;
      
      response = await baseApi(requestArgs, api, extraOptions);
    }
  }

  return response;
};

export default baseQueryWithReAuth;