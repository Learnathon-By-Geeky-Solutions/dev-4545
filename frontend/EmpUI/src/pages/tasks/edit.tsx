import { useParams } from "react-router-dom";
import { Spin } from "antd";
import UserForm from "@features/users/user-form";
import { useUser } from "@hooks/use-users";
import PageContent from "@layouts/partials/page-content";
import PageHeader from "@layouts/partials/page-header";
import { useTask } from "@hooks/use-tasks";

const TaskEdit = () => {
  const params = useParams();
  const taskId = Number(params.id);
  const { isLoading, task } = useTask(taskId);

  return (
    <>
      <PageHeader title={"Task"} />
      <PageContent>
        <Spin spinning={isLoading}>
          <UserForm initialValues={user} isEditMode />
        </Spin>
      </PageContent>
    </>
  );
};

export default TaskEdit;
