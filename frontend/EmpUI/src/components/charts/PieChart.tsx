import { useEffect, useState } from "react";
import { Pie } from "@ant-design/plots";

// Define the type for the data objects
interface PieData {
  type: string;
  value: number;
}

const PieChart = () => {
  const [data, setData] = useState<PieData[]>([]); // Explicitly type the state as an array of PieData

  useEffect(() => {
    setTimeout(() => {
      setData([
        { type: ".NET", value: 27 },
        { type: "Node.js", value: 25 },
        { type: "Python", value: 18 },
        { type: "Laravel", value: 15 },
        { type: "Java", value: 10 },
        { type: "Android", value: 5 },
      ]);
    }, 1000);
  }, []);

  const config = {
    data,
    angleField: "value",
    colorField: "type",
    label: {
      text: "value",
      style: {
        fontWeight: "bold",
      },
    },
    legend: {
      color: {
        title: false,
        position: "right",
        rowPadding: 5,
      },
    },
  };

  return <Pie {...config} />;
};

export default PieChart; // Export the component for use in other files
