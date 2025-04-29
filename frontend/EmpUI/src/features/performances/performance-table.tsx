import { useState } from "react";
import { Table, Input, Card } from "antd";
import useFilter from "@hooks/utility-hooks/use-filter";
import TableActions from "./performance-table-columns";
import { useDeletePerformanceMutation } from "@services/performance-service";

const PerformanceTable = () => {
  const [deletePerformance] = useDeletePerformanceMutation();

  const handleStatusChange = (id: number, status: string) => {
    console.log("Status changed:", id, status);
  };
  return <TableActions onStatusChange={handleStatusChange} />;
};

export default PerformanceTable;
