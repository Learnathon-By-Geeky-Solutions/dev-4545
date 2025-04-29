import { useParams } from "react-router-dom";
import { Spin } from "antd";
import UserForm from "@features/users/user-form";
import { useUser } from "@hooks/use-users";
import PageContent from "@layouts/partials/page-content";
import PageHeader from "@layouts/partials/page-header";

const UserEdit = () => {
  const { id: userId } = useParams();
  const { isLoading, user } = useUser(userId);

  return (
    <>
      <PageHeader title={"User edit"} />
      <PageContent>
        <Spin spinning={isLoading}>
          <UserForm initialValues={user} isEditMode userId={userId} />
        </Spin>
      </PageContent>
    </>
  );
};

export default UserEdit;
