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
import { columns } from "./salary-table-columns";
import { useSalaries } from "@hooks/use-salaries";

const { Text } = Typography;

const SalaryActions: React.FC<Props> = ({ onStatusChange, onDelete }) => {
  const { isLoading, data } = useSalaries();
  console.log("Salary data ", data);

  const getActions = (salaryId: number): MenuProps["items"] => [
    {
      key: `delete-${salaryId}`,
      label: (
        <span>
          <DeleteOutlined /> Delete
        </span>
      ),
      onClick: () => onDelete(salaryId),
    },
  ];
  const columns: TableProps<User>["columns"] = [
    {
      title: "Salary Id",
      key: "salaryId",
      render: (_, record) => (
        
        <Tooltip title={record?.salaryId}>
        <Text
          copyable={{ text: record?.salaryId }}
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
      title: "Salary Date",
      key: "salaryDate",
      render: (_, record) => (
        <Tag color="geekblue" className="uppercase">
          {record?.salaryDate}
        </Tag>
      ),
    },
    {
      title: "Amount",
      key: "amount",
      render: (_, record) => (
        <Tag color="geekblue" className="uppercase">
          {record?.amount}
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
      title: "Action",
      key: "action",
      fixed: "right" as const,
      align: "center" as const,
      width: 100,
      render: (_: unknown, record: DataType) => (
        <Dropdown
          menu={{ items: getActions(record.salaryId) }}
          overlayClassName="grid-action"
          trigger={["click"]}
        >
          <Button shape="circle" icon={<MoreOutlined />} />
        </Dropdown>
      ),
    },
  ];

  return (
    <Card title={"Salaries"}>
      <Table
        columns={columns}
        dataSource={data || []}
        loading={isLoading}
        pagination={false}
        // onChange={sortTableColumn}
        scroll={{ y: 350 }}
        rowKey="Id"
        bordered
      />
    </Card>
  );
};

export default SalaryActions;
