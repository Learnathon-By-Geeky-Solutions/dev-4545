import FormDescription from '@features/auth/components/form-description';
import ResetPasswordForm from '@features/auth/reset-password-form';

const ResetPassword = () => {
  return (
    <>
      <FormDescription formType={'resetPassword'} />
      <div className="w-[360px]">
        <ResetPasswordForm />
      </div>
    </>
  );
};

export default ResetPassword;