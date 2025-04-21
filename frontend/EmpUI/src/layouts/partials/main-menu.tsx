import React, { useEffect, useState } from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import { Menu, MenuProps } from 'antd';
import { MenuItem } from '@models/utils-model';
import { MAIN_MENU_ITEMS } from '@utils/constants/menu-constants';
import { getAuthorizedRoutes } from '../../routes/route-utils';

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
  console.log("ActiveMenu: " + activeMenu);
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

  const handleClick: MenuProps["onClick"] = (e) => {
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

  // Get authorized routes based on user role
  const authorizedRoutes = getAuthorizedRoutes();

  // Filter menu items to only show authorized ones
  const authorizedMenuItems = MAIN_MENU_ITEMS.filter((menuItem) => {
    // Check if the path of this menu item exists in authorized routes
    return authorizedRoutes.some(
      (route) => route.path === menuItem.key.replace("/", "")
    );
  });

  return (
    <Menu
      theme="dark"
      mode="inline"
      selectedKeys={[activeMenu]}
      onClick={({ key }) => onMenuClick(key)}
    >
      {authorizedMenuItems.map((menuItem) => (
        <Menu.Item key={menuItem.key} icon={menuItem.icon}>
          <Link to={menuItem.key}>{menuItem.label}</Link>
        </Menu.Item>
      ))}
    </Menu>
  );
};

export default MainMenu;
