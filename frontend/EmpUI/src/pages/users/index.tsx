import { Link } from 'react-router-dom';
import { Button } from 'antd';
import { PlusCircleOutlined } from '@ant-design/icons';
import UserTable from '@features/users/user-table';
import PageContent from '@layouts/partials/page-content';
import PageHeader from '@layouts/partials/page-header';

const Users = () => {
  return (
    <>
      <PageHeader
        title="Users"
        subTitle="Enable precise audience targeting using RTG users for effective campaign strategy and enhanced engagement"
      >
        <Link to={'/users/create'}>
          <Button type={'primary'} icon={<PlusCircleOutlined />}>
            Create User
          </Button>
        </Link>
      </PageHeader>
      <PageContent>
        <UserTable />
      </PageContent>
    </>
  );
};

export default Users;
