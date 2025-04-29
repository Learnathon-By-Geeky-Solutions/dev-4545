import PageContent from "@layouts/partials/page-content";
import PageHeader from "@layouts/partials/page-header";
import { UserPartial } from "@models/user-model";
import PerformancesForm from "@features/performances/performances-form";

const PerformanceCreate = () => {
  const initialValues: UserPartial = {};

  return (
    <>
      <PageHeader title={"Add Performance"} />
      <PageContent>
        <PerformancesForm initialValues={initialValues} />
      </PageContent>
    </>
  );
};

export default PerformanceCreate;

