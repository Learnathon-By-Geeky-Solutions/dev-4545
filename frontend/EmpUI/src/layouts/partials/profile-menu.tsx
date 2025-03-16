import { useNavigate } from "react-router-dom";
import { Avatar, Menu, MenuProps } from "antd";
import { UserOutlined } from "@ant-design/icons";

import { User } from "@models/user-model";
import { logOut } from "@reducers/auth-slice";
import { unsetUser } from "@reducers/user-slice";
import { PROFILE_MENU_ITEMS } from "@utils/constants/menu-constants";
import { useAppDispatch, useAppSelector } from "@/store";

const getMenuItems = (user: User | undefined) => {
  return [
    {
      key: "user-name",
      icon: <Avatar shape="square" icon={<UserOutlined />} />,
      label: user ? `${user.name} (${user.role})` : "",
      children: PROFILE_MENU_ITEMS,
    },
  ];
};

const ProfileMenu = ({
  activeMenu,
  onMenuClick,
}: {
  activeMenu: string;
  onMenuClick: (key: string) => void;
}) => {
  const navigate = useNavigate();
  const user = useAppSelector((state) => state.user);
  const dispatch = useAppDispatch();

  const handleClick: MenuProps["onClick"] = (e) => {
    if (e.key === "logout") {
      localStorage.removeItem("auth");
      localStorage.removeItem("currentUserId");
      dispatch(logOut());
      dispatch(unsetUser());

      return;
    }

    onMenuClick(e.key);
    navigate(e.key);
  };

  const menuItems = getMenuItems(user);

  return (
    <Menu
      theme="dark"
      className={"absolute w-[256px] bottom-0"}
      items={menuItems}
      onClick={handleClick}
      selectedKeys={[activeMenu]}
    />
  );
};

export default ProfileMenu;
