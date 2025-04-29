import { Space, Spin ,Button} from "antd";
import PageContent from "@layouts/partials/page-content";
import PageHeader from "@layouts/partials/page-header";
import { useFeatures } from "@hooks/use-features";
import FeatureTable from "../../features/features/feature-table";
import { Link } from "react-router-dom";
import { PlusCircleOutlined } from "@ant-design/icons";

const Features = () => {
  const { isLoading, data: features } = useFeatures();

  console.log(features);

  return (
    <>
       <PageHeader
        title="Features"
        subTitle="Access and adjust your preferences conveniently on our Features page"
      >
        <Link to={"/features/create"}>
          <Button type={"primary"} icon={<PlusCircleOutlined />}>
            Create Feature
          </Button>
        </Link>
      </PageHeader>
      <PageContent>
        <Space direction="vertical" size="large" style={{ display: "flex" }}>
          <Spin spinning={isLoading}>
            {/* call the features table component */}
            <FeatureTable />
          </Spin>
        </Space>
      </PageContent>
    </>
  );
};

export default Features;
