import { Link } from 'react-router-dom';
import { MenuProps, TableProps, Tag, Dropdown, Button, Typography } from 'antd';
import { EditOutlined, MoreOutlined } from '@ant-design/icons';
import { User } from '@models/user-model';

const { Text } = Typography;

const getActions = (userId: number): MenuProps['items'] => {
  return [
    {
      key: `edit-${userId}`,
      label: <Link to={`/users/${userId}`}>
        <EditOutlined /> Edit
      </Link>,
    }
  ];
};

const columns : TableProps<User>['columns'] = [
  {
    title: 'Name',
    dataIndex: 'name',
    sorter: true,
    key: 'name',
    render: (_, record) => (
      <Link to={`/users/${record.id}`}>
        {record.name}
        <Text type="secondary" className="flex text-xs">
          {record.id}
        </Text>
      </Link>
    )
  },
  {
    title: 'Email',
    key: 'email',
    render: (_, record) => (
      <Text>{record.email}</Text>
    )
  },
  {
    title: 'Role',
    key: 'role',
    render: (_, record) => (
      <Tag color="geekblue" className="uppercase">{record.role}</Tag>
    )
  },
  {
    title: 'Action',
    key: 'action',
    fixed: 'right',
    align: 'center',
    width: 100,
    render: (_, record) => (
      <Dropdown menu={{items: getActions(record.id!)}} overlayClassName="grid-action">
        <Button shape="circle" icon={<MoreOutlined />} />
      </Dropdown>
    )
  }
];

export { columns };