import { useParams } from "react-router-dom";
import { Spin } from "antd";
import PageContent from "@layouts/partials/page-content";
import PageHeader from "@layouts/partials/page-header";
import { useProject } from "@hooks/use-projects";
import ProjectForm from "../../features/projects/project-form";

const ProjectEdit = () => {
  const { id: projectId } = useParams();
  const { isLoading, project } = useProject(projectId);
  console.log("object", projectId);

  return (
    <>
      <PageHeader title={"Edit Project"} />
      <PageContent>
        <Spin spinning={isLoading}>
          <ProjectForm initialValues={project} isEditMode projectId={projectId} />
        </Spin>
      </PageContent>
    </>
  );
};

export default ProjectEdit;
