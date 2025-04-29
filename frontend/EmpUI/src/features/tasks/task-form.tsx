import _ from "lodash";
import { useEffect } from "react";
import { Button, Card, Col, Form, Input, Row, Select, DatePicker } from "antd";
import { SaveOutlined } from "@ant-design/icons";
import { useUsers } from "@hooks/use-users";
import { useFeatures } from "@hooks/use-features";
import { validationMessage } from "@utils/helpers/message-helpers";
import { useTaskForm } from "@hooks/use-tasks";

// Define task statuses
const TASK_STATUSES = [
  {
    label: "Pending",
    value: "pending",
  },
  {
    label: "In Progress",
    value: "in_progress",
  },
  {
    label: "Completed",
    value: "completed",
  },
  {
    label: "Cancelled",
    value: "cancelled",
  },
];

// Task model interfaces
interface Task {
  description: string;
  assignedDate: string;
  dueDate: string;
  status: string;
  assignedBy: string;
  employeeId: string;
  featureId: string;
}

interface TaskPartial extends Partial<Task> {}

// Employee interface based on your API response
interface Employee {
  employeeId: string;
  name: string;
  stack: string;
  email: string;
  dateOfJoin: string;
  phone: string;
  role: number;
}

// Feature interface based on your API response
interface Feature {
  featureId: string;
  projectId: string;
  featureName: string;
  startDate: string;
  endDate: string;
  description: string;
}

interface TaskFormProps {
  initialValues?: TaskPartial;
  isEditMode?: boolean;
}

const TaskForm = ({ initialValues, isEditMode = false }: TaskFormProps) => {
  const [form] = Form.useForm();
  const { onSaved, isLoading } = useTaskForm();
  const { isLoading: employeesLoading, data: employees = [] } = useUsers();
  const { isLoading: featuresLoading, data: features = [] } = useFeatures();

  // Format employee options for the dropdown
  const employeeOptions = employees.map((employee) => ({
    label: employee.name,
    value: employee.employeeId,
  }));

  // Format feature options for the dropdown
  const featureOptions = features.map((feature) => ({
    label: feature.featureName,
    value: feature.featureId,
  }));

  useEffect(() => {
    if (initialValues) {
      form.setFieldsValue({
        ...initialValues,
        assignedDate: initialValues.assignedDate
          ? new Date(initialValues.assignedDate)
          : null,
        dueDate: initialValues.dueDate ? new Date(initialValues.dueDate) : null,
      });
    }
  }, [initialValues, form, isEditMode]);

  const onFinished = (values: any) => {
    // Format dates to ISO strings
    const taskData: Task = {
      ...values,
      assignedDate: values.assignedDate?.toISOString(),
      dueDate: values.dueDate?.toISOString(),
    };

    // Remove id if it's undefined (for creation)
    if (!taskData.id) {
      delete taskData.id;
    }

    onSaved(taskData, isEditMode);
  };

  return (
    <Form
      form={form}
      layout="vertical"
      autoComplete="off"
      initialValues={initialValues}
      onFinish={onFinished}
    >
      <Card title={isEditMode ? "Update Task" : "Create Task"}>
        <Row gutter={24}>
          <Col span={24}>
            <Form.Item
              label="Description"
              name="description"
              rules={[
                { required: true, message: validationMessage("description") },
              ]}
            >
              <Input.TextArea rows={4} placeholder="Task description" />
            </Form.Item>
          </Col>

          <Col span={12}>
            <Form.Item
              label="Assigned Date"
              name="assignedDate"
              rules={[
                { required: true, message: validationMessage("assigned date") },
              ]}
            >
              <DatePicker style={{ width: "100%" }} />
            </Form.Item>
          </Col>

          <Col span={12}>
            <Form.Item
              label="Due Date"
              name="dueDate"
              rules={[
                { required: true, message: validationMessage("due date") },
              ]}
            >
              <DatePicker style={{ width: "100%" }} />
            </Form.Item>
          </Col>

          <Col span={12}>
            <Form.Item
              label="Status"
              name="status"
              rules={[{ required: true, message: validationMessage("status") }]}
              initialValue="pending"
            >
              <Select options={TASK_STATUSES} placeholder="Select status" />
            </Form.Item>
          </Col>

          <Col span={12}>
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

          <Col span={12}>
            <Form.Item
              label="Feature"
              name="featureId"
              rules={[
                { required: true, message: validationMessage("feature") },
              ]}
            >
              <Select
                options={featureOptions}
                placeholder="Select feature"
                loading={featuresLoading}
                showSearch
                filterOption={(input, option) =>
                  (option?.label?.toString() || "")
                    .toLowerCase()
                    .includes(input.toLowerCase())
                }
              />
            </Form.Item>
          </Col>

          <Col span={12}>
            <Form.Item
              label="Assigned By"
              name="assignedBy"
              rules={[
                { required: true, message: validationMessage("assigned by") },
              ]}
            >
              <Select
                options={employeeOptions}
                placeholder="Select who assigned this task"
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
            {isEditMode ? "Update Task" : "Create Task"}
          </Button>
        </Col>
      </Row>
    </Form>
  );
};

export default TaskForm;
