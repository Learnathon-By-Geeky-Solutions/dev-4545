import { useState } from 'react';
import { Table, Input, Card } from 'antd';
import { useUsers } from '@hooks/use-users';
import useFilter from '@hooks/utility-hooks/use-filter';
import { columns } from './user-table-columns';

const UserTable = () => {
  const {
    isLoading,
    data
  } = useUsers();

  // console.log('all employee data ', data);
  
  const { getQueryParams, setQueryParams, sortTableColumn } = useFilter();
  const [search, setSearch] = useState(getQueryParams().search as string);

  const onSearchHandle = (value: string) => {
    setQueryParams({
      ...getQueryParams(),
      search: value
    });
  };
  
  return (
    <Card
      title={"Users"}
      extra={
        <div className="my-6">
          <Input.Search
            placeholder={"Search"}
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            onSearch={onSearchHandle}
            allowClear
          />
        </div>
      }
    >
      <Table
        columns={columns}
        dataSource={data || []}
        loading={isLoading}
        pagination={false}
        onChange={sortTableColumn}
        scroll={{ y: 350 }}
        rowKey="employeeId"
        bordered
      />
      {/*<div className={'flex justify-end mt-4'}>*/}
      {/*  <PaginationWrapper totalItems={data?.totalNumberOfElemements || 0} />*/}
      {/*</div>*/}
    </Card>
  );
};

export default UserTable;
