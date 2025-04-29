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
  Tooltip,
} from "antd";
import { DeleteOutlined, EditOutlined, MoreOutlined } from "@ant-design/icons";
import { User } from "@models/user-model";
import useFilter from "@hooks/utility-hooks/use-filter";
import { columns } from "./task-table-columns";
import { useTasks } from "@hooks/use-tasks";

const { Text } = Typography;

const TableActions: React.FC<Props> = ({ onEdit, onDelete }) => {
  const { isLoading, data } = useTasks();

  const getActions = (taskId: number): MenuProps["items"] => [
    {
      key: `edit-${taskId}`,
      label: (
        <Link to={`/tasks/${taskId}`}>
          <EditOutlined /> Edit
        </Link>
      ),
      onClick: () => onEdit(taskId),
    },
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
      title: "Feature Id",
      key: "featureId",
      render: (_, record) => (
        
        <Tooltip title={record?.featureId}>
        <Text
          copyable={{ text: record.featureId }}
          style={{
            maxWidth: "150px",
            overflow: "hidden",
            textOverflow: "ellipsis",
            display: "inline-block",
          }}
        >
          {record.featureId}
        </Text>
      </Tooltip>
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
        rowKey="taskId"
        bordered
      />
    </Card>
  );
};

export default TableActions;
