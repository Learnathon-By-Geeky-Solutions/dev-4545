import { setCredentials } from "@reducers/auth-slice";

import { RootState, useAppDispatch, useAppSelector } from "@/store";

const useAuthentication = () => {
  const authState = useAppSelector((state: RootState) => state.auth);

  const dispatch = useAppDispatch();
  const authLocalData = localStorage.getItem("auth");

  const isAuthenticated = () => {
    if (!authState?.jwToken && authLocalData) {
      const authData = JSON.parse(authLocalData);
      const token = authData.jwToken || authData.accessToken;

      dispatch(
        setCredentials({
          jwToken: token,
          refreshToken: authData.refreshToken,
        })
      );
    }

    return !!authState?.jwToken;
  };

  return {
    isAuthenticated,
  };
};

export default useAuthentication;
