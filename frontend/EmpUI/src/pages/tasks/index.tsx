import { Button, Space, Spin } from "antd";
import PageContent from "@layouts/partials/page-content";
import PageHeader from "@layouts/partials/page-header";
import { useTasks } from "@hooks/use-tasks";
import TaskTable from "../../features/tasks/task-table";
import { Link } from "react-router-dom";
import { PlusCircleOutlined } from "@ant-design/icons";

const Tasks = () => {
  const { isLoading, data: tasks } = useTasks();

  console.log(tasks);

  return (
    <>
      <PageHeader
        title="Tasks"
        subTitle="Access and adjust your preferences conveniently on our Tasks page"
      >
        <Link to={"/tasks/create"}>
          <Button type={"primary"} icon={<PlusCircleOutlined />}>
            Create Task
          </Button>
        </Link>
      </PageHeader>
      <PageContent>
        <Space direction="vertical" size="large" style={{ display: "flex" }}>
          <Spin spinning={isLoading}>
            <TaskTable />
          </Spin>
        </Space>
      </PageContent>
    </>
  );
};

export default Tasks;
