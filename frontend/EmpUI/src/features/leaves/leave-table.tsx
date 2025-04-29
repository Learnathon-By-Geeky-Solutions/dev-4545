import { useDeleteLeaveMutation } from "@services/leave-service";
import TableActions from "./leave-table-columns";

const LeaveTable = () => {
  const [deleteLeave] = useDeleteLeaveMutation();

  const handleStatusChange = (id: number, status: string) => {
    console.log("Status changed:", id, status);
  };

  const handleDelete = async (employeeId: number) => {
    console.log("Deleting:", employeeId);
    try {
      await deleteLeave(employeeId).unwrap();
      console.log("Deleted:", employeeId);
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
