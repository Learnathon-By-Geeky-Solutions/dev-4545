import React, { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { Menu, MenuProps } from 'antd';
import { MenuItem } from '@models/utils-model';
import { MAIN_MENU_ITEMS } from '@utils/constants/menu-constants';

interface MainMenuProps {
  activeMenu: string;
  onMenuClick: (key: string) => void;
}

const findParentKey = (key: string, menuItems: MenuItem[], parentKey: string = ''): string | null => {
  for (const item of menuItems) {
    if (item.key === key) {
      return parentKey;
    }
    
    if (item.children) {
      const foundParentKey = findParentKey(key, item.children, item.key);
      if (foundParentKey) {
        return foundParentKey;
      }
    }
  }
  
  return null;
};

const findSelectedKey = (activeMenu: string) => {
  return '/' + activeMenu.split('/')[1];
};

const MainMenu: React.FC<MainMenuProps> = ({ activeMenu, onMenuClick }) => {
  const navigate = useNavigate();
  const { pathname } = useLocation();
  
  const [openKeys, setOpenKeys] = useState<string[]>([]);
  
  useEffect(() => {
    const key = findSelectedKey(pathname);
    const newOpenKeys: string[] = [];
    const parentKey = findParentKey(key, MAIN_MENU_ITEMS);
    
    if (parentKey && !newOpenKeys.includes(parentKey)) {
      newOpenKeys.push(parentKey);
    }
    
    setOpenKeys(newOpenKeys);
  }, [pathname]);
  
  const handleClick: MenuProps['onClick'] = (e) => {
    if (pathname === e.key) {
      return;
    }
    
    onMenuClick(e.key);
    navigate(e.key);
  };
  
  const handleOpenChange = (keys: string[]) => {
    setOpenKeys(keys);
  };
  
  const selectedKey = findSelectedKey(activeMenu || location.pathname);
  
  return (
    <Menu
      theme="dark"
      mode="inline"
      selectedKeys={[selectedKey]}
      openKeys={openKeys}
      onOpenChange={handleOpenChange}
      items={MAIN_MENU_ITEMS}
      onClick={handleClick}
    />
  );
};

export default MainMenu;
