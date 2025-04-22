import { useState } from "react";
import { Form, Input, Button, Checkbox, Typography } from "antd";
import { LockOutlined, UnlockOutlined, UserOutlined } from "@ant-design/icons";
import useAuth from "@hooks/use-auth";

const { Link } = Typography;

interface LoginFormProps {
  email: string;
  password: string;
}

const LoginForm = () => {
  const [passwordVisible, setPasswordVisible] = useState(false);
  const { onLogin, isLoading } = useAuth();

  const onFinish = (values: LoginFormProps) => {
    onLogin({
      email: values.email,
      password: values.password,
    });
  };

  return (
    <Form
      layout="vertical"
      initialValues={{
        email: "",
        password: "",
        remember: true,
      }}
      requiredMark={false}
      onFinish={onFinish}
      style={{ width: "100%" }}
    >
      <Form.Item
        name="email"
        label="Email"
        rules={[{ required: true, message: "Email is required" }]}
      >
        <Input prefix={<UserOutlined />} placeholder="Email" />
      </Form.Item>
      <Form.Item
        name="password"
        label="Password"
        rules={[{ required: true, message: "Password is required" }]}
      >
        <Input.Password
          prefix={passwordVisible ? <UnlockOutlined /> : <LockOutlined />}
          type="password"
          placeholder="Password"
          visibilityToggle={{
            visible: passwordVisible,
            onVisibleChange: setPasswordVisible,
          }}
        />
      </Form.Item>
      <Form.Item>
        <div className="flex items-center justify-between">
          <Form.Item name="remember" valuePropName="checked" noStyle>
            <Checkbox>Remember me</Checkbox>
          </Form.Item>

          <Link href={"/forget-password"}>Forgot password</Link>
        </div>
      </Form.Item>
      <Form.Item>
        <Button loading={isLoading} type="primary" htmlType="submit" block>
          Log in
        </Button>
      </Form.Item>
    </Form>
  );
};

export default LoginForm;
