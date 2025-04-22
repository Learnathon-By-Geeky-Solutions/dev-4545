import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { App } from "antd";
import useFilter from "@hooks/utility-hooks/use-filter";
import { Salary } from "@models/salary-model";
import { AppError, QueryParams } from "@models/utils-model";
import {
  useLazySalariesQuery,
  useSalaryQuery,
  useSalarySavedMutation,
} from "@services/salary-service";
import { formatQueryParams } from "@utils/helpers";

// Hook to fetch and manage the list of projects with filtering
export const useSalaries = () => {
  const location = useLocation();

  const { getQueryParams, setQueryParams, getDefaultQueryParams } = useFilter();
  const queryParams = getQueryParams();

  const [filterParams, setFilterParams] = useState<QueryParams>({});

  const [onFetching, { isFetching, data: response }] = useLazySalariesQuery();

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

// Hook to handle saving a project (create or update)
export const useSalaryForm = () => {
  const { message } = App.useApp();
  const navigate = useNavigate();

  const [salarySaved, { isLoading, isSuccess, isError, error }] =
    useSalarySavedMutation();

  useEffect(() => {
    if (isSuccess) {
      message.success("Salary saved successfully.");
      navigate("/salaries");
    }

    if (isError && error) {
      message.error((error as AppError).data.error_description);
    }
  }, [isSuccess, isError, error]);

  const onSaved = (salary: Salary) => {
    salarySaved(salary);
  };

  return {
    isLoading,
    onSaved,
  };
};

// Hook to fetch a single project by ID
export const useSalary = (salaryId: number) => {
  const { isLoading, data: salary } = useSalaryQuery(salaryId);

  return {
    isLoading,
    salary,
  };
};
