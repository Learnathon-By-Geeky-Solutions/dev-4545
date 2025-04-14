import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { User } from '@models/user-model';

const initialState = {
  id: 0,
  name: '',
  email: '',
  role: ''
} as User;

const userSlice = createSlice({
  name: 'user',
  initialState: initialState,
  reducers: {
    setUser: (state, action: PayloadAction<User>) => {
      return { ...state, ...action.payload };
    },
    unsetUser: () => {
      return initialState;
    }
  }
});

export const { setUser, unsetUser } = userSlice.actions;

export default userSlice.reducer;