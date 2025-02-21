import { Form, Input, Button, Typography } from 'antd';
import { UserOutlined } from '@ant-design/icons';
import useAuth from '@hooks/use-auth';

const { Text, Link } = Typography;

const ForgetPasswordForm = () => {
  const { onLogin, isLoading } = useAuth();

  return (
    <Form layout="vertical"
          initialValues={{
            username: 'imrul.hasan@vivacomsolutions.com'
          }}
          requiredMark={false}
          onFinish={onLogin}
          style={{ width: '100%' }}
    >
      <Form.Item
        name="username"
        label="Email"
        rules={[{ required: true, message: 'Email is required' }]}
      >
        <Input prefix={<UserOutlined className="site-form-item-icon" />} placeholder="Email" />
      </Form.Item>

      <Form.Item>
        <Button loading={isLoading} type="primary" htmlType="submit" block>
          Submit
        </Button>
      </Form.Item>
      
      <div className={'text-center'}>
        <Text type={'secondary'}>
          I changed my mind
          <Link href={'/login'} className={'pl-1'}>
            Return to login
          </Link>
        </Text>
      </div>
    </Form>
  );
};

export default ForgetPasswordForm;