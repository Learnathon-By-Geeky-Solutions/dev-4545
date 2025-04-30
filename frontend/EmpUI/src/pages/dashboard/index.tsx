import { Card, Typography } from "antd";
import PageContent from "@layouts/partials/page-content";
import PageHeader from "@layouts/partials/page-header";
import Charts from "@/layouts/partials/charts";
import { BoldOutlined } from "@ant-design/icons";

const Dashboard = () => {
  return (
    <>
      <PageHeader
        title="Dashboard"
        subTitle="Quick access to analytical insights"
      />
      <PageContent>
        <Card>
          <Typography.Title level={3}>Welcome to TaskTracking</Typography.Title>
        </Card>

        {/* Chart Layout */}
        <Card className="mt-6">
          <Charts/>
        </Card>
      </PageContent>
    </>
  );
};

export default Dashboard;
