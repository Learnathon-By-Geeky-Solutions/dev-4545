import { Card, Col, Form, Input, Row, Tag } from 'antd';
import { useAppSelector } from '@/store';

const UserDetails = () => {
  const user = useAppSelector((state) => state.user);
  
  return (
    <Card title="Information">
      <Form layout="vertical">
        <Row gutter={24}>
          <Col span={8}>
            <Form.Item label="Name">
              <Input value={user.name} disabled />
            </Form.Item>
          </Col>
          <Col span={8}>
            <Form.Item label="Email">
              <Input value={user.email} disabled />
            </Form.Item>
          </Col>
        </Row>
        <Row gutter={24}>
          <Col span={8}>
            <Form.Item label="Status">
              <Tag key='status' color="geekblue">{user.role}</Tag>
            </Form.Item>
          </Col>
        </Row>
      </Form>
    </Card>
  );
};

export default UserDetails;
