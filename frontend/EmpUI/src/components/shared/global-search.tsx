import { Input, } from 'antd';

const GlobalSearch = () => {
  return (
    <div className="max-w-[550px]">
      <Input.Search
        enterButton
        allowClear
        placeholder={'Search for anything...'}
      />
    </div>
  );
};

export default GlobalSearch;
