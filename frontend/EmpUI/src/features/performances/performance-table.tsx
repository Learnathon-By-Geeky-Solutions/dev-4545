import { useState } from "react";
import { Table, Input, Card } from "antd";
import useFilter from "@hooks/utility-hooks/use-filter";
import TableActions from "./performance-table-columns";
import { useDeletePerformanceMutation } from "@services/performance-service";

const TaskTable = () => {
  const [deletePerformance] = useDeletePerformanceMutation();

  const handleStatusChange = (id: number, status: string) => {
    console.log("Status changed:", id, status);
  };

  const handleDelete = async (performanceId: number) => {
    console.log("Deleting:", performanceId);
    try {
      await deletePerformance(performanceId).unwrap();
      console.log("Deleted:", performanceId);
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
