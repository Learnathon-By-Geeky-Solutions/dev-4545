import { useState } from "react";
import { Table, Input, Card } from "antd";
import useFilter from "@hooks/utility-hooks/use-filter";
import { useDeleteLeaveMutation } from "@services/leave-service";
import TableActions from "./leave-table-columns";

const LeaveTable = () => {
  const [deleteLeave] = useDeleteLeaveMutation();

  const handleStatusChange = (id: number, status: string) => {
    console.log("Status changed:", id, status);
  };

  const handleDelete = async (leaveId: number) => {
    console.log("Deleting:", leaveId);
    try {
      await deleteLeave(leaveId).unwrap();
      console.log("Deleted:", leaveId);
      // Refresh the task list
    } catch (error) {
      console.log(error);
    }
  };

  return (
    <TableActions onStatusChange={handleStatusChange} onDelete={handleDelete} />
  );
};

export default LeaveTable;
