import { useEffect } from "react";
import { Outlet } from "react-router-dom";
import { Layout } from "antd";

import PageLoader from "@components/shared/page-loader";
import Header from "@layouts/partials/header";
import Sidebar from "@layouts/partials/sidebar";
import { setUser } from "@reducers/user-slice";
import { useUserProfileQuery } from "@services/user-service";
import { useAppDispatch, useAppSelector } from "@/store";

const { Content } = Layout;

const DefaultLayout = () => {
  const dispatch = useAppDispatch();

  const employeeId = localStorage.getItem("employeeId");
  const { isLoading, data, isSuccess } = useUserProfileQuery(employeeId);

  useEffect(() => {
    if (isSuccess && data) {
      dispatch(setUser(data));
      // Store user data in localStorage for future use
      localStorage.setItem("userData", JSON.stringify(data));

      // Log employee details
      // console.log("Employee data loaded:", data, employeeId);
    }
  }, [data, isSuccess, dispatch]);

  if (isLoading && !isSuccess) {
    return <PageLoader />;
  }

  return (
    <div className="h-screen bg-gray-100">
      <Layout>
        <Sidebar />
        <Layout className={"pl-64"}>
          {/* <Header /> */}
          <Content>
            <Outlet />
          </Content>
        </Layout>
      </Layout>
    </div>
  );
};

export default DefaultLayout;
