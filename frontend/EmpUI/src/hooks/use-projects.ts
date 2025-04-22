import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { App } from "antd";
import useFilter from "@hooks/utility-hooks/use-filter";
import { Project } from "@models/project-model";
import { AppError, QueryParams } from "@models/utils-model";
import {
  useLazyProjectsQuery,
  useProjectQuery,
  useEmpProjectsQuery,
  useProjectSavedMutation,
} from "@services/project-service";
import { formatQueryParams } from "@utils/helpers";

// Hook to fetch and manage the list of projects with filtering
export const useProjects = () => {
  const location = useLocation();

  const { getQueryParams, setQueryParams, getDefaultQueryParams } = useFilter();
  const queryParams = getQueryParams();

  const [filterParams, setFilterParams] = useState<QueryParams>({});

  const [onFetching, { isFetching, data: response }] = useLazyProjectsQuery();

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
export const useProjectForm = () => {
  const { message } = App.useApp();
  const navigate = useNavigate();

  const [projectSaved, { isLoading, isSuccess, isError, error }] =
    useProjectSavedMutation();

  useEffect(() => {
    if (isSuccess) {
      message.success("Project saved successfully.");
      navigate("/projects");
    }

    if (isError && error) {
      message.error((error as AppError).data.error_description);
    }
  }, [isSuccess, isError, error]);

  const onSaved = (project: Project) => {
    projectSaved(project);
  };

  return {
    isLoading,
    onSaved,
  };
};

// Hook to fetch a single project by ID
export const useProject = (projectId: number) => {
  const { isLoading, data: project } = useProjectQuery(projectId);

  return {
    isLoading,
    project,
  };
};
export const useEmpProjects = (empId: string) => {
  const { isLoading, data: projects } = useEmpProjectsQuery(empId);

  return {
    isLoading,
    projects,
  };
};
