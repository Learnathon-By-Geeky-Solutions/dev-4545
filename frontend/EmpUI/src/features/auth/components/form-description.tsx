import { Typography } from 'antd';
import cn from '@utils/cn';

const { Title, Text } = Typography;

interface FormTypeProps {
  formType: 'login' | 'forgotPassword' | 'resetPassword',
  className?: string;
}

const formDetailsMap = {
  login: {
    title: 'Sign In',
    description: (
      <>
        Welcome back to <b>Employee Management System</b>
        <br />
        Please enter your details below to sign in.
      </>
    ),
  },
  forgotPassword: {
    title: 'Forget Password',
    description: (
      <>
        Please provide your username or email. We'll send password reset instructions.
      </>
    ),
  },
  resetPassword: {
    title: 'Set New Password',
    description: (
      <>
        Please enter your new password below.
      </>
    ),
  },
};

const FormDescription = ({
  formType= 'login',
  className = ''
}: FormTypeProps) => {
  const { title, description } = formDetailsMap[formType];
  
  return (
    <div className={cn('text-center mb-8', className)}>
      <Title level={2}>
        {title}
      </Title>
      <Text type={'secondary'} className={'text-center'}>
        {description}
      </Text>
    </div>
  );
};

export default FormDescription;