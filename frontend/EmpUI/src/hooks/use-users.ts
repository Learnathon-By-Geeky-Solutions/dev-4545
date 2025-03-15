// Import mock data functions
import { getUsersWithFilters, getUserById, saveUser } from "@mock/users";
import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { App } from "antd";
import useFilter from "@hooks/utility-hooks/use-filter";
import { User } from "@models/user-model";
import { AppError, QueryParams } from "@models/utils-model";
// Keep original imports as comments
// import {
//   useLazyUsersQuery,
//   useUserSavedMutation,
//   useUserQuery,
// } from "@services/user-service";
// import { formatQueryParams } from "@utils/helpers";

export const useUsers = () => {
  const location = useLocation();
  // Add state for mock implementation
  const [isLoading, setIsLoading] = useState(false);
  const [data, setData] = useState<User[]>([]);

  const { getQueryParams, setQueryParams, getDefaultQueryParams } = useFilter();
  const queryParams = getQueryParams();

  const [filterParams, setFilterParams] = useState<QueryParams>({});

  // Keep original API call as comment
  // const [onFetching, { isFetching, data: response }] = useLazyUsersQuery();

  useEffect(() => {
    setFilterParams(queryParams);
  }, [location.search]);

  // This effect is causing the infinite loop - we need to fix it
  useEffect(() => {
    const newQueryParams = {
      ...filterParams,
      ...getDefaultQueryParams(),
    };

    // This is causing the infinite loop - we're updating filterParams which triggers this effect again
    // setQueryParams(newQueryParams);
    // setFilterParams(newQueryParams);

    // Instead, only update the URL without updating the state again
    if (Object.keys(filterParams).length > 0) {
      setQueryParams(newQueryParams);
    }

    // Original API call (commented out)
    // onFetching(formatQueryParams(filterParams));

    // Mock implementation
    setIsLoading(true);
    setTimeout(() => {
      const filteredUsers = getUsersWithFilters(filterParams);
      setData(filteredUsers);
      setIsLoading(false);
    }, 300); // Small delay to simulate API call
  }, [filterParams]);

  // Add a separate effect for initial data loading
  useEffect(() => {
    // Initial data fetch
    const defaultParams = getDefaultQueryParams();
    setIsLoading(true);
    setTimeout(() => {
      const filteredUsers = getUsersWithFilters(defaultParams);
      setData(filteredUsers);
      setIsLoading(false);
    }, 300);
  }, []);

  // Original return (commented out)
  // const loading = isFetching;
  // return {
  //   isLoading: loading,
  //   data: response,
  // };

  // Mock implementation return
  return {
    isLoading,
    data,
  };
};

export const useUserForm = () => {
  const { message } = App.useApp();
  const navigate = useNavigate();

  // Original API mutation (commented out)
  // const [userSaved, { isLoading, isSuccess, isError, error }] =
  //   useUserSavedMutation();

  // Mock implementation state
  const [isLoading, setIsLoading] = useState(false);
  const [isSuccess, setIsSuccess] = useState(false);
  const [error, setError] = useState<AppError | null>(null);

  useEffect(() => {
    if (isSuccess) {
      message.success("User saved successfully.");
      navigate("/users");
    }

    // Original error handling (commented out)
    // if (isError && error) {
    //   message.error((error as AppError).data.error_description);
    // }

    // Mock implementation error handling
    if (error) {
      message.error(error.data.error_description);
    }
  }, [isSuccess, error]);

  const onSaved = (user: User) => {
    // Original API call (commented out)
    // userSaved({
    //   ...user,
    //   avatar: "https://i.imgur.com/DTfowdu.jpg",
    // });

    // Mock implementation
    setIsLoading(true);
    setTimeout(() => {
      try {
        saveUser({
          ...user,
          avatar: user.avatar || "https://i.imgur.com/DTfowdu.jpg",
        });
        setIsSuccess(true);
      } catch (err) {
        setError({
          data: {
            error_description: "Failed to save user",
          },
        } as AppError);
      } finally {
        setIsLoading(false);
      }
    }, 500);
  };

  return {
    isLoading,
    onSaved,
  };
};

export const useUser = (userId: number) => {
  // Original API query (commented out)
  // const { isLoading, data: user } = useUserQuery(userId);

  // Mock implementation state
  const [isLoading, setIsLoading] = useState(false);
  const [user, setUser] = useState<User | undefined>(undefined);

  // Original console log (commented out)
  // console.log("USE USER QEURY", user);

  // Original response comment (kept for reference)
  // response single user response
  // avatar: "https://i.imgur.com/DTfowdu.jpg";
  // creationAt: "2025-03-14T16:22:21.000Z";
  // email: "maria@mail.com";
  // id: 2;
  // name: "Maria";
  // password: "12345";
  // role: "customer";
  // updatedAt: "2025-03-14T16:22:21.000Z";

  // Mock implementation
  useEffect(() => {
    setIsLoading(true);
    setTimeout(() => {
      const foundUser = getUserById(userId);
      setUser(foundUser);
      setIsLoading(false);
    }, 300);
  }, [userId]);

  return {
    isLoading,
    user,
  };
};
