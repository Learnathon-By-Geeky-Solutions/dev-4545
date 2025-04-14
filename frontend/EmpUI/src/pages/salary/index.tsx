import { Space, Spin } from "antd";
import PageContent from "@layouts/partials/page-content";
import PageHeader from "@layouts/partials/page-header";
import { useSalaries } from "@hooks/use-salaries";
import SalaryTable from "../../features/salaries/salary-table";

const Salaries = () => {
  const { isLoading, data: salaries } = useSalaries();

  console.log(salaries);

  return (
    <>
      <PageHeader
        title="salaries"
        subTitle="Access and adjust your preferences conveniently on our salaries page"
      />
      <PageContent>
        <Space direction="vertical" size="large" style={{ display: "flex" }}>
          <Spin spinning={isLoading}>
            <SalaryTable />
          </Spin>
        </Space>
      </PageContent>
    </>
  );
};

export default Salaries;
