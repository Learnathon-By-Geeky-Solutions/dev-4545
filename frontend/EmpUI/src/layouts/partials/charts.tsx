import { LineChart, PieChart } from "@/components/charts";

const chartConfigs = [
  { Component: LineChart },
  { Component: PieChart },
  { Component: LineChart },
  { Component: LineChart },
  { Component: LineChart }
];

const Charts = () => {
  return (
    <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
      {chartConfigs.map((chart, index) => (
        <div key={index} className="bg-white p-4 rounded-lg shadow-md">
          <chart.Component />
        </div>
      ))}
    </div>
  );
};

export default Charts;
