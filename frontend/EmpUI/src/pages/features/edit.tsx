import { useParams } from "react-router-dom";
import { Spin } from "antd";
import PageContent from "@layouts/partials/page-content";
import PageHeader from "@layouts/partials/page-header";
import { useFeature } from "@hooks/use-features";
import FeatureForm from "../../features/features/feature-form";

const FeatureEdit = () => {
  const { id: featureId } = useParams();
  const { isLoading, feature } = useFeature(featureId);
  console.log("object", featureId);

  return (
    <>
      <PageHeader title={"Edit Feature"} />
      <PageContent>
        <Spin spinning={isLoading}>
          <FeatureForm initialValues={feature} isEditMode featureId={featureId} />
        </Spin>
      </PageContent>
    </>
  );
};

export default FeatureEdit;
