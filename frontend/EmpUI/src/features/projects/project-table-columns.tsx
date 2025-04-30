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
import { useProjects } from "@hooks/use-projects";

const { Text } = Typography;

const TableActions: React.FC<Props> = ({ onStatusChange,onEdit, onDelete }) => {
  const { isLoading, data } = useProjects();
  console.log("project data ", data);

  const getActions = (projectId: number): MenuProps["items"] => [
    {
      key: `edit-${projectId}`,
      label: (
        <Link to={`/projects/${projectId}`}>
          <EditOutlined /> Edit
        </Link>
      ),
      onClick: () => onEdit(projectId),
    },
    {
      key: `delete-${projectId}`,
      label: (
        <span>
          <DeleteOutlined /> Delete
        </span>
      ),
      onClick: () => onDelete(projectId),
    },
  ];
  const columns: TableProps<User>["columns"] = [
    {
      title: "Client Name",
      dataIndex: "client",
      sorter: true,
      key: "client",
      render: (_, record) => {
        console.log("Full Record:", record);
        return <Text>{record?.client}</Text>;
      },
    },
    {
      title: "Description",
      key: "description",
      render: (_, record) => {
        return <Text>{record?.description}</Text>;
      },
    },
    {
      title: "Start Date",
      key: "startDate",
      render: (_, record) => (
        <Text >
          {record?.startDate}
        </Text>
      ),
    },
    {
      title: "End Date",
      key: "endDate",
      render: (_, record) => (
        <Text color="geekblue" className="uppercase">
          {record?.endDate}
        </Text>
      ),
    },
    {
      title: "Project Name",
      key: "project",
      render: (_, record) => (
        <Text>
          {record?.projectName}
        </Text>
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
          menu={{ items: getActions(record.projectId) }}
          overlayClassName="grid-action"
          trigger={["click"]}
        >
          <Button shape="circle" icon={<MoreOutlined />} />
        </Dropdown>
      ),
    },
  ];

  return (
    <Card title={"Projects"}>
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
