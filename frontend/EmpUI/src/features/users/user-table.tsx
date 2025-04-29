import { useEffect, useState } from "react";
import { Table, Input, Card } from "antd";
import { useUsers } from "@hooks/use-users";
import useFilter from "@hooks/utility-hooks/use-filter";
import { columns } from "./user-table-columns";

const UserTable = () => {
  const { isLoading, data } = useUsers();

  const { getQueryParams, setQueryParams, sortTableColumn } = useFilter();
  const [search, setSearch] = useState(getQueryParams().search as string);
  const [filteredUsers, setFilteredUsers] = useState([]);

  // Initialize filtered users with the raw data when it loads
  useEffect(() => {
    if (data) {
      setFilteredUsers(data);
    }
  }, [data]);

  const onSearchHandle = (value) => {
    setSearch(value);

    // Update URL params for consistency
    setQueryParams({
      ...getQueryParams(),
      search: value,
    });

    // Filter the data based on search term
    if (!value.trim()) {
      // If search is empty, show all users
      setFilteredUsers(data || []);
    } else {
      const searchTerm = value.toLowerCase();
      const filtered = data.filter((employee) => {
        const roleMatch =
          (searchTerm.includes("admin") && employee.role === 0) ||
          (searchTerm.includes("employee") && employee.role === 2) ||
          // Also match if user directly searches for "0" or "2"
          (employee.role !== undefined &&
            employee.role.toString() === searchTerm);

        return (
          // Name field
          (employee.name && employee.name.toLowerCase().includes(searchTerm)) ||
          // Email
          (employee.email &&
            employee.email.toLowerCase().includes(searchTerm)) ||
          // Stack
          (employee.stack &&
            employee.stack.toLowerCase().includes(searchTerm)) ||
          // Role - using our special handling logic
          roleMatch ||
          // Employee ID
          (employee.employeeId &&
            String(employee.employeeId).includes(searchTerm))
        );
      });
      setFilteredUsers(filtered);
    }
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
        dataSource={filteredUsers || []}
        loading={isLoading}
        pagination={false}
        onChange={sortTableColumn}
        scroll={{ y: 700 }}
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
