import { Feature } from "@models/feature-model";
import baseService from "@services/core/base-service";
import API_END_POINTS from "@utils/constants/api-end-points";

export const featureService = baseService.injectEndpoints({
  endpoints: (builder) => ({
    features: builder.query<Feature[], string>({
      query: () => ({
        url: API_END_POINTS.features,
        method: "GET",
      }),
      providesTags: ["features"],
    }),
    feature: builder.query<Feature, string>({
      query: (featureId) => ({
        url: API_END_POINTS.features + `/${featureId}`,
        method: "GET",
      }),
      providesTags: ["feature"],
    }),
    empFeatures: builder.query<User, string>({
      query: (empId) => ({
        url: API_END_POINTS.empFeatures + `/${empId}`,
        method: "GET",
      }),
      providesTags: ["employee-features"],
    }),
    featureSaved: builder.mutation<Feature, Feature>({
      query: (feature) => {
        const requestUrl = feature?.isEditMode
          ? API_END_POINTS.features + `/${feature.featureId}`
          : API_END_POINTS.features;
        const requestMethod = feature?.isEditMode ? "PUT" : "POST";
     

        console.log("feature request method ", requestUrl, feature);

        return {
          url: requestUrl,
          method: requestMethod,
          body: feature.feature,
        };
      },
      invalidatesTags: ["features", "feature"],
    }),
    deleteFeature: builder.mutation<void, number>({
      query: (featureId) => ({
        url: `${API_END_POINTS.features}?Id=${featureId}`,
        method: "DELETE",
      }),
      invalidatesTags: ["features"], // This will auto-refresh the features list
    }),
  }),
});

export const {
  useLazyFeaturesQuery,
  useFeatureSavedMutation,
  useEmpFeaturesQuery,
  useFeatureQuery,
  useDeleteFeatureMutation,
} = featureService;
