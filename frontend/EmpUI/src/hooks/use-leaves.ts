import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { App } from "antd";
import useFilter from "@hooks/utility-hooks/use-filter";
import { Leave } from "@models/leave-model";
import { AppError, QueryParams } from "@models/utils-model";
import {
  useLazyLeavesQuery,
  useLeaveQuery,
  useLeaveSavedMutation,
  useEmpLeaveQuery,
} from "@services/leave-service";
import { formatQueryParams } from "@utils/helpers";

// Hook to fetch and manage the list of leaves with filtering
export const useLeaves = () => {
  const location = useLocation();

  const { getQueryParams, setQueryParams, getDefaultQueryParams } = useFilter();
  const queryParams = getQueryParams();

  const [filterParams, setFilterParams] = useState<QueryParams>({});

  const [onFetching, { isFetching, data: response }] = useLazyLeavesQuery();

  useEffect(() => {
    setFilterParams(queryParams);
  }, [location.search]);

  useEffect(() => {
    const newQueryParams = {
      ...filterParams,
      ...getDefaultQueryParams(),
    };

    setQueryParams(newQueryParams);
    setFilterParams(newQueryParams);

    onFetching(formatQueryParams(newQueryParams));
  }, []);

  const loading = isFetching;

  return {
    isLoading: loading,
    data: response,
  };
};

// Hook to handle saving a leave (create or update)
export const useLeaveForm = () => {
  const { message } = App.useApp();
  const navigate = useNavigate();

  const [leaveSaved, { isLoading, isSuccess, isError, error }] =
    useLeaveSavedMutation();

  useEffect(() => {
    if (isSuccess) {
      message.success("Leave saved successfully.");
      navigate("/leave");
    }

    if (isError && error) {
      message.error((error as AppError).data.error_description);
    }
  }, [isSuccess, isError, error]);

  const onSaved = (leave: Leave) => {
    leaveSaved(leave);
  };

  return {
    isLoading,
    onSaved,
  };
};

// Hook to fetch a single leave by ID
export const useLeave = (leaveId: number) => {
  const { isLoading, data: leave } = useLeaveQuery(leaveId);

  return {
    isLoading,
    leave,
  };
};
export const useEmpLeave = (empId: string) => {
  const { isLoading, data: leave } = useEmpLeaveQuery(empId);

  return {
    isLoading,
    leave,
  };
};
