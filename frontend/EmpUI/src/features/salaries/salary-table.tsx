import { useState } from "react";
import { Table, Input, Card } from "antd";
import useFilter from "@hooks/utility-hooks/use-filter";
import TableActions, { columns } from "./salary-table-columns";
import { useTaskForm } from "@hooks/use-salaries";
import { useDeleteSalaryMutation } from "@services/salary-service";

const SalaryTable = () => {
  const [deleteSalary] = useDeleteSalaryMutation();

  const handleStatusChange = (id: number, status: string) => {
    console.log("Status changed:", id, status);
  };

  const handleDelete = async (salaryId: number) => {
    console.log("Deleting:", salaryId);
    try {
      await deleteSalary(salaryId).unwrap();
      console.log("Deleted:", salaryId);
      // Refresh the task list
    } catch (error) {
      console.log(error);
    }
  };

  return (
    <TableActions onStatusChange={handleStatusChange} onDelete={handleDelete} />
  );
};

export default SalaryTable;
