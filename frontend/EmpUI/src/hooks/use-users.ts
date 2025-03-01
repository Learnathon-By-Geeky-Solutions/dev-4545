import { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { App } from 'antd';
import useFilter from '@hooks/utility-hooks/use-filter';
import { User } from '@models/user-model';
import { AppError, QueryParams } from '@models/utils-model';
import { useLazyUsersQuery, useUserSavedMutation, useUserQuery } from '@services/user-service';
import { formatQueryParams } from '@utils/helpers';

export const useUsers = () =>{
  const location = useLocation();

  const { getQueryParams, setQueryParams, getDefaultQueryParams } = useFilter();
  const queryParams = getQueryParams();

  const [filterParams, setFilterParams] = useState<QueryParams>({});
  
  const [onFetching, { isFetching, data: response }] = useLazyUsersQuery();

  useEffect(() => {
    setFilterParams(queryParams);
  }, [location.search]);
  
  useEffect(() => {
    const newQueryParams = {
      ...filterParams,
      ...getDefaultQueryParams()
    };
    
    setQueryParams(newQueryParams);
    setFilterParams(newQueryParams);
    
    onFetching(formatQueryParams(filterParams));
  }, []);

  const loading = isFetching;
  
  return {
    isLoading: loading,
    data: response
  };
};

export const useUserForm = () => {
  const { message } = App.useApp();
  const navigate = useNavigate();
  
  const [userSaved, { isLoading, isSuccess, isError, error }] = useUserSavedMutation();
  
  useEffect(() => {
    if (isSuccess) {
      message.success('User saved successfully.');
      navigate('/users');
    }
    
    if (isError && error) {
      message.error((error as AppError).data.error_description);
    }
  }, [isSuccess, isError, error]);
  
  const onSaved = (user: User) => {
    userSaved({
      ...user,
      avatar: 'https://i.imgur.com/DTfowdu.jpg'
    });
  };
  
  return {
    isLoading,
    onSaved
  };
};

export const useUser = (userId: number) => {
  const { isLoading, data: user } = useUserQuery(userId);
  
  return {
    isLoading,
    user
  };
};