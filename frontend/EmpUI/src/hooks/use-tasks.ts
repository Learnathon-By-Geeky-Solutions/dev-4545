import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { App } from "antd";
import useFilter from "@hooks/utility-hooks/use-filter";
import { Task } from "@models/task-model";
import { AppError, QueryParams } from "@models/utils-model";
import {
  useLazyTasksQuery,
  useTaskQuery,
} from "@services/task-service";
import { formatQueryParams } from "@utils/helpers";

// Hook to fetch and manage the list of tasks with filtering
export const useTasks = () => {
  const location = useLocation();

  const { getQueryParams, setQueryParams, getDefaultQueryParams } = useFilter();
  const queryParams = getQueryParams();

  const [filterParams, setFilterParams] = useState<QueryParams>({});

  const [onFetching, { isFetching, data: response }] = useLazyTasksQuery();

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

// Hook to handle saving a task (create or update)
// export const useTaskForm = () => {
//   const { message } = App.useApp();
//   const navigate = useNavigate();

//   const [taskSaved, { isLoading, isSuccess, isError, error }] =
//     useTaskSavedMutation();

//   useEffect(() => {
//     if (isSuccess) {
//       message.success("Task saved successfully.");
//       navigate("/tasks");
//     }

//     if (isError && error) {
//       message.error((error as AppError).data.error_description);
//     }
//   }, [isSuccess, isError, error]);

//   const onSaved = (task: Task) => {
//     taskSaved(task);
//   };

//   return {
//     isLoading,
//     onSaved,
//   };
// };

// Hook to fetch a single task by ID
export const useTask = (taskId: number) => {
  const { isLoading, data: task } = useTaskQuery(taskId);

  return {
    isLoading,
    task,
  };
};
