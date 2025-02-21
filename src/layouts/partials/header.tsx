import { Layout } from 'antd';
import GlobalSearch from '@components/shared/global-search';

const { Header: AntdHeader} = Layout;

const Header = () => {
  return (
    <AntdHeader className={'p-4 bg-white border-b'}>
      <GlobalSearch />
    </AntdHeader>
  );
};

export default Header;