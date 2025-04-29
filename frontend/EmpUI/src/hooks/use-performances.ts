import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { App } from "antd";
import useFilter from "@hooks/utility-hooks/use-filter";
import { Performance } from "@models/performance-model";
import { AppError, QueryParams } from "@models/utils-model";
import {
  useLazyPerformancesQuery,
  usePerformanceQuery,
  usePerformanceSavedMutation,
} from "@services/performance-service";
import { formatQueryParams } from "@utils/helpers";

// Hook to fetch and manage the list of performances with filtering
export const usePerformances = () => {
  const location = useLocation();

  const { getQueryParams, setQueryParams, getDefaultQueryParams } = useFilter();
  const queryParams = getQueryParams();

  const [filterParams, setFilterParams] = useState<QueryParams>({});

  const [onFetching, { isFetching, data: response }] =
    useLazyPerformancesQuery();

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

// Hook to handle saving a performance (create or update)
export const usePerformanceForm = () => {
  const { message } = App.useApp();
  const navigate = useNavigate();

  const [performanceSaved, { isLoading, isSuccess, isError, error }] =
    usePerformanceSavedMutation();

  useEffect(() => {
    if (isSuccess) {
      message.success("Performance saved successfully.");
      navigate(`/users`);
    }

    if (isError && error) {
      message.error((error as AppError).data.error_description);
    }
  }, [isSuccess, isError, error]);

  const onSaved = async (performance: Performance, isEditMode: boolean) => {
    await performanceSaved({ performance, isEditMode });
  };

  return {
    isLoading,
    onSaved,
  };
};

// Hook to fetch a single performance by ID
export const usePerformance = (employeeId: number) => {
  const { isLoading, data: performance } = usePerformanceQuery(employeeId);

  return {
    isLoading,
    performance,
  };
};
