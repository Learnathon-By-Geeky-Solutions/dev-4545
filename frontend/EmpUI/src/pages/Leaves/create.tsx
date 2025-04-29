import React from 'react'
import PageContent from '@layouts/partials/page-content';
import PageHeader from '@layouts/partials/page-header';

import { useEmpLeave } from '@hooks/use-leaves';
import LeaveForm from '../../features/leaves/leave-form';

const LeavesCreate = () => {
    
  return (
    <>
   
    <PageHeader
        title={'Application for leave'}
      />
      <PageContent>
        <LeaveForm />
       
      </PageContent>
    </>
  )
}

export default LeavesCreate
