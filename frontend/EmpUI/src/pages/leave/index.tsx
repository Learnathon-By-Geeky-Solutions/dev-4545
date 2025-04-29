import React from 'react';
import { Card, Button, Typography, Row, Col, Tag, Space, Divider, Empty, Spin, Tooltip } from 'antd';
import {
  CalendarOutlined,
  ClockCircleOutlined,
  FileTextOutlined,
  UserOutlined,
  EditOutlined,
  PlusCircleOutlined,
} from '@ant-design/icons';
import { useEmpLeave } from '@hooks/use-leaves';
import { Link } from 'react-router-dom';
const { Title, Text } = Typography;

const Index = () => {
  const empId = localStorage.getItem('employeeId');
  const { leave: leaveData, loading, error } = useEmpLeave(empId); // Assume useEmpLeave returns loading and error states

  // Function to format date
  const formatDate = (dateString) => {
    const options = { year: 'numeric', month: 'long', day: 'numeric' };
    return new Date(dateString).toLocaleDateString(undefined, options);
  };

  // Function to get status tag color
  const getStatusTag = (status) => {
    const statusMap = {
      0: { color: 'gold', text: 'Pending' },
      1: { color: 'green', text: 'Approved' },
      2: { color: 'red', text: 'Rejected' },
    };
    return statusMap[status] || { color: 'default', text: 'Unknown' };
  };

  // Calculate leave duration
  const calculateDuration = (startDate, endDate) => {
    const start = new Date(startDate);
    const end = new Date(endDate);
    const timeDiff = end - start;
    return Math.floor(timeDiff / (1000 * 60 * 60 * 24)) + 1;
  };

  const handleEdit = (leaveId) => {
    console.log('Edit leave with ID:', leaveId);
    // Add your edit logic here
  };

  const handleApplyLeave = () => {
    console.log('Apply for new leave');
    // Add your apply leave logic here
  };

  // Check if leaveData exists and has the required properties
  const isValidLeaveData = leaveData && leaveData.leaveId && leaveData.startDate && leaveData.endDate;

  // Get status info and leave duration if data is valid
  const statusInfo = isValidLeaveData ? getStatusTag(leaveData.status) : { color: 'default', text: 'Unknown' };
  const leaveDuration = isValidLeaveData ? calculateDuration(leaveData.startDate, leaveData.endDate) : 0;

  return (
    <div style={{ padding: '24px', background: '#f0f2f5', minHeight: '100vh' }}>
      <Row justify="space-between" align="middle" style={{ marginBottom: 24 }}>
        <Col>
          <Title level={2} style={{ margin: 0, color: '#1a1a1a' }}>
            My Leave Application
          </Title>
        </Col>
        <Col>
        <Link to={'/leave/create'}>
          <Button
            type="primary"
            icon={<PlusCircleOutlined />}
            size="large"
            style={{ borderRadius: 8 }}
          >
            Apply for Leave
          </Button>
          
        </Link>
        </Col>
      </Row>

      {loading ? (
        <Spin size="large" style={{ display: 'block', textAlign: 'center', marginTop: 50 }} />
      ) : error ? (
        <Empty
          description="Failed to load leave data. Please try again."
          image={Empty.PRESENTED_IMAGE_SIMPLE}
        >
          <Button type="primary" onClick={handleApplyLeave}>
            Apply for Leave
          </Button>
        </Empty>
      ) : isValidLeaveData ? (
        <Card
          hoverable
          style={{
            width: '100%',
            borderRadius: 12,
            boxShadow: '0 4px 12px rgba(0, 0, 0, 0.1)',
            overflow: 'hidden',
          }}
          title={
            <Row justify="space-between" align="middle">
              <Col>
                <Title level={4} style={{ margin: 0, color: '#1a1a1a' }}>
                  Leave Application
                </Title>
              </Col>
              <Col>
                <Tag
                  color={statusInfo.color}
                  style={{ fontSize: 14, padding: '4px 12px', borderRadius: 12 }}
                >
                  {statusInfo.text}
                </Tag>
              </Col>
            </Row>
          }
          extra={
            <Tooltip title={leaveData.status !== 0 ? 'Cannot edit approved/rejected leaves' : 'Edit leave'}>
              <Link to={'/leave/create'}>
              <Button
                icon={<EditOutlined />}
                disabled={leaveData.status !== 0}
                style={{ borderRadius: 8 }}
              >
                Edit
              </Button>
          
               </Link>
             
              
            </Tooltip>
          }
        >
          <Space direction="vertical" size="large" style={{ width: '100%' }}>
            <Row gutter={[24, 16]}>
              <Col xs={24} sm={12}>
                <Text type="secondary" strong>
                  <CalendarOutlined style={{ marginRight: 8 }} />
                  Start Date
                </Text>
                <div style={{ fontSize: 16, color: '#1a1a1a' }}>
                  {formatDate(leaveData.startDate)}
                </div>
              </Col>
              <Col xs={24} sm={12}>
                <Text type="secondary" strong>
                  <CalendarOutlined style={{ marginRight: 8 }} />
                  End Date
                </Text>
                <div style={{ fontSize: 16, color: '#1a1a1a' }}>
                  {formatDate(leaveData.endDate)}
                </div>
              </Col>
            </Row>

            <Row gutter={[24, 16]}>
              <Col xs={24} sm={12}>
                <Text type="secondary" strong>
                  <ClockCircleOutlined style={{ marginRight: 8 }} />
                  Duration
                </Text>
                <div style={{ fontSize: 16, color: '#1a1a1a' }}>
                  {leaveDuration} day{leaveDuration > 1 ? 's' : ''}
                </div>
              </Col>
              <Col xs={24} sm={12}>
                <Text type="secondary" strong>
                  <FileTextOutlined style={{ marginRight: 8 }} />
                  Leave Type
                </Text>
                <div style={{ fontSize: 16, color: '#1a1a1a', textTransform: 'capitalize' }}>
                  {leaveData.leaveType}
                </div>
              </Col>
            </Row>

            <Divider style={{ margin: '12px 0' }} />

            <div>
              <Text type="secondary" strong>
                <UserOutlined style={{ marginRight: 8 }} />
                Employee ID
              </Text>
              <div style={{ fontSize: 16, color: '#1a1a1a' }}>{leaveData.employeeId}</div>
            </div>

            <div>
              <Text type="secondary" strong>Reason</Text>
              <div style={{ fontSize: 16, color: '#1a1a1a', lineHeight: 1.5 }}>
                {leaveData.reason}
              </div>
            </div>
          </Space>
        </Card>
      ) : (
        <Empty description="No leave application found" image={Empty.PRESENTED_IMAGE_SIMPLE}>
          <Button
            type="primary"
            icon={<PlusCircleOutlined />}
            onClick={handleApplyLeave}
            style={{ borderRadius: 8 }}
          >
            Apply for Leave
          </Button>
        </Empty>
      )}
    </div>
  );
};

export default Index;