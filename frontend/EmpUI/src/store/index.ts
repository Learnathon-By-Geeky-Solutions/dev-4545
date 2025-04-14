import { configureStore } from '@reduxjs/toolkit';
import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux';

import authSlice from '@reducers/auth-slice';
import userSlice from '@reducers/user-slice';
import baseService from '@services/core/base-service';

export const store = configureStore({
  reducer: {
    auth: authSlice,
    user: userSlice,
    [baseService.reducerPath]: baseService.reducer
  },
  middleware: (getDefaultMiddlewares) =>
    getDefaultMiddlewares().concat([
      baseService.middleware
    ])
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;