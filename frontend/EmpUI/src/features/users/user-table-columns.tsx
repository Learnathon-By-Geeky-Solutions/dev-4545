import { Link } from "react-router-dom";
import { MenuProps, TableProps, Tag, Dropdown, Button, Typography, Tooltip } from "antd";
import { DeleteOutlined, EditOutlined, MoreOutlined } from "@ant-design/icons";
import { User } from "@models/user-model";

const { Text } = Typography;

const getActions = (userId: number): MenuProps["items"] => {
  return [
    {
      key: `edit-${userId}`,
      label: (
        <Link to={`/users/${userId}`}>
          <EditOutlined /> Edit
        </Link>
      ),
    },
  ];
};

const columns: TableProps<User>["columns"] = [
  {
    title: "Name",
    dataIndex: "name",
    sorter: true,
    key: "name",
    render: (_, record) => {
      // console.log("Full Record:", record);
      return (
        <Link to={`/users/details/${record.employeeId}`}>
          {record.name}
          <Text type="secondary" className="flex text-xs">
            {record.id}
          </Text>
        </Link>
      );
    },
  },
  {
    title: "Email",
    key: "email",
    render: (_, record) => <Text>{record.email}</Text>,
  },
  {
    title: "Stack",
    key: "stack",
    render: (_, record) => (
      <Tag color="geekblue" className="uppercase">
        {record.stack}
      </Tag>
    ),
  },
  {
    title: "Employee Id",
    key: "employeeId",
    render: (_, record) => (
      <Tooltip title={record.employeeId}>
        <Text
          copyable={{ text: record.employeeId }}
          style={{
            maxWidth: "150px",
            overflow: "hidden",
            textOverflow: "ellipsis",
            display: "inline-block",
          }}
        >
          {record.employeeId}
        </Text>
      </Tooltip>
    ),
  },
  {
    title: "Role",
    key: "role",
    render: (_, record) => (
      <Tag color="geekblue" className="uppercase">
        {/* {localStorage.getItem("role")} */}
        {record.role == 0 ? "Admin" : "Employee"}
      </Tag>
    ),
  },
  {
    title: "Action",
    key: "action",
    fixed: "right",
    align: "center",
    width: 100,
    render: (_, record) => (
      <Dropdown
        menu={{ items: getActions(record.employeeId!) }}
        overlayClassName="grid-action"
      >
        <Button shape="circle" icon={<MoreOutlined />} />
      </Dropdown>
    ),
  },
];

export { columns };
