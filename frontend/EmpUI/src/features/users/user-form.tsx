import _ from 'lodash';
import { useEffect } from 'react';
import { Button, Card, Col, Form, Input, Row, Select } from 'antd';
import { SaveOutlined } from '@ant-design/icons';
import { useUserForm } from '@hooks/use-users';
import { User, UserPartial } from '@models/user-model';
import { validationMessage } from '@utils/helpers/message-helpers';

interface UserFormProps {
  initialValues?: UserPartial;
  isEditMode?: boolean;
}

const USER_ROLES = [
  {
    label: 'Admin',
    value: 'admin'
  },
  {
    label: 'CEO',
    value: 'ceo'
  },
  {
    label: 'SE',
    value: 'se'
  },
  {
    label: 'HR',
    value: 'hr'
  }
];

const UserForm = ({ initialValues, isEditMode = false }: UserFormProps) => {
  const [form] = Form.useForm();
  
  const { onSaved, isLoading } = useUserForm();
  
  useEffect(() => {
    if (initialValues) {
      form.setFieldsValue({
        ...initialValues,
        password: '',
        confirm_password: ''
      });
    }
  }, [initialValues, form, isEditMode]);
  
  const onFinished = (values: User) => {
    values.id = isEditMode ? initialValues?.id ?? 0 : 0;
    
    const userData = _.omit(values, 'confirm_password');
    onSaved(userData);
  };
  
  return (
    <Form
      form={form}
      layout="vertical"
      autoComplete={'off'}
      initialValues={initialValues}
      onFinish={onFinished}>
      <Card title="Update User">
        <Row gutter={24}>
          <Col span={12}>
            <Form.Item
              label="Name"
              name="name"
              rules={[{ required: true, message: validationMessage('name') }]}
            >
              <Input placeholder="Name" />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item
              label="Email"
              name="email"
              rules={[
                { required: true, message: validationMessage('email') },
                { type: 'email', message: validationMessage('email', 'email') }
              ]}
            >
              <Input placeholder="Email" />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item
              label="Password"
              name="password"
              rules={[
                { required: !isEditMode, message: validationMessage('password') },
                { min: 4, message: 'Password must be at least 4 characters' },
              ]}
            >
              <Input.Password placeholder="Password" />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item
              label="Re-enter Password"
              name="confirm_password"
              dependencies={['password']}
              rules={[
                { required: !isEditMode, message: validationMessage('confirm password') },
                ({ getFieldValue }) => ({
                  validator(_, value) {
                    if (!value || getFieldValue('password') === value) {
                      return Promise.resolve();
                    }
                    return Promise.reject(new Error('The two passwords do not match'));
                  },
                }),
              ]}
            >
              <Input.Password placeholder="Re-enter Password" />
            </Form.Item>
          </Col>
          <Col span={12}>
            <Form.Item
              label="Role"
              name="role"
              rules={[{ required: true, message: validationMessage('role') }]}>
              <Select options={USER_ROLES} placeholder="Select role" />
            </Form.Item>
          </Col>
        </Row>
      </Card>
      <Row className="my-6">
        <Col span={24} className="text-right">
          <Button
            type="primary"
            htmlType="submit"
            icon={<SaveOutlined />}
            loading={isLoading}>
            Save changes
          </Button>
        </Col>
      </Row>
    </Form>
  );
};

export default UserForm;
