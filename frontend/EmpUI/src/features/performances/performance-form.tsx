
import { useEffect } from "react";
import { Button, Card, Col, Form, Row, Rate, Input } from "antd";
import { SaveOutlined } from "@ant-design/icons";
import { Performance } from "@models/performance-model"; // Assuming you have this model
import { validationMessage } from "@utils/helpers/message-helpers";
import { usePerformanceForm } from "@hooks/use-performances";

const { TextArea } = Input;

interface PerformanceFormProps {
  initialValues?: Performance;
  employeeId: string;
  isEditMode?: boolean;
}

const PerformanceForm = ({
  initialValues,
  employeeId,
  isEditMode = false,
}: PerformanceFormProps) => {
  const [form] = Form.useForm();
  const { onSaved, isLoading } = usePerformanceForm();

  useEffect(() => {
    if (initialValues) {
      form.setFieldsValue({
        ...initialValues,
        rating: initialValues.rating ? parseInt(initialValues.rating) : 0,
      });
    }
  }, [initialValues, form]);

  const onFinished = (values: Performance) => {
    // Prepare the performance data for submission
    const performanceData: Performance = {
      id: initialValues.id,
      rating: values.rating.toString(),
      comments: values.comments,
      employeeId: employeeId,
      // date: new Date().toISOString(),
      reviewerId: initialValues.reviewId,
    };

    console.log("onfinished form ", performanceData);
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
          isEditMode ? "Update Performance Review" : "Add Performance Review"
        }
      >
        <Row gutter={24}>
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
            {isEditMode ? "Update Performance" : "Save Performance"}
          </Button>
        </Col>
      </Row>
    </Form>
  );
};

export default PerformanceForm;
