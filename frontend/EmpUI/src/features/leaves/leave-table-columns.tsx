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
import { Leave } from "@models/leave-model";
import useFilter from "@hooks/utility-hooks/use-filter";
import { columns } from "./leave-table-columns";
import { useLeaves } from "@hooks/use-leaves";

const { Text } = Typography;

interface Props {
  onStatusChange?: (leaveId: number, status: string) => void;
  onDelete: (leaveId: number) => void;
}

const TableActions: React.FC<Props> = ({ onStatusChange, onDelete }) => {
  const { isLoading, data } = useLeaves();

  const getActions = (leaveId: number): MenuProps["items"] => [
    {
      key: `delete-${leaveId}`,
      label: (
        <span>
          <DeleteOutlined /> Delete
        </span>
      ),
      onClick: () => onDelete(leaveId),
    },
  ];

  const columns: TableProps<Leave>["columns"] = [
    {
      title: "Employee",
      dataIndex: "employeeName",
      sorter: true,
      key: "employeeName",
      render: (_, record) => {
          return (
            <Link to={`/users/details/${record.employeeId}`}>
              <Text>{record?.employeeId}</Text>;
            </Link>
          );

        // return (
        //   <Tooltip title={record.employeeId}>
        //     <Text
        //       copyable={{ text: record.employeeId }}
        //       style={{
        //         maxWidth: "150px",
        //         overflow: "hidden",
        //         textOverflow: "ellipsis",
        //         display: "inline-block",
        //       }}
        //     >
        //       {record.employeeId}
        //     </Text>
        //   </Tooltip>
        // );

      },
    },
    {
      title: "Leave Type",
      key: "leaveType",
      render: (_, record) => (
        <Tag color="geekblue" className="uppercase">
          {record?.leaveType}
        </Tag>
      ),
    },
    {
      title: "Reason",
      key: "reason",
      render: (_, record) => <Text>{record?.reason}</Text>,
    },
    {
      title: "Start Date",
      key: "startDate",
      render: (_, record) => <Text>{record?.startDate}</Text>,
    },
    {
      title: "End Date",
      key: "endDate",
      render: (_, record) => <Text>{record?.endDate}</Text>,
    },
    {
      title: "Status",
      key: "status",
      render: (_, record) => {
        let color = "default";
        if (record?.status === 1) {
          color = "success";
        } else if (record?.status === 0) {
          color = "error";
        }

        return (
          <Tag color={color} className="uppercase">
            {record?.status==0?"Pending":"Accepted"}
          </Tag>
        );
      },
    },
    {
      title: "Action",
      key: "action",
      fixed: "right" as const,
      align: "center" as const,
      width: 100,
      render: (_: unknown, record: Leave) => (
        <Dropdown
          menu={{ items: getActions(record.leaveId) }}
          overlayClassName="grid-action"
          trigger={["click"]}
        >
          <Button shape="circle" icon={<MoreOutlined />} />
        </Dropdown>
      ),
    },
  ];

  return (
    <Card title={"Leaves"}>
      <Table
        columns={columns}
        dataSource={data || []}
        loading={isLoading}
        pagination={false}
        // onChange={sortTableColumn}
        scroll={{ y: 350 }}
        rowKey="leaveId"
        bordered
      />
    </Card>
  );
};

export default TableActions;
