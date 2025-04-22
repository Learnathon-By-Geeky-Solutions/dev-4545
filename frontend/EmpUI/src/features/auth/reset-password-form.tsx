import { Form, Input, Button } from 'antd';
import { LockOutlined } from '@ant-design/icons';
import useAuth from '@hooks/use-auth';

const ResetPasswordForm = () => {
  const { onLogin, isLoading } = useAuth();

  return (
    <Form name="login"
          layout="vertical"
          initialValues={{
            password: '',
            confirmPassword: ''
          }}
          requiredMark={false}
          onFinish={onLogin}
          style={{ width: '100%' }}
    >
      <Form.Item
        name="newPassword"
        rules={[{ required: true, message: 'New password is required' }]}
      >
        <Input.Password
          prefix={<LockOutlined className="site-form-item-icon" />}
          type="password"
          placeholder="New Password"
        />
      </Form.Item>
      <Form.Item
        name="confirmPassword"
        rules={[{ required: true, message: 'Confirm password is required' }]}
      >
        <Input.Password
          prefix={<LockOutlined className="site-form-item-icon" />}
          type="password"
          placeholder="Confirm New Password"
        />
      </Form.Item>
      <Form.Item>
        <Button loading={isLoading} type="primary" htmlType="submit" block>
          Reset password
        </Button>
      </Form.Item>
    </Form>
  );
};

export default ResetPasswordForm;