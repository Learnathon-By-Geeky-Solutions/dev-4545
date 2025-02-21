import FormDescription from '@features/auth/components/form-description';

import ForgetPasswordForm from '@features/auth/forget-password-form';

const ForgetPassword = () => {
  return (
    <>
      <FormDescription formType={'forgotPassword'} />
      <div className="w-[360px]">
        <ForgetPasswordForm />
      </div>
    </>
  );
};

export default ForgetPassword;