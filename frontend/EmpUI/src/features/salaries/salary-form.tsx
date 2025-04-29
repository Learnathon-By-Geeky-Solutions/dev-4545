import { useEffect } from "react";
import { Button, Card, Col, Form, Row, Rate, Input, Select, DatePicker } from "antd";
import { SaveOutlined } from "@ant-design/icons";
import { Performance } from "@models/performance-model"; // Assuming you have this model
import { validationMessage } from "@utils/helpers/message-helpers";
import { usePerformanceForm } from "@hooks/use-performances";
import { useUsers } from "@hooks/use-users";
import moment from "moment"; // For handling date formatting

const { TextArea } = Input;

interface PerformanceFormProps {
  initialValues?: Performance;
  employeeId?: string; // Made optional since employeeId will be selected via form
  isEditMode?: boolean;
}

const empId = localStorage.getItem("employeeId");

const PerformanceForm = ({
  initialValues,
  employeeId,
  isEditMode = false,
}: PerformanceFormProps) => {
  const [form] = Form.useForm();
  const { onSaved, isLoading } = usePerformanceForm();
  const { isLoading: employeesLoading, data: employees = [] } = useUsers();

  // Format employee options for the dropdown
  const employeeOptions = employees.map((employee) => ({
    label: employee.name,
    value: employee.employeeId,
  }));

  useEffect(() => {
    // Reset form to clear any stale state
    form.resetFields();

    if (initialValues) {
      // Log initialValues.date for debugging
      console.log(
        "initialValues.date:",
        initialValues.date,
        typeof initialValues.date,
        JSON.stringify(initialValues.date)
      );

      // Safely parse date
      let dateValue: moment.Moment | null = null;
      if (initialValues.date) {
        if (moment.isMoment(initialValues.date) && initialValues.date.isValid()) {
          dateValue = initialValues.date; // Use valid moment object
        } else if (typeof initialValues.date === "string" && initialValues.date.trim() !== "") {
          const parsedDate = moment(initialValues.date, ["YYYY-MM-DD", moment.ISO_8601], true);
          dateValue = parsedDate.isValid() ? parsedDate : null; // Parse valid string or set null
        } else {
          console.warn("Invalid initialValues.date, setting to null:", initialValues.date);
        }
      }

      form.setFieldsValue({
        ...initialValues,
        rating: initialValues.rating ? parseInt(initialValues.rating, 10) : 0,
        date: dateValue, // Set valid moment object or null
      });
    } else if (employeeId) {
      // Pre-fill employeeId if provided (for new records)
      form.setFieldsValue({ employeeId });
    }
  }, [initialValues, employeeId, form, isEditMode]);

  const onFinished = (values: any) => {
    // Validate empId
    if (!empId && !isEditMode) {
      form.setFields([
        {
          name: "employeeId",
          errors: ["Please log in to submit a performance review."],
        },
      ]);
      return;
    }

    // Log values.date for debugging
    console.log("values.date:", values.date, typeof values.date, JSON.stringify(values.date));

    // Prepare the performance data for submission
    const performanceData: Performance = {
      ...values,
      id: isEditMode ? initialValues?.id : undefined, // Backend generates ID for new records
      rating: values.rating.toString(),
      date: values.date && moment.isMoment(values.date) && values.date.isValid()
        ? values.date.format("YYYY-MM-DD")
        : undefined, // Format valid date or set undefined
      reviewerId: isEditMode ? initialValues?.reviewerId : empId, // Use empId from localStorage
    };

    console.log("Payload sent to server:", JSON.stringify(performanceData, null, 2)); // Log for debugging
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
      <Card
        title={
          isEditMode ? "Update Salary Review" : "Add Salary Review"
        }
      >
        <Row gutter={24}>
          <Col span={24}>
            <Form.Item
              label="Employee"
              name="employeeId"
              rules={[
                { required: true, message: validationMessage("employee") },
              ]}
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
              rules={[
                { required: true, message: validationMessage("comments") },
              ]}
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
            {isEditMode ? "Update Salary" : "Save Salary"}
          </Button>
        </Col>
      </Row>
    </Form>
  );
};

export default PerformanceForm;