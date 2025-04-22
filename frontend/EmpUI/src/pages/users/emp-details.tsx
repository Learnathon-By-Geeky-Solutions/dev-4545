import { Spin } from "antd";
import { useUser } from "@hooks/use-users";
import PageContent from "@layouts/partials/page-content";
import PageHeader from "@layouts/partials/page-header";
import { Button, Card, Col, Form, Input, Row, Tag,Empty } from "antd";
import {Typography, Descriptions, Space, Divider, Badge, Tooltip } from 'antd';
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


const EmployeeDetails = () => {
  const empId = localStorage.getItem("employeeId");
  const { isLoading, user: employeeDetails } = useUser(empId);
  const [showPerformanceForm, setShowPerformanceForm] = useState(false);
  const { performance } = usePerformance(empId);
  // const { tasks } = useEmpTasks(empId);
  // const { features } = useEmpFeatures(empId);
  // const { projects }= useEmpProjects(empId);

  const handleEditPerformance = () => {
    setShowPerformanceForm(true);
  };
  
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
      
          </div>
        </Spin>
      </PageContent>
    </>
  );
};

export default EmployeeDetails;
