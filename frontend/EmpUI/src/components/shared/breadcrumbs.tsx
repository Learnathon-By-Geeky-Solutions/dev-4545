import { Link, useLocation } from 'react-router-dom';
import useBreadcrumbs from 'use-react-router-breadcrumbs';
import { Row, Col, Breadcrumb } from 'antd';
import routes from '@/routes/routes';

const Breadcrumbs = () => {
  const location = useLocation();
  const breadcrumbs = useBreadcrumbs(routes);
  
  const breadcrumbItems = breadcrumbs.map(({ match, breadcrumb }) => ({
    key: match.pathname,
    title: match.pathname !== location.pathname ? (
      <Link to={match.pathname}>{breadcrumb}</Link>
    ) : (
      <div>{breadcrumb}</div>
    ),
  }));
  
  return breadcrumbItems && location.pathname !== '/' && (
    <div className="px-6 pt-3">
      <Row>
        <Col span={24}>
          <Breadcrumb items={breadcrumbItems} />
        </Col>
      </Row>
    </div>
  );
};

export default Breadcrumbs;
