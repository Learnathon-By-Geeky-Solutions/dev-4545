import { Project } from "@models/project-model";
import baseService from "@services/core/base-service";
import API_END_POINTS from "@utils/constants/api-end-points";

export const projectService = baseService.injectEndpoints({
  endpoints: (builder) => ({
    projects: builder.query<Project[], string>({
      query: () => ({
        url: API_END_POINTS.projects,
        method: "GET",
      }),
      providesTags: ["projects"],
    }),
    project: builder.query<Project, string>({
      query: (projectId) => ({
        url: API_END_POINTS.projects + `?Id=${projectId}`,
        method: "GET",
      }),
      providesTags: ["project"],
    }),
    empProjects: builder.query<User, string>({
      query: (empId) => ({
        url: API_END_POINTS.empProjects + `/${empId}`,
        method: "GET",
      }),
      providesTags: ["employee-projects"],
    }),
    projectSaved: builder.mutation<Project, Project>({
      query: (project) => {
        const requestUrl = project?.projectId
          ? API_END_POINTS.projects + `/${project.projectId}`
          : API_END_POINTS.projects;
        const requestMethod = project?.projectId ? "PUT" : "POST";

        console.log("project request method ", requestUrl, requestMethod);

        return {
          url: requestUrl,
          method: requestMethod,
          body: project,
        };
      },
      invalidatesTags: ["projects", "project"],
    }),
    deleteProject: builder.mutation<void, number>({
      query: (projectId) => ({
        url: `${API_END_POINTS.projects}?Id=${projectId}`,
        method: "DELETE",
      }),
      invalidatesTags: ["projects"], // This will auto-refresh the projects list
    }),
  }),
});

export const {
  useLazyProjectsQuery,
  useProjectSavedMutation,
  useEmpProjectsQuery,
  useProjectQuery,
  useDeleteProjectMutation,
} = projectService;
