import { useState } from "react";
import { Table, Input, Card } from "antd";
import useFilter from "@hooks/utility-hooks/use-filter";
import TableActions, { columns } from "./task-table-columns";
import { useTaskForm } from "@hooks/use-tasks";
import { useDeleteTaskMutation } from "@services/task-service";

const TaskTable = () => {
  const [deleteTask] = useDeleteTaskMutation();

  const handleStatusChange = (id: number, status: string) => {
    console.log("Status changed:", id, status);
  };

  const handleDelete = async (taskId: number) => {
    console.log("Deleting:", taskId);
    try {
      await deleteTask(taskId).unwrap();
      console.log("Deleted:", taskId);
      // Refresh the task list
    } catch (error) {
      console.log(error);
    }
  };

  return (
    <TableActions onStatusChange={handleStatusChange} onDelete={handleDelete} />
  );
};

export default TaskTable;
