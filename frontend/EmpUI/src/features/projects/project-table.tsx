import { useState } from "react";
import { Table, Input, Card } from "antd";
import useFilter from "@hooks/utility-hooks/use-filter";
import TableActions, { columns } from "./project-table-columns";
import { useTaskForm } from "@hooks/use-tasks";
import { useDeleteProjectMutation } from "@services/project-service";

const ProjectTable = () => {
  const [deleteTask] = useDeleteProjectMutation();

  const handleStatusChange = (id: number, status: string) => {
    console.log("Status changed:", id, status);
  };

  const handleDelete = async (projectId: number) => {
    console.log("Deleting:", projectId);
    try {
      await deleteTask(projectId).unwrap();
      console.log("Deleted:", projectId);
      // Refresh the task list
    } catch (error) {
      console.log(error);
    }
  };

  return (
    <TableActions onStatusChange={handleStatusChange} onDelete={handleDelete} />
  );
};

export default ProjectTable;
