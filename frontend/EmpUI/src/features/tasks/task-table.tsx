import { useState } from "react";
import { Table, Input, Card } from "antd";
import useFilter from "@hooks/utility-hooks/use-filter";
import TableActions, { columns } from "./task-table-columns";
import { useTaskForm } from "@hooks/use-tasks";
import { useDeleteTaskMutation } from "@services/task-service";
import { useTaskSavedMutation } from "@services/task-service";

const TaskTable = () => {
  const [deleteTask] = useDeleteTaskMutation();
  const { onSaved, isLoading } = useTaskForm();

  const editTask = (taskId: string) => {
    const isEditMode = true;
    onSaved(taskId, isEditMode);
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

  return <TableActions onEdit={editTask} onDelete={handleDelete} />;
};

export default TaskTable;
