import { useDispatch } from "react-redux";
import useResponseError from "@hooks/utility-hooks/use-response-error";
import { AuthRequest } from "@models/auth-model";
import { setCredentials } from "@reducers/auth-slice";
import { useLoginMutation } from "@services/auth-service";

const useAuth = () => {
  const dispatch = useDispatch();
  const { displayError } = useResponseError();

  const [login, { isLoading }] = useLoginMutation();

  const onLogin = async (requestData: AuthRequest) => {
    try {
      const response = await login(requestData).unwrap();

      // console.log("Login response:", response);

      const authData = {
        jwToken: response.jwToken,
        refreshToken: response.refreshToken,
      };

      // Ensure tokens are stored in localStorage
      try {
        localStorage.setItem("jwToken", response.jwToken);
        localStorage.setItem("refreshToken", response.refreshToken);
        localStorage.setItem("role", response.role);
        localStorage.setItem("employeeId", response.id);
      } catch (storageError) {
        console.error("Failed to store tokens in localStorage:", storageError);
      }

      dispatch(setCredentials(authData));

    } catch (error: any) {
      console.error("Login error:", error);
      displayError(error.data || { message: "Login failed" });
    }
  };

  return {
    onLogin,
    isLoading,
  };
};

export default useAuth;
