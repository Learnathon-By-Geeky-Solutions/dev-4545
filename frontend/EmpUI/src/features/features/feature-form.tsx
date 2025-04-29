import React, { useEffect } from 'react';
import { Form, Card, Row, Col, Button, Input, DatePicker, Select, Spin } from 'antd';
import { SaveOutlined } from '@ant-design/icons';
import { useFeature, useFeatureForm } from '@hooks/use-features'; // Assumed hooks
import { useProjects } from '@hooks/use-projects'; // Assumed hook for projects
import { validationMessage } from '@utils/helpers/message-helpers';
import moment from 'moment';

const { TextArea } = Input;

const FeatureForm = ({ isEditMode = false, featureId, initialValues = {} }) => {
  const [form] = Form.useForm();
  const { feature, loading: featureLoading, error } = useFeature(featureId); // Fetch feature data
  const { onSaved, isLoading } = useFeatureForm();
  const { isLoading: projectsLoading, data: projects = [] } = useProjects(); // Fetch project options

  // Format project options for the dropdown
  const projectOptions = projects.map((project) => ({
    label: project.projectName,
    value: project.projectId,
  }));

  // Format initialValues based on feature data or initialValues prop
  const formInitialValues = {
    featureName: feature?.featureName || initialValues.featureName || '',
    projectId: feature?.projectId || initialValues.projectId || undefined,
    startDate: feature?.startDate ? moment(feature.startDate) : initialValues.startDate ? moment(initialValues.startDate) : null,
    endDate: feature?.endDate ? moment(feature.endDate) : initialValues.endDate ? moment(initialValues.endDate) : null,
    description: feature?.description || initialValues.description || '',
  };

  // Update form when feature data changes
  useEffect(() => {
    if ((feature || initialValues) && isEditMode) {
      form.setFieldsValue(formInitialValues);
    }
  }, [feature, initialValues, isEditMode, form]);

  // onFinished function
  const onFinished = (values) => {
    // Format dates to ISO 8601 strings
    if (values.startDate) {
      values.startDate = values.startDate.toISOString();
    }
    if (values.endDate) {
      values.endDate = values.endDate.toISOString();
    }

    const featureData = { ...values };

    console.log('Formatted feature data:', featureData);

    // Call onSaved with the formatted data and isEditMode
    onSaved(featureData, isEditMode);
  };

  return (
    <div style={{ padding: '24px', background: '#f0f2f5', minHeight: '100vh' }}>
      <Form
        form={form}
        layout="vertical"
        autoComplete="off"
        initialValues={formInitialValues}
        onFinish={onFinished}
      >
        <Card
          title={isEditMode ? 'Update Feature' : 'Create Feature'}
          style={{
            borderRadius: 12,
            boxShadow: '0 4px 12px rgba(0, 0, 0, 0.1)',
            overflow: 'hidden',
          }}
        >
          {featureLoading ? (
            <Spin size="large" style={{ display: 'block', textAlign: 'center', marginTop: 50 }} />
          ) : error ? (
            <div style={{ textAlign: 'center', color: 'red' }}>
              Failed to load feature data. Please try again.
            </div>
          ) : (
            <Row gutter={24}>
              <Col xs={24} sm={12}>
                <Form.Item
                  label="Feature Name"
                  name="featureName"
                  rules={[{ required: true, message: validationMessage('feature name') }]}
                >
                  <Input placeholder="Enter feature name" />
                </Form.Item>
              </Col>
              <Col xs={24} sm={12}>
                <Form.Item
                  label="Project"
                  name="projectId"
                  rules={[{ required: true, message: validationMessage('project') }]}
                >
                  <Select
                    options={projectOptions}
                    placeholder="Select project"
                    loading={projectsLoading}
                    showSearch
                    filterOption={(input, option) =>
                      (option?.label?.toString() || '')
                        .toLowerCase()
                        .includes(input.toLowerCase())
                    }
                  />
                </Form.Item>
              </Col>
              <Col xs={24} sm={12}>
                <Form.Item
                  label="Start Date"
                  name="startDate"
                  rules={[{ required: true, message: validationMessage('start date') }]}
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
                    { required: true, message: validationMessage('end date') },
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
                  rules={[{ required: true, message: validationMessage('description') }]}
                >
                  <TextArea
                    rows={4}
                    placeholder="Enter feature description"
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
              {isEditMode ? 'Update Feature' : 'Create Feature'}
            </Button>
          </Col>
        </Row>
      </Form>
    </div>
  );
};

export default FeatureForm;