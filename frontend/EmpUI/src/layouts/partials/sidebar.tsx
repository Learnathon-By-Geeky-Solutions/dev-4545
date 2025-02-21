import { useEffect, useState } from 'react';
import { Link, useLocation } from 'react-router-dom';
import { Image, Layout } from 'antd';
import Logo from '@assets/logo.png';
import MainMenu from '@layouts/partials/main-menu';
import ProfileMenu from '@layouts/partials/profile-menu';

const { Sider } = Layout;

const Sidebar = () => {
  const location = useLocation();
  
  const [activeMainMenu, setActiveMainMenu] = useState<string>(location.pathname);
  const [activeProfileMenu, setActiveProfileMenu] = useState<string>(location.pathname);
  
  const handleMainMenuClick = (key: string) => {
    setActiveMainMenu(key);
    setActiveProfileMenu('');
  };
  
  const handleProfileMenuClick = (key: string) => {
    setActiveProfileMenu(key);
    setActiveMainMenu('');
  };
  
  useEffect(() => {
    if (location.pathname === '/') {
      setActiveMainMenu('/');
    }
  }, [location]);
  
  return (
    <Sider>
      <div className="p-4">
        <Link to="/">
          <Image src={Logo} preview={false} height={40} />
        </Link>
      </div>
      
      <MainMenu activeMenu={activeMainMenu} onMenuClick={handleMainMenuClick} />
      <ProfileMenu activeMenu={activeProfileMenu} onMenuClick={handleProfileMenuClick} />
    </Sider>
  );
};

export default Sidebar;
