export interface User {
  employeeId: string;
  name: string;
  stack: string;
  email: string;
  salt?: string;
  password?: string;
  dateOfJoin: string;
  phone: string;
}

export type UserPartial = Partial<User>;

export interface Users {
  users: User[];
}
