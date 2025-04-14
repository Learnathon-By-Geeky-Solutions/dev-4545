export interface AuthState {
  jwToken: string;
  refreshToken: string;
}

export interface AuthRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  access_token: string;
  refresh_token: string;
  employeeId?: string;
}
