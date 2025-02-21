import { Space } from 'antd';
import UserDetails from '@features/settings/user-details';
import PageContent from '@layouts/partials/page-content';
import PageHeader from '@layouts/partials/page-header';

const Settings = () => {
  return (
    <>
      <PageHeader
        title="Settings"
        subTitle="Access and adjust your preferences conveniently on our settings page"
      />
      <PageContent>
        <Space direction="vertical" size="large" style={{ display: 'flex' }}>
          <UserDetails />
        </Space>
      </PageContent>
    </>
  );
};

export default Settings;
