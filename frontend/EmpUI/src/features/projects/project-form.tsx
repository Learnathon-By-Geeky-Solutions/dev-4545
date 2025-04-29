import React, { useEffect } from 'react';
import { Form, Card, Row, Col, Button, Input, DatePicker, Spin } from 'antd';
import { SaveOutlined } from '@ant-design/icons';
import { useProject, useProjectForm } from '@hooks/use-projects';
import moment from 'moment';

const { TextArea } = Input;

const ProjectForm = ({ initialValues, isEditMode = true, projectId }) => {
  const [form] = Form.useForm();
  const { project, loading: projectLoading, error } = useProject(projectId);
  const { onSaved, isLoading } = useProjectForm();

  // Create formatted initial values with proper moment conversion
  const formattedInitialValues = {
    projectName: initialValues?.projectName || '',
    startDate: initialValues?.startDate ? moment(initialValues.startDate) : null,
    endDate: initialValues?.endDate ? moment(initialValues.endDate) : null,
    description: initialValues?.description || '',
    client: initialValues?.client || '',
  };

  // Update form when project data changes
  useEffect(() => {
    if (project && isEditMode) {
      form.setFieldsValue({
        ...project,
        startDate: project.startDate ? moment(project.startDate) : null,
        endDate: project.endDate ? moment(project.endDate) : null,
      });
    }
  }, [project, isEditMode, form]);

  const onFinished = (values) => {
    const projectData = {
      ...values,
      startDate: values.startDate?.toISOString(),
      endDate: values.endDate?.toISOString(),
    };
    onSaved(projectData, isEditMode, projectId);
  };

  return (
    <div style={{ padding: '24px', background: '#f0f2f5', minHeight: '100vh' }}>
      <Form
        form={form}
        layout="vertical"
        autoComplete="off"
        initialValues={formattedInitialValues}
        onFinish={onFinished}
      >
        <Card
          title={isEditMode ? 'Edit Project' : 'Create Project'}
          style={{
            borderRadius: 12,
            boxShadow: '0 4px 12px rgba(0, 0, 0, 0.1)',
            overflow: 'hidden',
          }}
        >
          {projectLoading ? (
            <Spin size="large" style={{ display: 'block', textAlign: 'center', marginTop: 50 }} />
          ) : error ? (
            <div style={{ textAlign: 'center', color: 'red' }}>
              Failed to load project data. Please try again.
            </div>
          ) : (
            <Row gutter={24}>
              <Col xs={24} sm={12}>
                <Form.Item
                  label="Project Name"
                  name="projectName"
                  rules={[{ required: true, message: 'Please enter project name' }]}
                >
                  <Input placeholder="Enter project name" />
                </Form.Item>
              </Col>
              <Col xs={24} sm={12}>
                <Form.Item
                  label="Client"
                  name="client"
                  rules={[{ required: true, message: 'Please enter client' }]}
                >
                  <Input placeholder="Enter client name" />
                </Form.Item>
              </Col>
              <Col xs={24} sm={12}>
                <Form.Item
                  label="Start Date"
                  name="startDate"
                  rules={[{ required: true, message: 'Please enter start date' }]}
                >
                  <DatePicker
                    showTime
                    format="YYYY-MM-DD HH:mm:ss"
                    style={{ width: '100%' }}
                    placeholder="Select start date and time"
                    disabledDate={(current) => current && current < moment().startOf('day')}
                  />
                </Form.Item>
              </Col>
              <Col xs={24} sm={12}>
                <Form.Item
                  label="End Date"
                  name="endDate"
                  rules={[
                    { required: true, message: 'Please enter end date' },
                    ({ getFieldValue }) => ({
                      validator(_, value) {
                        if (!value || !getFieldValue('startDate') || value >= getFieldValue('startDate')) {
                          return Promise.resolve();
                        }
                        return Promise.reject(new Error('End date cannot be before start date'));
                      },
                    }),
                  ]}
                >
                  <DatePicker
                    showTime
                    format="YYYY-MM-DD HH:mm:ss"
                    style={{ width: '100%' }}
                    placeholder="Select end date and time"
                    disabledDate={(current) => current && current < moment().startOf('day')}
                  />
                </Form.Item>
              </Col>
              <Col xs={24}>
                <Form.Item
                  label="Description"
                  name="description"
                  rules={[{ required: true, message: 'Please enter description' }]}
                >
                  <TextArea
                    rows={4}
                    placeholder="Enter project description"
                    style={{ resize: 'none' }}
                  />
                </Form.Item>
              </Col>
            </Row>
          )}
        </Card>
        <Row className="my-6">
          <Col span={24} className="text-right">
            <Button
              type="primary"
              htmlType="submit"
              icon={<SaveOutlined />}
              loading={isLoading}
              style={{ borderRadius: 8 }}
            >
              Save Changes
            </Button>
          </Col>
        </Row>
      </Form>
    </div>
  );
};

export default ProjectForm;
