import { Space, Spin } from "antd";
import PageContent from "@layouts/partials/page-content";
import PageHeader from "@layouts/partials/page-header";
import { useFeatures } from "@hooks/use-features";

const Features = () => {
  const { isLoading, data: features } = useFeatures();

  console.log(features);

  return (
    <>
      <PageHeader
        title="Features"
        subTitle="Access and adjust your preferences conveniently on our Features page"
      />
      <PageContent>
        <Space direction="vertical" size="large" style={{ display: "flex" }}>
          <Spin spinning={isLoading}>
            {/* call the features table component */}
          </Spin>
        </Space>
      </PageContent>
    </>
  );
};

export default Features;
