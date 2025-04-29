import PageContent from "@layouts/partials/page-content";
import PageHeader from "@layouts/partials/page-header";
import { UserPartial } from "@models/user-model";
import TaskForm from "@features/tasks/task-form";

const TaskCreate = () => {
  const initialValues: UserPartial = {};

  return (
    <>
      <PageHeader title={"Tasks Create"} />
      <PageContent>
        <TaskForm initialValues={initialValues} />
      </PageContent>
    </>
  );
};

export default TaskCreate;
