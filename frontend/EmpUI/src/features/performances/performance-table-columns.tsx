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
import { DeleteOutlined, MoreOutlined } from "@ant-design/icons";
import { User } from "@models/user-model";
import { usePerformances } from "@hooks/use-performances";
import { useDeletePerformanceMutation } from "@services/performance-service";

const { Text } = Typography;

const TableActions: React.FC<Props> = ({ onStatusChange, onDelete }) => {
  const { isLoading, data } = usePerformances();
  const [deletePerformance] = useDeletePerformanceMutation();

  const delPerformance = async (performanceId: string) => {
    console.log("Deleting:", performanceId);
    try {
      await deletePerformance(performanceId).unwrap();
      console.log("Deleted:", performanceId);
      // Refresh the task list
    } catch (error) {
      console.log(error);
    }
  };

  const getActions = (performanceId: string): MenuProps["items"] => [
    {
      key: `delete-${performanceId}`,
      label: (
        <span>
          <DeleteOutlined /> Delete
        </span>
      ),
      onClick: () => delPerformance(performanceId),
    },
  ];
  const columns: TableProps<User>["columns"] = [
    {
      title: "Reviewed By",
      dataIndex: "reviewedBy",
      sorter: true,
      key: "reviewedBy",
      render: (_, record) => {
        console.log("Full Record:", record);
        return <Text>{record?.reviewerId}</Text>;
      },
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
      title: "Date",
      key: "date",
      render: (_, record) => <Text>{record?.date}</Text>,
    },
    {
      title: "Comments",
      key: "comments",
      render: (_, record) => (
        <Tag color="geekblue" className="uppercase">
          {record?.comments}
        </Tag>
      ),
    },
    {
      title: "Rating",
      key: "rating",
      render: (_, record) => (
        <Tag color="geekblue" className="uppercase">
          {record?.rating}
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
          menu={{ items: getActions(record.id) }}
          overlayClassName="grid-action"
          trigger={["click"]}
        >
          <Button shape="circle" icon={<MoreOutlined />} />
        </Dropdown>
      ),
    },
  ];

  return (
    <Card title={"Performances"}>
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
