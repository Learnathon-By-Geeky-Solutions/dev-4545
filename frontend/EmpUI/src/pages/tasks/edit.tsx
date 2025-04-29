import { useParams } from "react-router-dom";
import { Spin } from "antd";
import PageContent from "@layouts/partials/page-content";
import PageHeader from "@layouts/partials/page-header";
import { useTask } from "@hooks/use-tasks";
import TaskForm from "../../features/tasks/task-form";

const TaskEdit = () => {
  const { id: taskId } = useParams();
  const { isLoading, task } = useTask(taskId);

  return (
    <>
      <PageHeader title={"Edit Task"} />
      <PageContent>
        <Spin spinning={isLoading}>
          <TaskForm initialValues={task} isEditMode taskId={taskId} />
        </Spin>
      </PageContent>
    </>
  );
};

export default TaskEdit;
