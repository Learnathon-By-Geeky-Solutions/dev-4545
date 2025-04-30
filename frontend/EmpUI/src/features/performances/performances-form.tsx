import { useEffect } from "react";
import { Button, Card, Col, Form, Row, Rate, Input, Select, DatePicker } from "antd";
import { SaveOutlined } from "@ant-design/icons";
import { Performance } from "@models/performance-model";
import { validationMessage } from "@utils/helpers/message-helpers";
import { usePerformanceForm } from "@hooks/use-performances";
import { useUsers } from "@hooks/use-users";
import moment from "moment";

const { TextArea } = Input;

interface PerformanceFormProps {
  initialValues?: Performance;
  isEditMode?: boolean;
}

const PerformanceForm = ({ initialValues, isEditMode = false }: PerformanceFormProps) => {
  const [form] = Form.useForm();
  const { onSaved, isLoading } = usePerformanceForm();
  const { isLoading: employeesLoading, data: employees = [] } = useUsers();
  const empId = localStorage.getItem("employeeId"); // Reviewer ID

  const employeeOptions = employees.map((employee) => ({
    label: employee.name,
    value: employee.employeeId,
  }));

  useEffect(() => {
    form.resetFields();
    
    if (initialValues) {
      // Date handling remains the same
      let dateValue: moment.Moment | null = null;
      if (initialValues.date) {
        dateValue = moment(initialValues.date);
      }

      form.setFieldsValue({
        ...initialValues,
        rating: initialValues.rating ? parseInt(initialValues.rating, 10) : 0,
        date: dateValue,
      });
    }
  }, [initialValues, form]);

  const onFinished = (values: any) => {
    // Validate reviewer is logged in
    if (!empId) {
      form.setFields([
        {
          name: "reviewerId",
          errors: ["Reviewer must be logged in to submit a performance review"],
        },
      ]);
      return;
    }

    const performanceData: Performance = {
      ...values,
      id: isEditMode ? initialValues?.id : undefined,
      rating: values.rating.toString(),
      date: values.date?.format("YYYY-MM-DD"),
      reviewerId: empId, // Always use logged in reviewer ID
    };

    console.log("Submission payload:", performanceData);
    onSaved(performanceData, isEditMode);
  };

  return (
    <Form
      form={form}
      layout="vertical"
      autoComplete="off"
      initialValues={initialValues}
      onFinish={onFinished}
    >
      <Card title={isEditMode ? "Update Performance Review" : "Add Performance Review"}>
        <Row gutter={24}>
          <Col span={24}>
            <Form.Item
              label="Employee"
              name="employeeId"
              rules={[{ required: true, message: validationMessage("employee") }]}
            >
              <Select
                options={employeeOptions}
                placeholder="Select employee"
                loading={employeesLoading}
                showSearch
                filterOption={(input, option) =>
                  (option?.label?.toString() || "")
                    .toLowerCase()
                    .includes(input.toLowerCase())
                }
              />
            </Form.Item>
          </Col>

          {/* Rest of the form fields remain the same */}
          <Col span={24}>
            <Form.Item
              label="Date"
              name="date"
              rules={[{ required: true, message: validationMessage("date") }]}
            >
              <DatePicker
                format="YYYY-MM-DD"
                placeholder="Select date"
                style={{ width: "100%" }}
              />
            </Form.Item>
          </Col>
          <Col span={24}>
            <Form.Item
              label="Rating"
              name="rating"
              rules={[{ required: true, message: validationMessage("rating") }]}
            >
              <Rate />
            </Form.Item>
          </Col>
          <Col span={24}>
            <Form.Item
              label="Comments"
              name="comments"
              rules={[{ required: true, message: validationMessage("comments") }]}
            >
              <TextArea rows={4} placeholder="Enter performance comments" />
            </Form.Item>
          </Col>
        </Row>
      </Card>

      <Row className="my-6">
        <Col span={24} className="text-right">
          <Button
            type="primary"
            htmlType="submit"
            icon={<SaveOutlined />}
            loading={isLoading}
          >
            {isEditMode ? "Update Performance" : "Save Performance"}
          </Button>
        </Col>
      </Row>
    </Form>
  );
};

export default PerformanceForm;