import { useEffect } from 'react';
import { Outlet } from 'react-router-dom';
import { Layout } from 'antd';

import PageLoader from '@components/shared/page-loader';
import Header from '@layouts/partials/header';
import Sidebar from '@layouts/partials/sidebar';
import { setUser } from '@reducers/user-slice';
import { useUserProfileQuery } from '@services/user-service';
import { useAppDispatch } from '@/store';

const { Content } = Layout;

const DefaultLayout = () => {
  const dispatch = useAppDispatch();
  const { isLoading, data, isSuccess } = useUserProfileQuery();
  
  useEffect(() => {
    if (isSuccess && data) {
      dispatch(setUser(data));
    }
  }, [data, isSuccess]);
  
  if (isLoading && !isSuccess) {
    return <PageLoader />;
  }
  
  return (
    <div className="h-screen bg-gray-100">
      <Layout>
        <Sidebar />
        <Layout className={'pl-64'}>
          <Header />
          <Content>
            <Outlet />
          </Content>
        </Layout>
      </Layout>
    </div>
  );
};

export default DefaultLayout;
