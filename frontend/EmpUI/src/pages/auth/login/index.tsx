import FormDescription from '@features/auth/components/form-description';

import LoginForm from '@features/auth/login-form';

const Login = () => {
  return (
    <>
      <FormDescription formType={'login'} />
      <div className="w-[360px]">
        <LoginForm />
      </div>
    </>
  );
};

export default Login;