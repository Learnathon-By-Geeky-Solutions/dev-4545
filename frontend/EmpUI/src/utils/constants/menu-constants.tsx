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
    key: "/",
    label: "Dashboard",
    icon: <DashboardOutlined />,
  },
  {
    key: "/users",
    label: "Users",
    icon: <FolderOpenOutlined />,
  },
  {
    key: "/tasks",
    label: "Tasks",
    icon: <FolderOpenOutlined />,
  },
  {
    key: "/projects",
    label: "Projects",
    icon: <FolderOpenOutlined />,
  },
  {
    key: "/features",
    label: "Features",
    icon: <FolderOpenOutlined />,
  },
  {
    key: "/leaves",
    label: "Leaves",
    icon: <FolderOpenOutlined />,
  },
  {
    key: "/performances",
    label: "Performances",
    icon: <FolderOpenOutlined />,
  },
  {
    key: "/salaries",
    label: "Salaries",
    icon: <FolderOpenOutlined />,
  },
  {
    key: "/details",
    label: "User Details",
    icon: <FolderOpenOutlined />,
  },
  {
    key: "/leave",
    label: "Leave Applications",
    icon: <FolderOpenOutlined />,
  },
];