import { Space, Spin } from "antd";
import PageContent from "@layouts/partials/page-content";
import PageHeader from "@layouts/partials/page-header";
import { usePerformances } from "@hooks/use-performances";
import PerformanceTable from "../../features/performances/performance-table-columns";

const Performances = () => {
  const { isLoading, data: performances } = usePerformances();

  console.log(performances);

  return (
    <>
      <PageHeader
        title="Performances"
        subTitle="Access and adjust your preferences conveniently on our Performances page"
      />
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
