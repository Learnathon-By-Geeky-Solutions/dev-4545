import { useLoaderData, useParams } from "react-router-dom";
import { Spin } from "antd";
import UserForm from "@features/users/user-form";
import { useUser } from "@hooks/use-users";
import PageContent from "@layouts/partials/page-content";
import PageHeader from "@layouts/partials/page-header";
import { Button, Card, Col, Form, Input, Row, Tag,Empty } from "antd";
import {Typography, Descriptions, Space, Divider, Badge, Tooltip } from 'antd';
import { useAppSelector } from "@/store";
import { usePerformance } from "@hooks/use-performances";
import PerformanceForm from "@features/performances/performance-form";
import { useState } from "react";
import { useEmpTasks } from "@hooks/use-tasks";
import { useEmpFeatures } from "@hooks/use-features";
import { useEmpProjects } from "@hooks/use-projects";

import { 
  CalendarOutlined, 
  UserOutlined, 
  TagOutlined, 
  CheckCircleOutlined,
  ClockCircleOutlined,
  NumberOutlined,
  TeamOutlined,
  ProjectOutlined, 
  IdcardOutlined,
  InfoCircleOutlined,
  AppstoreOutlined,
  EditOutlined,
   DeleteOutlined
} from '@ant-design/icons';




const UserDetails = () => {
  const { id: empId } = useParams();
  const [showPerformanceForm, setShowPerformanceForm] = useState(false);

  const { isLoading, user: employeeDetails } = useUser(empId);
  const { performance } = usePerformance(empId);
  const { tasks } = useEmpTasks(empId);
  const { features } = useEmpFeatures(empId);
  const { projects }= useEmpProjects(empId);

  const handleEditPerformance = () => {
    setShowPerformanceForm(true);
  };

  // Helper function to determine status color
 


  const { Title, Text, Paragraph } = Typography;
  
  const formatDate = (dateString) => {
    const options = { year: 'numeric', month: 'short', day: 'numeric' };
    return new Date(dateString).toLocaleDateString('en-US', options);
  };
  
  // Helper function to check if a project is active
  const isProjectActive = (endDate) => {
    return new Date(endDate) >= new Date();
  };
  
  // Handle delete project
  const handleDelete = (projectId) => {
    
  };

    // Helper functions
    const isOverdue = (dueDate) => {
      return new Date(dueDate) < new Date();
    };
  
    const getStatusColor = (status) => {
      if (!status || status === "string") return "default";
      
      switch (status.toLowerCase()) {
        case "completed": return "green";
        case "in progress": return "blue";
        case "pending": return "orange";
        case "overdue": return "red";
        default: return "default";
      }
    };
  
    const getStatusBadgeStatus = (status) => {
      if (!status || status === "string") return "default";
      
      switch (status.toLowerCase()) {
        case "completed": return "success";
        case "in progress": return "processing";
        case "pending": return "warning";
        case "overdue": return "error";
        default: return "default";
      }
    };
    
      const getFeatureStatus = (startDate, endDate) => {
        const now = new Date();
        const start = new Date(startDate);
        const end = new Date(endDate);
        
        if (now < start) return { text: "Upcoming", color: "blue", status: "processing" };
        if (now > end) return { text: "Completed", color: "green", status: "success" };
        return { text: "In Progress", color: "orange", status: "warning" };
      };
    
      const calculateDuration = (startDate, endDate) => {
        const start = new Date(startDate);
        const end = new Date(endDate);
        const diffTime = Math.abs(end - start);
        const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
        return `${diffDays} days`;
      };
    
  

  return (
    <>
      <PageHeader title={"User Details"} />
      <PageContent>
        <Spin spinning={isLoading}>
          <div>
            {/* user details */}
            <Card title="User Info">
              <Form layout="vertical">
                <Row gutter={24}>
                  <Col span={8}>
                    <Form.Item label="Name">
                      <Input value={employeeDetails?.name} disabled />
                    </Form.Item>
                  </Col>
                  <Col span={8}>
                    <Form.Item label="Email">
                      <Input value={employeeDetails?.email} disabled />
                    </Form.Item>
                  </Col>
                </Row>
                <Row gutter={24}>
                  <Col span={8}>
                    <Form.Item label="Stack">
                      <Tag key="stack" color="geekblue">
                        {employeeDetails?.stack}
                      </Tag>
                    </Form.Item>
                  </Col>
                </Row>
                <Row gutter={24}>
                  <Col span={8}>
                    <Form.Item label="Joining Date">
                      <Tag key="stack" color="geekblue">
                        {employeeDetails?.dateOfJoin}
                      </Tag>
                    </Form.Item>
                  </Col>
                  <Col span={8}>
                    <Form.Item label="Phone">
                      <Tag key="stack">{employeeDetails?.phone}</Tag>
                    </Form.Item>
                  </Col>
                </Row>
              </Form>
            </Card>
            {/* Performance Section */}
            {performance && !showPerformanceForm ? (
              <Card
                title="Performance Review"
                extra={
                  <Button type="primary" onClick={handleEditPerformance}>
                    Edit Performance
                  </Button>
                }
              >
                <Row gutter={24}>
                  <Col span={24}>
                    <Form.Item label="Rating">
                      <div>
                        {Array(5)
                          .fill(0)
                          .map((_, index) => (
                            <span
                              key={index}
                              style={{
                                fontSize: "18px",
                                color:
                                  index < parseInt(performance.rating)
                                    ? "#fadb14"
                                    : "#f0f0f0",
                                marginRight: "8px",
                              }}
                            >
                              ★
                            </span>
                          ))}
                      </div>
                    </Form.Item>
                  </Col>
                </Row>
                <Row gutter={24}>
                  <Col span={24}>
                    <Form.Item label="Comments">
                      <div
                        style={{
                          backgroundColor: "#f5f5f5",
                          padding: 12,
                          borderRadius: 4,
                        }}
                      >
                        {performance.comments}
                      </div>
                    </Form.Item>
                  </Col>
                </Row>
                <Row>
                  <Col span={24}>
                    <Form.Item label="Review Date">
                      <Tag color="blue">
                        {new Date(performance.date).toLocaleDateString()}
                      </Tag>
                    </Form.Item>
                  </Col>
                </Row>
              </Card>
            ) : showPerformanceForm ? (
              <PerformanceForm
                initialValues={performance}
                employeeId={performance.id}
                isEditMode={true}
              />
            ) : null}
            {/* Task section */}
            <Card
      title={
        <Title level={4} style={{ margin: 0 }}>Assigned Tasks</Title>
      }
      style={{ marginTop: 24 }}
      loading={isLoading}
      bordered
    >
      {tasks && tasks.length > 0 ? (
        <div className="task-list">
          {tasks.map((task,index) => (
            <Card
              key={task.taskId}
              type="inner"
              style={{ marginBottom: 16 }}
              title={
                <div
                  style={{
                    display: "flex",
                    justifyContent: "space-between",
                    alignItems: "center",
                  }}
                >
                  <Space>
                    <Badge status={getStatusBadgeStatus(task.status)} />
                    <Text strong>{"Task No:"|| "No Description"}</Text>{index+1} 
                  </Space>
                  <Tag color={getStatusColor(task.status)}>
                    {task.status === "string" ? "Not Set" : task.status}
                  </Tag>
                </div>
              }
            >
              <Row gutter={[16, 16]}>
                {/* First row */}
                <Col xs={24} md={12}>
                  <Space>
                    <IdcardOutlined />
                    <Text strong>Task ID:</Text>
                    <Tooltip title={task.taskId}>
                      <Text copyable={{ text: task.taskId }} style={{ maxWidth: '150px', overflow: 'hidden', textOverflow: 'ellipsis', display: 'inline-block' }}>
                        {task.taskId}
                      </Text>
                    </Tooltip>
                  </Space>
                </Col>
                
                <Col xs={24} md={12}>
                  <Space>
                    <TagOutlined />
                    <Text strong>Description:</Text>
                    <Text>{task.description || "No Description"}</Text>
                  </Space>
                </Col>
                
                {/* Second row */}
                <Col xs={24} md={12}>
                  <Space>
                    <CalendarOutlined />
                    <Text strong>Assigned Date:</Text>
                    <Text>{new Date(task.assignedDate).toLocaleDateString()}</Text>
                  </Space>
                </Col>
                
                <Col xs={24} md={12}>
                  <Space>
                    <CalendarOutlined />
                    <Text strong>Due Date:</Text>
                    <Text
                      type={isOverdue(task.dueDate) ? "danger" : null}
                    >
                      {new Date(task.dueDate).toLocaleDateString()}
                      {isOverdue(task.dueDate) && (
                        <Tag color="red" style={{ marginLeft: 8 }}>Overdue</Tag>
                      )}
                    </Text>
                  </Space>
                </Col>
                
                <Divider style={{ margin: '8px 0' }} />
                
                {/* Third row */}
                <Col xs={24} md={12}>
                  <Space>
                    <TeamOutlined />
                    <Text strong>Assigned By:</Text>
                    <Tooltip title={task.assignedBy}>
                      <Text copyable={{ text: task.assignedBy }} style={{ maxWidth: '150px', overflow: 'hidden', textOverflow: 'ellipsis', display: 'inline-block' }}>
                        {task.assignedBy}
                      </Text>
                    </Tooltip>
                  </Space>
                </Col>
                
                <Col xs={24} md={12}>
                  <Space>
                    <UserOutlined />
                    <Text strong>Employee ID:</Text>
                    <Tooltip title={task.employeeId}>
                      <Text copyable={{ text: task.employeeId }} style={{ maxWidth: '150px', overflow: 'hidden', textOverflow: 'ellipsis', display: 'inline-block' }}>
                        {task.employeeId}
                      </Text>
                    </Tooltip>
                  </Space>
                </Col>
                
                {/* Fourth row */}
                <Col span={24}>
                  <Space>
                    <AppstoreOutlined />
                    <Text strong>Feature ID:</Text>
                    <Tooltip title={task.featureId}>
                      <Text copyable={{ text: task.featureId }} style={{ maxWidth: '250px', overflow: 'hidden', textOverflow: 'ellipsis', display: 'inline-block' }}>
                        {task.featureId}
                      </Text>
                    </Tooltip>
                  </Space>
                </Col>
              </Row>
            </Card>
          ))}
        </div>
      ) : (
        <Empty 
          description={<Text strong>No tasks yet!</Text>} 
          style={{ padding: '40px 0' }}
        />
      )}
    </Card>
{/* Features section */}
<Card
      title={
        <Title level={4} style={{ margin: 0 }}>Project Features</Title>
      }
      style={{ marginTop: 24 }}
      loading={isLoading}
      bordered
    >
      {features && features.length > 0 ? (
        <div className="feature-list">
          {features.map((feature) => {
            const status = getFeatureStatus(feature.startDate, feature.endDate);
            
            return (
              <Card
                key={feature.featureId}
                type="inner"
                style={{ marginBottom: 16 }}
                title={
                  <div
                    style={{
                      display: "flex",
                      justifyContent: "space-between",
                      alignItems: "center",
                    }}
                  >
                    <Space>
                      <Badge status={status.status} />
                      <Text strong>{feature.featureName || "Unnamed Feature"}</Text>
                    </Space>
                    <Tag color={status.color}>
                      {status.text}
                    </Tag>
                  </div>
                }
              >
                {/* Description Section */}
                <div style={{ 
                  backgroundColor: "#f9f9f9", 
                  padding: "12px 16px", 
                  borderRadius: "4px",
                  marginBottom: "16px"
                }}>
                  <Space direction="vertical" style={{ width: "100%" }}>
                    <div>
                      <InfoCircleOutlined style={{ marginRight: 8 }} />
                      <Text strong>Description:</Text>
                    </div>
                    <Text style={{ paddingLeft: 24 }}>
                      {feature.description || "No description provided"}
                    </Text>
                  </Space>
                </div>
                
                <Row gutter={[16, 16]}>
                  {/* First row */}
                  <Col xs={24} md={12}>
                    <Space>
                      <AppstoreOutlined />
                      <Text strong>Feature ID:</Text>
                      <Tooltip title={feature.featureId}>
                        <Text copyable={{ text: feature.featureId }} style={{ maxWidth: '150px', overflow: 'hidden', textOverflow: 'ellipsis', display: 'inline-block' }}>
                          {feature.featureId}
                        </Text>
                      </Tooltip>
                    </Space>
                  </Col>
                  
                  <Col xs={24} md={12}>
                    <Space>
                      <ProjectOutlined />
                      <Text strong>Project ID:</Text>
                      <Tooltip title={feature.projectId}>
                        <Text copyable={{ text: feature.projectId }} style={{ maxWidth: '150px', overflow: 'hidden', textOverflow: 'ellipsis', display: 'inline-block' }}>
                          {feature.projectId}
                        </Text>
                      </Tooltip>
                    </Space>
                  </Col>
                  
                  {/* Second row */}
                  <Col xs={24} md={12}>
                    <Space>
                      <CalendarOutlined />
                      <Text strong>Start Date:</Text>
                      <Tag color="blue">
                        {new Date(feature.startDate).toLocaleDateString()}
                      </Tag>
                    </Space>
                  </Col>
                  
                  <Col xs={24} md={12}>
                    <Space>
                      <CalendarOutlined />
                      <Text strong>End Date:</Text>
                      <Tag color="orange">
                        {new Date(feature.endDate).toLocaleDateString()}
                      </Tag>
                    </Space>
                  </Col>
                  
                  {/* Duration row */}
                  <Col span={24}>
                    <Divider style={{ margin: '8px 0' }} />
                    <Card 
                      size="small" 
                      style={{ backgroundColor: '#f0f5ff', border: '1px solid #d6e4ff' }}
                    >
                      <Space>
                        <ClockCircleOutlined />
                        <Text strong>Duration:</Text>
                        <Text>{calculateDuration(feature.startDate, feature.endDate)}</Text>
                      </Space>
                    </Card>
                  </Col>
                </Row>
              </Card>
            );
          })}
        </div>
      ) : (
        <Empty 
          description={<Text strong>No features yet!</Text>} 
          style={{ padding: '40px 0' }}
        />
      )}
    </Card>
    <Space direction="vertical" size="large" style={{ width: '100%' }}>
      <Title level={2}>Projects</Title>
      
      {projects && projects.length > 0 ? (
        projects.map(project => (
          <Badge.Ribbon 
            key={project.projectId}
            text={isProjectActive(project.endDate) ? "Active" : "Completed"} 
            color={isProjectActive(project.endDate) ? "green" : "blue"}
          >
            <Card
              hoverable
              style={{ width: '100%' }}
              actions={[
                <Tooltip title="Edit Project">
                  <EditOutlined key="edit" />
                </Tooltip>,
                <Tooltip title="Delete Project">
                  <DeleteOutlined key="delete" onClick={() => handleDelete(project.projectId)} />
                </Tooltip>
              ]}
            >
              <Space direction="vertical" size="middle" style={{ width: '100%' }}>
                <div>
                  <Title level={4}>{project.projectName}</Title>
                  <Text type="secondary">{project.projectId}</Text>
                </div>
                
                <Space>
                  <Tag icon={<UserOutlined />} color="blue">
                    {project.client}
                  </Tag>
                  <Tag icon={<CalendarOutlined />} color="green">
                    {calculateDuration(project.startDate, project.endDate)} months
                  </Tag>
                </Space>
                
                <Paragraph>
                  <InfoCircleOutlined /> {project.description}
                </Paragraph>
                
                <Divider style={{ margin: '12px 0' }} />
                
                <Space size="large">
                  <div>
                    <Text type="secondary">Start Date</Text>
                    <div>{formatDate(project.startDate)}</div>
                  </div>
                  <div>
                    <Text type="secondary">End Date</Text>
                    <div>{formatDate(project.endDate)}</div>
                  </div>
                </Space>
              </Space>
            </Card>
          </Badge.Ribbon>
        ))
      ) : (
        <Card>
          <Text>No projects available</Text>
        </Card>
      )}
    </Space>
          </div>
        </Spin>
      </PageContent>
    </>
  );
};


export default UserDetails;
