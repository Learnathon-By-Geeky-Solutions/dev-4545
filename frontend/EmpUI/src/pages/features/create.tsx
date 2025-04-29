import PageContent from "@layouts/partials/page-content";
import PageHeader from "@layouts/partials/page-header";
import { UserPartial } from "@models/user-model";
import FeatureForm from "@features/features/feature-form";

const FeatureCreate = () => {
  const initialValues: UserPartial = {};

  return (
    <>
      <PageHeader title={"Features Create"} />
      <PageContent>
        <FeatureForm initialValues={initialValues} />
      </PageContent>
    </>
  );
};

export default FeatureCreate;

