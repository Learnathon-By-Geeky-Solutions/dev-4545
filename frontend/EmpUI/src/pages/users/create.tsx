import UserForm from '@features/users/user-form';
import PageContent from '@layouts/partials/page-content';
import PageHeader from '@layouts/partials/page-header';
import { UserPartial } from '@models/user-model';

const UserCreate = () => {
  const initialValues: UserPartial = {};
  
  return (
    <>
      <PageHeader
        title={'Users'}
      />
      <PageContent>
        <UserForm initialValues={initialValues} />
      </PageContent>
    </>
  );
};

export default UserCreate;
