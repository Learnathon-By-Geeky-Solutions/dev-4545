import { Link } from "react-router-dom";
import {
  MenuProps,
  TableProps,
  Tag,
  Dropdown,
  Button,
  Typography,
  Card,
  Table,
} from "antd";
import { DeleteOutlined, EditOutlined, MoreOutlined } from "@ant-design/icons";
import { User } from "@models/user-model";
import useFilter from "@hooks/utility-hooks/use-filter";
import { columns } from "./task-table-columns";
import { useTasks } from "@hooks/use-tasks";

const { Text } = Typography;

const TableActions: React.FC<Props> = ({ onStatusChange, onDelete }) => {
  const { isLoading, data } = useTasks();

  const getActions = (taskId: number): MenuProps["items"] => [
    // {
    //   key: `set-status-${taskId}`,
    //   label: "Set Status",
    //   children: [
    //     {
    //       key: `set-status-pending-${taskId}`,
    //       label: "Pending",
    //       onClick: () => onStatusChange(taskId, "pending"),
    //     },
    //     {
    //       key: `set-status-in-progress-${taskId}`,
    //       label: "In Progress",
    //       onClick: () => onStatusChange(taskId, "in-progress"),
    //     },
    //     {
    //       key: `set-status-done-${taskId}`,
    //       label: "Done",
    //       onClick: () => onStatusChange(taskId, "done"),
    //     },
    //   ],
    // },
    {
      key: `delete-${taskId}`,
      label: (
        <span>
          <DeleteOutlined /> Delete
        </span>
      ),
      onClick: () => onDelete(taskId),
    },
  ];
  const columns: TableProps<User>["columns"] = [
    {
      title: "Assigned By",
      dataIndex: "assignedBy",
      sorter: true,
      key: "assignedBy",
      render: (_, record) => {
        // console.log("Full Record:", record);
        return (
          <Link to={`/users/details/${record.employeeId}`}>
            {record.assignedBy}
            <Text type="secondary" className="flex text-xs">
              {record.id}
            </Text>
          </Link>
        );
      },
    },
    {
      title: "Assigned Date",
      key: "assignedDate",
      render: (_, record) => <Text>{record?.assignedDate}</Text>,
    },
    {
      title: "Description",
      key: "description",
      render: (_, record) => (
        <Tag color="geekblue" className="uppercase">
          {record?.description}
        </Tag>
      ),
    },
    {
      title: "Employee Id",
      key: "employeeId",
      render: (_, record) => (
        <Tag color="geekblue" className="uppercase">
          {record?.employeeId}
        </Tag>
      ),
    },
    {
      title: "Feature Id",
      key: "featureId",
      render: (_, record) => (
        <Tag color="geekblue" className="uppercase">
          {record?.featureId}
        </Tag>
      ),
    },
    {
      title: "Status",
      key: "status",
      render: (_, record) => (
        <Tag color="geekblue" className="uppercase">
          {record?.status}
        </Tag>
      ),
    },

    {
      title: "Action",
      key: "action",
      fixed: "right" as const,
      align: "center" as const,
      width: 100,
      render: (_: unknown, record: DataType) => (
        <Dropdown
          menu={{ items: getActions(record.taskId) }}
          overlayClassName="grid-action"
          trigger={["click"]}
        >
          <Button shape="circle" icon={<MoreOutlined />} />
        </Dropdown>
      ),
    },
  ];

  return (
    <Card title={"Tasks"}>
      <Table
        columns={columns}
        dataSource={data || []}
        loading={isLoading}
        pagination={false}
        // onChange={sortTableColumn}
        scroll={{ y: 350 }}
        rowKey="employeeId"
        bordered
      />
    </Card>
  );
};

export default TableActions;
