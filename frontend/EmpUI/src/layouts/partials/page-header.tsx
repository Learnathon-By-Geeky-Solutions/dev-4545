import React from 'react';
import { Row, Col, Typography } from 'antd';
import Breadcrumbs from '@components/shared/breadcrumbs';

const { Text, Title } = Typography;

const PageHeader = ({
  title,
  subTitle,
  menu,
  children
}: {
  title?: string;
  menu?: React.ReactNode;
  subTitle?: string;
  children?: React.ReactNode;
}) => {
  return (
    <div className="bg-white">
      <Breadcrumbs />
      <div className="px-6 py-6">
        <Row>
          <Col span={12}>
            <Title level={2} style={{ marginBottom: 0 }}>
              {title}
            </Title>
          </Col>
          <Col span={12} className="text-right">
            {children}
          </Col>
        </Row>
        {subTitle && (
          <div className="mt-4">
            <Text type="secondary">
              {subTitle}
            </Text>
          </div>
        )}
      </div>
      {menu && (
        <div>{menu}</div>
      )}
    </div>
  );
};

export default PageHeader;
