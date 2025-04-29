import TableActions from "./feature-table-columns";
import { useDeleteFeatureMutation } from "@services/feature-service";
import { useFeatureForm } from "@hooks/use-features";

const FeatureTable = () => {
  const [deleteFeature] = useDeleteFeatureMutation();
  const { onSaved, isLoading } = useFeatureForm();

  const editFeature = (featureId: string) => {
    const isEditMode = true;
    onSaved(featureId, isEditMode);
  };

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
    <TableActions onStatusChange={handleStatusChange} onEdit={editFeature} onDelete={handleDelete} />
  );
};

export default FeatureTable;
