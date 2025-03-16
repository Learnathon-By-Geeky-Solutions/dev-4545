import { AuthRequest, AuthResponse } from "@models/auth-model";
import { mockUsers } from "./users";

// Mock function to authenticate a user
export const authenticateUser = (
  credentials: AuthRequest
): Promise<AuthResponse | null> => {
  return new Promise((resolve, reject) => {
    // Simulate network delay
    setTimeout(() => {
      // Find user with matching email and password
      const user = mockUsers.find(
        (user) =>
          user.email === credentials.email &&
          user.password === credentials.password
      );

      if (user) {
        // Store the current user ID in localStorage
        localStorage.setItem("currentUserId", user.id!.toString());

        // Generate mock tokens
        const response: AuthResponse = {
          access_token: `mock_access_token_${user.id}_${Date.now()}`,
          refresh_token: `mock_refresh_token_${user.id}_${Date.now()}`,
        };
        resolve(response);
      } else {
        // Simulate API error response
        reject({
          status: 401,
          data: {
            error: "invalid_credentials",
            error_description: "Invalid email or password",
          },
        });
      }
    }, 500); // 500ms delay to simulate network
  });
};
