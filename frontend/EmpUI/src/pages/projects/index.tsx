import { Button, Space, Spin } from "antd";
import PageContent from "@layouts/partials/page-content";
import PageHeader from "@layouts/partials/page-header";
import { useProjects } from "@hooks/use-projects";
import ProjectTable from "../../features/projects/project-table";
import { Link } from "react-router-dom";
import { PlusCircleOutlined } from "@ant-design/icons";

const Projects = () => {
  const { isLoading, data: projects } = useProjects();

  console.log(projects);

  return (
    <>
      <PageHeader
        title="projects"
        subTitle="Access and adjust your preferences conveniently on our projects page"
      >
        <Link to={"/projects/create"}>
          <Button type={"primary"} icon={<PlusCircleOutlined />}>
            Create Project
          </Button>
        </Link>
        </PageHeader>
      <PageContent>
        <Space direction="vertical" size="large" style={{ display: "flex" }}>
          <Spin spinning={isLoading}>
            <ProjectTable />
          </Spin>
        </Space>
      </PageContent>
    </>
  );
};

export default Projects;
