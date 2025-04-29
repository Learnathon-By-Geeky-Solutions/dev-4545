import React, { useEffect } from 'react';
import { Form, Card, Row, Col, Button, Input, DatePicker, Select, Spin } from 'antd';
import { SaveOutlined } from '@ant-design/icons';
import { useEmpLeave, useLeaveForm } from '@hooks/use-leaves';
import moment from 'moment';

const { TextArea } = Input;

const LeaveApplicationForm = ({ isEditMode = false }) => {
  const [form] = Form.useForm();
  const empId = localStorage.getItem('employeeId');
  const { leave, loading: leaveLoading, error } = useEmpLeave(empId); // Call hook inside component

  // Format initialValues based on leave data
  const initialValues = {
    startDate: leave?.startDate ? moment(leave.startDate) : null,
    endDate: leave?.endDate ? moment(leave.endDate) : null,
    leaveType: leave?.leaveType || undefined,
    reason: leave?.reason || '',
    employeeId: empId || '',
    status: leave?.status || 0, // Add status field with default value 0
    leaveId: leave?.leaveId || null, // Add leaveId field
  };

  // Update form when leave data changes
  useEffect(() => {
    if (leave && isEditMode) {
      form.setFieldsValue(initialValues);
    }
  }, [leave, isEditMode, form]);
  
  const { onSaved, isLoading } = useLeaveForm();
  
  // Leave type options
  const LEAVE_TYPES = [
    { value: 'sick', label: 'Sick Leave' },
    { value: 'vacation', label: 'Vacation' },
    { value: 'personal', label: 'Personal Leave' },
    { value: 'maternity', label: 'Maternity Leave' },
    { value: 'paternity', label: 'Paternity Leave' },
  ];

  // Status options
  const STATUS_OPTIONS = [
    { value: 0, label: 'Pending' },
    { value: 1, label: 'Approved' },
  ];
  
  // Updated onFinished function
  const onFinished = (values) => {
    // Format dates to strings
    if (values.startDate) {
      values.startDate = values.startDate.format('YYYY-MM-DD');
    }
    if (values.endDate) {
      values.endDate = values.endDate.format('YYYY-MM-DD');
    }

    // For new applications, ensure status is set to 0 (Pending)
    if (!isEditMode) {
      values.status = 0;
    }

    // Include leaveId only when in edit mode
    const leaveData = { ...values };
    if (!isEditMode) {
      delete leaveData.leaveId;
    }

    console.log('Formatted leave data:', leaveData);

    // Call onSaved with the formatted data, isEditMode, and leaveId
    onSaved(leaveData, isEditMode, values.employeeId);
  };

  return (
    <div style={{ padding: '24px', background: '#f0f2f5', minHeight: '100vh' }}>
      <Form
        form={form}
        layout="vertical"
        autoComplete="off"
        initialValues={initialValues}
        onFinish={onFinished}
      >
        <Card
          title={isEditMode ? 'Edit Leave Application' : 'Apply for Leave'}
          style={{
            borderRadius: 12,
            boxShadow: '0 4px 12px rgba(0, 0, 0, 0.1)',
            overflow: 'hidden',
          }}
        >
          {leaveLoading ? (
            <Spin size="large" style={{ display: 'block', textAlign: 'center', marginTop: 50 }} />
          ) : error ? (
            <div style={{ textAlign: 'center', color: 'red' }}>
              Failed to load leave data. Please try again.
            </div>
          ) : (
            <Row gutter={24}>
              {/* Include leaveId as a hidden field when in edit mode */}
              {isEditMode && (
                <Form.Item name="leaveId" hidden>
                  <Input />
                </Form.Item>
              )}
              <Col xs={24} sm={12}>
                <Form.Item
                  label="Start Date"
                  name="startDate"
                  rules={[{ required: true }]}
                >
                  <DatePicker
                    format="YYYY-MM-DD"
                    style={{ width: '100%' }}
                    placeholder="Select Start Date"
                    disabledDate={(current) => current && current < moment().startOf('day')}
                  />
                </Form.Item>
              </Col>
              <Col xs={24} sm={12}>
                <Form.Item
                  label="End Date"
                  name="endDate"
                  rules={[
                    { required: true },
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
                    format="YYYY-MM-DD"
                    style={{ width: '100%' }}
                    placeholder="Select End Date"
                    disabledDate={(current) => current && current < moment().startOf('day')}
                  />
                </Form.Item>
              </Col>
              <Col xs={24} sm={12}>
                <Form.Item
                  label="Leave Type"
                  name="leaveType"
                  rules={[{ required: true }]}
                >
                  <Select
                    options={LEAVE_TYPES}
                    placeholder="Select Leave Type"
                    style={{ width: '100%' }}
                  />
                </Form.Item>
              </Col>
              <Col xs={24} sm={12}>
                <Form.Item
                  label="Employee ID"
                  name="employeeId"
                  rules={[{ required: true }]}
                >
                  <Input placeholder="Employee ID" disabled />
                </Form.Item>
              </Col>
              {/* Show status field only in edit mode */}
              {isEditMode && (
                <Col xs={24} sm={12}>
                  <Form.Item
                    label="Status"
                    name="status"
                    rules={[{ required: true }]}
                  >
                    <Select
                      options={STATUS_OPTIONS}
                      placeholder="Select Status"
                      style={{ width: '100%' }}
                    />
                  </Form.Item>
                </Col>
              )}
              <Col xs={24}>
                <Form.Item
                  label="Reason"
                  name="reason"
                  rules={[{ required: true }]}
                >
                  <TextArea
                    rows={4}
                    placeholder="Enter reason for leave"
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
              {isEditMode ? 'Update Leave' : 'Save Changes'}
            </Button>
          </Col>
        </Row>
      </Form>
    </div>
  );
};

export default LeaveApplicationForm;