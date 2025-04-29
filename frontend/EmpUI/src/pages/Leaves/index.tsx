import { Space, Spin,Button } from "antd";
import PageContent from "@layouts/partials/page-content";
import PageHeader from "@layouts/partials/page-header";
import { useLeaves } from "@hooks/use-leaves";
import LeaveTable from "../../features/leaves/leave-table";
import { Link } from "react-router-dom";
import { PlusCircleOutlined } from "@ant-design/icons";

const Leaves = () => {
  const { isLoading, data: leaves } = useLeaves();

  console.log(leaves);

  return (
    <>
      <PageHeader
        title="Leaves"
        subTitle="Access and adjust your preferences conveniently on our Leaves page"
      >
        <Link to={"/leaves/create"}>
          <Button type={"primary"} icon={<PlusCircleOutlined />}>
            Leave Application
          </Button>
        </Link>
      </PageHeader>
      <PageContent>
        <Space direction="vertical" size="large" style={{ display: "flex" }}>
          <Spin spinning={isLoading}>
            {/* Leaves Table */}
            <LeaveTable />
          </Spin>
        </Space>
      </PageContent>
    </>
  );
};

export default Leaves;
