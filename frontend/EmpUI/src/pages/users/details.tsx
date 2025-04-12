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

const UserDetails = () => {
  const { id: empId } = useParams();
  const [showPerformanceForm, setShowPerformanceForm] = useState(false);

  const { isLoading, user: employeeDetails } = useUser(empId);
  const { performance } = usePerformance(empId);

  // console.log("performance ", performance);

  const handleEditPerformance = () => {
    setShowPerformanceForm(true);
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
                employeeId={empId}
                isEditMode={true}
              />
            ) : null}
          </div>
        </Spin>
      </PageContent>
    </>
  );
};

export default UserDetails;
