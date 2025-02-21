import React from 'react';

const PageContent = ({
  children
}: {
  children: React.ReactNode;
}) => {
  
  return (
    <div className="p-6">
      {children}
    </div>
  );
};

export default PageContent;
