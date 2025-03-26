import { FC, PropsWithChildren } from 'react';
import { Typography } from 'antd';

const HelpText: FC<PropsWithChildren> = ({ children }) => {
  return (
    <Typography.Text type="secondary" className="text-sm">{children}</Typography.Text>
  );
};

export default HelpText;