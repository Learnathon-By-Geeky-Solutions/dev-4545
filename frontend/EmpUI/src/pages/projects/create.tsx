import PageContent from "@layouts/partials/page-content";
import PageHeader from "@layouts/partials/page-header";
import { UserPartial } from "@models/user-model";
import ProjectForm from "@features/projects/project-form";

const ProjectCreate = () => {
  const initialValues: UserPartial = {};

  return (
    <>
      <PageHeader title={"Projects Create"} />
      <PageContent>
        <ProjectForm initialValues={initialValues} />
      </PageContent>
    </>
  );
};

export default ProjectCreate;