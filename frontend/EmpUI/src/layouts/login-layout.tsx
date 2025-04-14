import { Outlet } from 'react-router-dom';

import { Image } from 'antd';

import IconMarketplace from '@assets/icon.png';
import LoginBanner from '@assets/login-banner.jpeg';

const LoginLayout = () => {
  return (
    <div className="flex flex-col md:flex-row min-h-screen">
      <div className="flex w-full md:w-1/2 items-center justify-center p-4 md:p-0">
        <div className="flex flex-col max-w-[320px] items-center">
          <Image
            src={IconMarketplace}
            preview={false}
            width={35}
            className="mb-8"
          />
          <Outlet/>
        </div>
      </div>
      <div
        className="hidden md:block w-full md:w-1/2 bg-cover"
        style={{backgroundImage: `url(${LoginBanner})`}}
      />
    </div>
  );
};

export default LoginLayout;