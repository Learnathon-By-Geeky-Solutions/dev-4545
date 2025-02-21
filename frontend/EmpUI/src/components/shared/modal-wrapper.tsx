import React, { useState } from 'react';
import { Modal } from 'antd';
import { MODAL_SIZES } from '@utils/constants';

interface ModalWrapperProps {
  clickArea: React.ReactNode | string;
  title: string;
  children?: (close: () => void) => React.ReactNode;
  footer?: React.ReactNode | null;
  width?: number;
  centered?: boolean;
}

const ModalWrapper = ({
  clickArea,
  title,
  children,
  footer = null,
  width = MODAL_SIZES.MEDIUM,
  centered = false
}: ModalWrapperProps) => {
  const [open, setOpen] = useState(false);
  
  const handleCancel = () => {
    setOpen(false);
  };
  
  return (
    <>
      <span onClick={() => setOpen(true)}>{clickArea}</span>
      <Modal
        open={open}
        title={title}
        onCancel={handleCancel}
        footer={footer}
        width={width}
        centered={centered}
      >
        {children && children(handleCancel)}
      </Modal>
    </>
  );
};

export default ModalWrapper;
