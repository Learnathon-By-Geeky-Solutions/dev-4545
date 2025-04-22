import { useState, useEffect } from 'react';
import { FormInstance } from 'antd';

interface FormValues {
  [key: string]: any;
}

const useFormValidation = (form: FormInstance, formValues: FormValues) => {
  const [isFormValid, setIsFormValid] = useState(false);
  
  useEffect(() => {
    form.validateFields({ validateOnly: true }).then(
      () => setIsFormValid(true)
    ).catch(
      () => setIsFormValid(false)
    );
  }, [form, formValues]);
  
  return isFormValid;
};

export default useFormValidation;