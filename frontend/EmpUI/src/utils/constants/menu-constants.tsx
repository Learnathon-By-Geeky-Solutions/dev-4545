import {
  DashboardOutlined,
  FolderOpenOutlined,
  LogoutOutlined,
  SettingOutlined
} from '@ant-design/icons';

export const PROFILE_MENU_ITEMS = [
  {
    key: '/settings',
    icon: <SettingOutlined />,
    label: 'Settings',
  },
  {
    key: 'logout',
    icon: <LogoutOutlined />,
    label: 'Logout',
  }
];

export const MAIN_MENU_ITEMS = [
  {
    key: '/',
    label: 'Dashboard',
    icon: <DashboardOutlined />
  },
  {
    key: '/users',
    label: 'Users',
    icon: <FolderOpenOutlined />
  }
];