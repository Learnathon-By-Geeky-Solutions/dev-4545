import TableActions from "./feature-table-columns";
import { useDeleteFeatureMutation } from "@services/feature-service";

const FeatureTable = () => {
  const [deleteFeature] = useDeleteFeatureMutation();

  const handleStatusChange = (id: number, status: string) => {
    console.log("Status changed:", id, status);
  };

  const handleDelete = async (featureId: number) => {
    console.log("Deleting:", featureId);
    try {
      await deleteFeature(featureId).unwrap();
      console.log("Deleted:", featureId);
      // Refresh the task list
    } catch (error) {
      console.log(error);
    }
  };

  return (
    <TableActions onStatusChange={handleStatusChange} onDelete={handleDelete} />
  );
};

export default FeatureTable;
