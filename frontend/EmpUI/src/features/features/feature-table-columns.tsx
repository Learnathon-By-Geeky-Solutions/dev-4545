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
import { Feature } from "@models/feature-model";
import useFilter from "@hooks/utility-hooks/use-filter";
import { columns } from "./feature-table-columns";
import { useFeatures } from "@hooks/use-features";

const { Text } = Typography;

interface Props {
  onStatusChange?: (featureId: number, status: string) => void;
  onDelete: (featureId: number) => void;
}

const TableActions: React.FC<Props> = ({ onStatusChange, onDelete }) => {
  const { isLoading, data } = useFeatures();

  const getActions = (featureId: number): MenuProps["items"] => [
    {
      key: `delete-${featureId}`,
      label: (
        <span>
          <DeleteOutlined /> Delete
        </span>
      ),
      onClick: () => onDelete(featureId),
    },
  ];

  const columns: TableProps<Feature>["columns"] = [
    {
      title: "Feature Name",
      dataIndex: "featureName",
      sorter: true,
      key: "featureName",
      render: (_, record) => {
        return (
          <Link to={`/users/details/${record.featureId}`}>
            {record.featureName}
            <Text type="secondary" className="flex text-xs">
              {record.id}
            </Text>
          </Link>
        );
      },
    },
    {
      title: "Description",
      key: "description",
      render: (_, record) => <Text>{record?.description}</Text>,
    },
    {
      title: "Start Date",
      key: "startDate",
      render: (_, record) => (
        <Tag color="geekblue" className="uppercase">
          {record?.startDate}
        </Tag>
      ),
    },
    {
      title: "End Date",
      key: "endDate",
      render: (_, record) => (
        <Tag color="geekblue" className="uppercase">
          {record?.endDate}
        </Tag>
      ),
    },
    {
      title: "Action",
      key: "action",
      fixed: "right" as const,
      align: "center" as const,
      width: 100,
      render: (_: unknown, record: Feature) => (
        <Dropdown
          menu={{ items: getActions(record.featureId) }}
          overlayClassName="grid-action"
          trigger={["click"]}
        >
          <Button shape="circle" icon={<MoreOutlined />} />
        </Dropdown>
      ),
    },
  ];

  return (
    <Card title={"Features"}>
      <Table
        columns={columns}
        dataSource={data || []}
        loading={isLoading}
        pagination={false}
        // onChange={sortTableColumn}
        scroll={{ y: 350 }}
        rowKey="featureId"
        bordered
      />
    </Card>
  );
};

export default TableActions;
