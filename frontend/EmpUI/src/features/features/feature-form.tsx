import React, { useEffect } from 'react';
import { Form, Card, Row, Col, Button, Input, DatePicker, Select, Spin } from 'antd';
import { SaveOutlined } from '@ant-design/icons';
import { useFeature, useFeatureForm } from '@hooks/use-features';
import { useProjects } from '@hooks/use-projects';
import { validationMessage } from '@utils/helpers/message-helpers';
import moment from 'moment';

const { TextArea } = Input;
interface FeatureFormProps {
 
  isEditMode?: boolean;
}
const FeatureForm = ({ initialValues, isEditMode=false, featureId}: FeatureFormProps) => {
  const [form] = Form.useForm();
  const { feature, loading: featureLoading, error } = useFeature(featureId);
  const { onSaved, isLoading } = useFeatureForm();
  const { isLoading: projectsLoading, data: projects = [] } = useProjects();

  const projectOptions = projects.map((project) => ({
    label: project.projectName,
    value: project.projectId,
  }));

  // Proper initial values with moment conversion
  const formInitialValues = {
    featureName: initialValues?.featureName || '',
    projectId: initialValues?.projectId || undefined,
    startDate: initialValues?.startDate ? moment(initialValues.startDate) : null,
    endDate: initialValues?.endDate ? moment(initialValues.endDate) : null,
    description: initialValues?.description || '',
  };

  // Corrected useEffect with proper field names
  useEffect(() => {
    if (initialValues) {
      form.setFieldsValue({
        ...initialValues,
        startDate: initialValues.startDate ? moment(initialValues.startDate) : null,
        endDate: initialValues.endDate ? moment(initialValues.endDate) : null,
      });
    }
  }, [initialValues, form,isEditMode]);

  const onFinished = (values) => {
    const formattedValues = {
      ...values,
      startDate: values.startDate?.toISOString(),
      endDate: values.endDate?.toISOString(),
    };
    console.log("onfinidhes", formattedValues)
    onSaved(formattedValues, isEditMode, featureId);
  };

  return (
    <div style={{ padding: '24px', background: '#f0f2f5', minHeight: '100vh' }}>
      <Form
        form={form}
        layout="vertical"
        autoComplete="off"
        initialValues={formInitialValues} // Use properly formatted initial values
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