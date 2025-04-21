import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { App } from "antd";
import useFilter from "@hooks/utility-hooks/use-filter";
import { Feature } from "@models/feature-model";
import { AppError, QueryParams } from "@models/utils-model";
import {
  useLazyFeaturesQuery,
  useFeatureQuery,
  useEmpFeaturesQuery,
  useFeatureSavedMutation,
} from "@services/feature-service";
import { formatQueryParams } from "@utils/helpers";

// Hook to fetch and manage the list of features with filtering
export const useFeatures = () => {
  const location = useLocation();

  const { getQueryParams, setQueryParams, getDefaultQueryParams } = useFilter();
  const queryParams = getQueryParams();

  const [filterParams, setFilterParams] = useState<QueryParams>({});

  const [onFetching, { isFetching, data: response }] = useLazyFeaturesQuery();

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

// Hook to handle saving a feature (create or update)
export const useFeatureForm = () => {
  const { message } = App.useApp();
  const navigate = useNavigate();

  const [featureSaved, { isLoading, isSuccess, isError, error }] =
    useFeatureSavedMutation();

  useEffect(() => {
    if (isSuccess) {
      message.success("Feature saved successfully.");
      navigate("/features");
    }

    if (isError && error) {
      message.error((error as AppError).data.error_description);
    }
  }, [isSuccess, isError, error]);

  const onSaved = (feature: Feature) => {
    featureSaved(feature);
  };

  return {
    isLoading,
    onSaved,
  };
};

// Hook to fetch a single feature by ID
export const useFeature = (featureId: number) => {
  const { isLoading, data: feature } = useFeatureQuery(featureId);

  return {
    isLoading,
    feature,
  };
};
export const useEmpFeatures = (empId: string) => {
  const { isLoading, data: features } = useEmpFeaturesQuery(empId);

  return {
    isLoading,
    features,
  };
};
