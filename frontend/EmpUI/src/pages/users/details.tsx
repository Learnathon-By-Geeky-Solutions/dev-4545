import { useLoaderData, useParams } from "react-router-dom";
import { Spin } from "antd";
import UserForm from "@features/users/user-form";
import { useUser } from "@hooks/use-users";
import PageContent from "@layouts/partials/page-content";
import PageHeader from "@layouts/partials/page-header";
import { Button, Card, Col, Form, Input, Row, Tag } from "antd";
import { useAppSelector } from "@/store";
import { usePerformance } from "@hooks/use-performances";
import PerformanceForm from "@features/performances/performance-form";
import { useState } from "react";
import { useEmpTasks } from "@hooks/use-tasks";

const UserDetails = () => {
  const { id: empId } = useParams();
  const [showPerformanceForm, setShowPerformanceForm] = useState(false);

  const { isLoading, user: employeeDetails } = useUser(empId);
  const { performance } = usePerformance(empId);
  const { tasks } = useEmpTasks(empId);

  const handleEditPerformance = () => {
    setShowPerformanceForm(true);
  };

  // Helper function to determine status color
  const getStatusColor = (status) => {
    switch (status.toLowerCase()) {
      case "completed":
        return "green";
      case "in progress":
        return "blue";
      case "pending":
        return "orange";
      case "delayed":
        return "red";
      default:
        return "default";
    }
  };

  // Helper function to check if task is overdue
  const isOverdue = (dueDate) => {
    const today = new Date();
    const taskDueDate = new Date(dueDate);
    return taskDueDate < today;
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
    <div className="card-title" style={{ fontSize: "18px", fontWeight: "600", color: "#1890ff" }}>
      Assigned Tasks
    </div>
  }
  style={{ 
    marginTop: "24px",
    boxShadow: "0 4px 12px rgba(0, 0, 0, 0.08)",
    borderRadius: "8px"
  }}
  loading={isLoading}
  headStyle={{ backgroundColor: "#f9fafc", borderBottom: "1px solid #f0f0f0" }}
  bodyStyle={{ padding: "0" }}
>
  {tasks && tasks.length > 0 ? (
    <div className="task-list">
      {tasks.map((task) => (
        <Card
          key={task.taskId}
          type="inner"
          style={{ 
            marginBottom: "16px", 
            borderRadius: "6px",
            border: "1px solid #f0f0f0",
            overflow: "hidden"
          }}
          title={
            <div
              style={{
                display: "flex",
                justifyContent: "space-between",
                alignItems: "center"
              }}
            >
              <span style={{ fontWeight: "500", fontSize: "16px" }}>{task.description}</span>
              <Tag 
                color={getStatusColor(task.status)}
                style={{ 
                  borderRadius: "4px", 
                  padding: "4px 12px", 
                  fontWeight: "500" 
                }}
              >
                {task.status}
              </Tag>
            </div>
          }
          headStyle={{ backgroundColor: "#fafafa" }}
          bodyStyle={{ padding: "16px" }}
        >
          <Row gutter={[16, 20]}>
            <Col xs={24} sm={12}>
              <div style={{ display: "flex", alignItems: "center" }}>
                <span style={{ fontWeight: "500", marginRight: "8px", color: "#595959" }}>Task ID:</span>
                <span style={{ color: "#8c8c8c", fontSize: "14px" }}>{task.taskId}</span>
              </div>
            </Col>
            <Col xs={24} sm={12}>
              <div style={{ display: "flex", alignItems: "center" }}>
                <span style={{ fontWeight: "500", marginRight: "8px", color: "#595959" }}>Description:</span>
                <span style={{ color: "#8c8c8c", fontSize: "14px" }}>{task.description}</span>
              </div>
            </Col>
            <Col xs={24} sm={12}>
              <div style={{ display: "flex", alignItems: "center" }}>
                <span style={{ fontWeight: "500", marginRight: "8px", color: "#595959" }}>Assigned Date:</span>
                <span style={{ color: "#8c8c8c", fontSize: "14px" }}>
                  {new Date(task.assignedDate).toLocaleDateString()}
                </span>
              </div>
            </Col>
            <Col xs={24} sm={12}>
              <div style={{ display: "flex", alignItems: "center" }}>
                <span style={{ fontWeight: "500", marginRight: "8px", color: "#595959" }}>Due Date:</span>
                <span
                  style={{
                    color: isOverdue(task.dueDate) ? "#ff4d4f" : "#8c8c8c",
                    fontSize: "14px",
                    display: "flex",
                    alignItems: "center"
                  }}
                >
                  {new Date(task.dueDate).toLocaleDateString()}
                  {isOverdue(task.dueDate) && (
                    <span style={{ color: "#ff4d4f", marginLeft: "8px", fontWeight: "500" }}>
                      (Overdue)
                    </span>
                  )}
                </span>
              </div>
            </Col>
            <Col xs={24} sm={12}>
              <div style={{ display: "flex", alignItems: "center" }}>
                <span style={{ fontWeight: "500", marginRight: "8px", color: "#595959" }}>Status:</span>
                <span style={{ color: "#8c8c8c", fontSize: "14px" }}>{task.status}</span>
              </div>
            </Col>
            <Col xs={24} sm={12}>
              <div style={{ display: "flex", alignItems: "center" }}>
                <span style={{ fontWeight: "500", marginRight: "8px", color: "#595959" }}>Assigned By:</span>
                <span style={{ color: "#8c8c8c", fontSize: "14px" }}>{task.assignedBy}</span>
              </div>
            </Col>
            <Col xs={24} sm={12}>
              <div style={{ display: "flex", alignItems: "center" }}>
                <span style={{ fontWeight: "500", marginRight: "8px", color: "#595959" }}>Employee ID:</span>
                <span style={{ color: "#8c8c8c", fontSize: "14px" }}>{task.employeeId}</span>
              </div>
            </Col>
            <Col xs={24} sm={12}>
              <div style={{ display: "flex", alignItems: "center" }}>
                <span style={{ fontWeight: "500", marginRight: "8px", color: "#595959" }}>Feature ID:</span>
                <span style={{ color: "#8c8c8c", fontSize: "14px" }}>{task.featureId}</span>
              </div>
            </Col>
          </Row>
        </Card>
      ))}
    </div>
  ) : (
    <div style={{ 
      padding: "40px 20px", 
      textAlign: "center", 
      color: "#8c8c8c", 
      fontSize: "16px",
      backgroundColor: "#fafafa",
      borderRadius: "0 0 8px 8px"
    }}>
      No tasks yet!
    </div>
  )}
</Card>
          </div>
        </Spin>
      </PageContent>
    </>
  );
};

export default UserDetails;
