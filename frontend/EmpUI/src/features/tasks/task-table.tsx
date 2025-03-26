import { useState } from "react";
import { Table, Input, Card } from "antd";
import useFilter from "@hooks/utility-hooks/use-filter";
import TableActions, { columns } from "./task-table-columns";

const TaskTable = () => {
  

  const handleStatusChange = (id: number, status: string) => {
    console.log("Status changed:", id, status);
  };

  const handleDelete = (id: number) => {
    console.log("Deleting:", id);
  };

  return (
    <TableActions onStatusChange={handleStatusChange} onDelete={handleDelete} />
  );
};

export default TaskTable;
