import { Space, Spin,Button} from "antd";
import PageContent from "@layouts/partials/page-content";
import PageHeader from "@layouts/partials/page-header";
import { usePerformances } from "@hooks/use-performances";
import PerformanceTable from "../../features/performances/performance-table-columns";
import { Link } from "react-router-dom";
import { PlusCircleOutlined } from "@ant-design/icons";

const Performances = () => {
  const { isLoading, data: performances } = usePerformances();

  console.log(performances);

  return (
    <>
      <PageHeader
        title="Performances"
        subTitle="Access and adjust your preferences conveniently on our Performances page"
      >
        <Link to={"/performances/create"}>
          <Button type={"primary"} icon={<PlusCircleOutlined />}>
            Add Performance
          </Button>
        </Link>
      </PageHeader>
      <PageContent>
        <Space direction="vertical" size="large" style={{ display: "flex" }}>
          <Spin spinning={isLoading}>
            <PerformanceTable />
          </Spin>
        </Space>
      </PageContent>
    </>
  );
};

export default Performances;
