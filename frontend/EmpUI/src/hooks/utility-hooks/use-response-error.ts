import { SerializedError } from '@reduxjs/toolkit';
import { FetchBaseQueryError } from '@reduxjs/toolkit/query';
import _ from 'lodash';
import { App } from 'antd';

const useResponseError = () => {
  const { message } = App.useApp();
  
  const displayError = (error: FetchBaseQueryError | SerializedError | any) => {
    if (isFetchBaseQueryError(error)) {
      const errorData = error.data as { messages?: Array<{ description: string }> } | undefined;
      
      if (errorData?.messages && Array.isArray(errorData.messages)) {
        errorData.messages.forEach((msg) => {
          message.error(msg.description);
        });
      } else {
        message.error('An error occurred: ' + (errorData?.messages?.[0]?.description || 'Unknown error'));
      }
    } else if (isSerializedError(error)) {
      message.error(error.message || 'An unexpected error occurred');
    } else if (_.has(error, 'error')) {
      message.error(error.error || 'An unexpected error occurred');
    } else {
      message.error('An unexpected error occurred');
    }
  };
  
  const isFetchBaseQueryError = (error: any): error is FetchBaseQueryError => {
    return typeof error === 'object' && error !== null && 'status' in error;
  };
  
  const isSerializedError = (error: any): error is SerializedError => {
    return typeof error === 'object' && error !== null && 'name' in error;
  };
  
  return {
    displayError,
  };
};

export default useResponseError;
